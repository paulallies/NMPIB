using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using NMPIB.Models.Repositories;
using NMPIB.Models;
using System.IO;
using System.Drawing;
using System.Net.Mime;
using System.Drawing.Drawing2D;

namespace NMPIB.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/
        IImageRepository imagedb = new ImageRepository();
        IMagazineRepository magdb = new MagazineRepository();
        IUserRepository userdb = new UserRepository();
        public ActionResult Index()
        {
            List<SelectListItem> Items = new List<SelectListItem>()
            {
            new SelectListItem() { Value = "datedesc", Text = "date descending", Selected = true },
            new SelectListItem() { Value = "dateasc", Text = "date ascending", Selected = false },
            new SelectListItem() { Value = "magazine", Text = "magazine", Selected = false }
            
            };
            ViewData["SearchOptions"] = new SelectList(Items, "Value","Text" );
            return View();

        }

        public ActionResult SLCreate()
        {
            return View();
        }

        public ActionResult GetImages(int pIndex, string SearchString, int PageSize, string sord)
        {


            
            int totalRecords = imagedb.GetAllImages().Count();
            if (SearchString != null && SearchString.Trim().Length > 0) totalRecords = imagedb.GetImagesBySearch(SearchString).Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)PageSize);

            //Get all images
            var images = imagedb.GetAllImagesPaged(pIndex, PageSize, sord);
            if (SearchString != null && SearchString.Trim().Length > 0) images = imagedb.GetImagesBySearchOrderPaged(pIndex, PageSize, SearchString, sord);


            var jsonData = new
            {
                totalPages = totalPages,
                pageIndex = pIndex,
                ImageList = (
                    from p in images
                    select new
                    {
                        id = p.id,
                        thumb_location = p.thumb_location,
                        preview_location = p.preview_location,
                        name = p.name,
                        magazine  = p.tbl_publication.name,
                        shootdate = p.shoot_date,
                        description = p.description
                    }).ToArray()
            };

            return Json(jsonData);

        }

        //  [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Details(int id, string Error)
        {
            try
            {
                ViewData["Error"] = Error;
                return View(imagedb.GetImageByID(id));
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        //  [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, string Error)
        {
            ViewData["Magazines"] = new SelectList(magdb.GetMagazines(), "id", "name");
            ViewData["Users"] = new SelectList(userdb.GetUsers(), "id", "username", userdb.getUserbyUsername(HttpContext.User.Identity.Name.ToString()).id);
            tbl_Image myImage = imagedb.GetImageByID(id);
            string UploadedBy = myImage.tbl_user.UserName.ToUpper();
            ViewData["UploadedBy"] = UploadedBy;

            try
            {
                ViewData["Error"] = Error;
                return View(myImage);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(tbl_Image postedmImage)
        {
            var returnUrl = Request["hdnReturnurl"];
            //Get image from the database
            tbl_Image myImage = imagedb.GetImageByID(postedmImage.id);

            //Get collections for dropdowns
            ViewData["Magazines"] = new SelectList(magdb.GetMagazines(), "id", "name");
            ViewData["Users"] = new SelectList(userdb.GetUsers(), "id", "username", userdb.getUserbyUsername(HttpContext.User.Identity.Name.ToString()).id);


            #region Validation of Input fields
            HttpPostedFileBase hpf = Request.Files["FileUp"] as HttpPostedFileBase;
            if (hpf.ContentLength > 0)
            {
                //Save updated file in photos folder as temp file
                if (hpf.ContentType != "image/jpeg")
                ModelState.AddModelError("", "Wrong File Type");
            }

            if (postedmImage.magazine_issue.Trim().Length == 0)
                ModelState.AddModelError("magazine_issue", "Magazine issue missing");
            if (postedmImage.shoot_description.Trim().Length == 0)
                ModelState.AddModelError("shoot_description", "Shoot description missing");
            try { DateTime testDate = (DateTime)postedmImage.shoot_date.Value; }
            catch { ModelState.AddModelError("shoot_date", "Invalid Shoot Date"); }
            if (postedmImage.description.Trim().Length == 0)
                ModelState.AddModelError("description", "Description issue missing");
            if (postedmImage.keywords.Trim().Length == 0)
                ModelState.AddModelError("keywords", "Keywords issue missing");

            if (!ModelState.IsValid)
            {
                return View(postedmImage);
            }
            #endregion

            //if validation is OK then execute the following

            string tempFileName = System.Guid.NewGuid().ToString("N") + ".jpg";
            if (hpf.ContentLength > 0)
            {
                //Save updated file in photos folder as temp file
                string filePath = "";
                filePath = Path.Combine(HttpContext.Server.MapPath("~/Photos"), Path.GetFileName(tempFileName));
                hpf.SaveAs(filePath);
            }

            //Update Image Object
            myImage.magazine_id = postedmImage.magazine_id;
            myImage.magazine_issue = postedmImage.magazine_issue;
            myImage.shoot_description = postedmImage.shoot_description;
            myImage.shoot_date = postedmImage.shoot_date;
            myImage.date_updated = DateTime.Now;
            myImage.keywords = postedmImage.keywords;

            string oldName = tempFileName;
            myImage.name = myImage.tbl_publication.name.Replace(' ', '_') +
            "_" +
            myImage.magazine_issue.Replace(' ', '_') +
            "_" +
            ((DateTime)myImage.shoot_date).ToString("dd_MMM_yyyy") +
            "_" +
            myImage.id.ToString() + ".jpg";
            myImage.preview_location = "pv_" + myImage.name;
            myImage.thumb_location = "th_" + myImage.name;
            myImage.thumb_location = "th_" + myImage.name;
            myImage.preview_location = "pv_" + myImage.name;

            if (hpf.ContentLength > 0)
            {
                CreateThumbAndPreview("~/photos", oldName, myImage.name);
                DeleteCurrentFile("~/photos", oldName);
            }
            else
            {
            RenameFile(myImage.thumb_location, "th_" + myImage.name, "~/photos/thumb");
            RenameFile(myImage.preview_location, "pv_" + myImage.name, "~/photos/preview");

            }

            imagedb.Save();
            return Redirect(returnUrl);

        }

        public void RenameFile(string fromName, string ToName, string path)
        {
            if (fromName != ToName)
            {
                FileInfo file = new FileInfo(Path.Combine(HttpContext.Server.MapPath(path), Path.GetFileName(fromName)));
                if (file.Exists)
                {
                    file.CopyTo(Path.Combine(HttpContext.Server.MapPath(path), Path.GetFileName(ToName)));
                    file.Delete();
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            ViewData["Magazines"] = new SelectList(magdb.GetMagazines(), "id", "name");
            ViewData["Users"] = new SelectList(userdb.GetUsers(), "id", "username", userdb.getUserbyUsername(HttpContext.User.Identity.Name.ToString()).id);


            return View();
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create([Bind(Exclude = "id")] tbl_Image myImage)
        //{
        //    ViewData["Magazines"] = new SelectList(magdb.GetMagazines(), "id", "name");
        //    ViewData["Users"] = new SelectList(userdb.GetUsers(), "id", "username", userdb.getUserbyUsername(HttpContext.User.Identity.Name.ToString()).id);

        //    myImage.date_uploaded = DateTime.Now;

        //    if (myImage.magazine_issue.Trim().Length == 0)
        //        ModelState.AddModelError("magazine_issue", "Magazine issue missing");
        //    if (myImage.shoot_description.Trim().Length == 0)
        //        ModelState.AddModelError("shoot_description", "Shoot description missing");
        //    try { DateTime testDate = (DateTime)myImage.shoot_date.Value; }
        //    catch { ModelState.AddModelError("shoot_date", "Invalid Shoot Date"); }
        //    if (myImage.description.Trim().Length == 0)
        //        ModelState.AddModelError("description", "Description issue missing");
        //    if (myImage.keywords.Trim().Length == 0)
        //        ModelState.AddModelError("keywords", "Keywords issue missing");

        //    FileInfo myFile = new FileInfo(Path.Combine(HttpContext.Server.MapPath("~/photos"), Path.GetFileName(myImage.name)));

        //    if (!myFile.Exists)
        //    {
        //        ModelState.AddModelError("", "Please select file");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    try
        //    {
        //        imagedb.Add(myImage);
        //        imagedb.Save();
        //        if (myFile.Length > 0)
        //        {
        //            saveImage(ref myImage);
        //            imagedb.Save();
        //        }

        //        return View("Index");

        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Error: " + ex.Message);
        //        return View();
        //    }
        //}


        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Uploadify()
        //{
        //    HttpPostedFileBase hpf = HttpContext.Request.Files[0] as HttpPostedFileBase;
        //    string tempFileName = "";
        //    try
        //    {
        //        if (hpf.ContentLength > 0)
        //        {
        //            tempFileName = System.Guid.NewGuid().ToString("N") + ".jpg";
        //            string filePath = Path.Combine(HttpContext.Server.MapPath("~/Photos"), Path.GetFileName(tempFileName));
        //            hpf.SaveAs(filePath);
        //        }
        //        return Json(tempFileName);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Json("Error Adding Image:" + ex.Message);
        //    }
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteCurrentFile(string folder, string currentFilename)
        {
            bool status = true;
            try
            {
                FileInfo myFile = new FileInfo(Path.Combine(HttpContext.Server.MapPath(folder), Path.GetFileName(currentFilename)));
                if (myFile.Length > 0)
                {
                    myFile.Delete();
                }
            }

            catch (Exception ex)
            {
                status = false;
            }
            return Json(status);
        }

        private void saveImage(ref tbl_Image myImage)
        {
            string oldName = myImage.name;
            myImage.name = myImage.tbl_publication.name.Replace(' ', '_') +
            "_" +
            myImage.magazine_issue.Replace(' ', '_') +
            "_" +
            ((DateTime)myImage.shoot_date).ToString("dd_MMM_yyyy") +
            "_" +
            myImage.id.ToString() + ".jpg";
            myImage.preview_location = "pv_" + myImage.name;
            myImage.thumb_location = "th_" + myImage.name;
            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path.Combine(HttpContext.Server.MapPath("~/photos"), Path.GetFileName(oldName))))
                {

                    var fileWidth = image.Width;
                    var fileHeight = image.Height;

                    //Create Thumbnail
                    int ThumbHeight = 110;
                    int ThumbWidth = 110;

                    if (fileWidth > fileHeight)
                    {
                        ThumbHeight = imagedb.GetProportionalImageHeight(ThumbWidth, fileHeight, fileWidth);
                    }
                    else if (fileWidth < fileHeight)
                    {
                        ThumbWidth = imagedb.GetProportionalImageWidth(ThumbHeight, fileWidth, fileHeight);
                    }

                    using (Bitmap bitmapThumb = new Bitmap(image, ThumbWidth, ThumbHeight))
                    {

                        bitmapThumb.Save(Server.MapPath("~/photos/thumb/" + myImage.thumb_location), image.RawFormat);
                    }

                    //Create Preview
                    int destinationHeight = 380;
                    int destinationWidth = 380;

                    if (fileWidth > fileHeight)
                    {
                        destinationHeight = imagedb.GetProportionalImageHeight(destinationWidth, fileWidth, fileWidth);
                    }
                    else if (fileWidth < fileHeight)
                    {
                        destinationWidth = imagedb.GetProportionalImageWidth(destinationHeight, fileWidth, fileHeight);
                    }



                    using (Bitmap bitmapPreview = new Bitmap(image, destinationWidth, destinationHeight))
                    {
                        Graphics g = Graphics.FromImage(bitmapPreview);
                        Color WaterMark = Color.FromArgb(80, Color.Gray);
                        Pen p = new Pen(WaterMark, 5);
                        g.DrawString("ImageBank Preview", new Font("Arial", 15), new SolidBrush(Color.DarkGray), new Point((bitmapPreview.Size.Width / 2) - 85, (bitmapPreview.Size.Height / 2) - 10));
                        g.DrawLine(p, new Point(0, 0), new Point((bitmapPreview.Size.Width / 2) - 50, (bitmapPreview.Size.Height / 2) - 50));
                        g.DrawLine(p, new Point((bitmapPreview.Size.Width / 2) + 50, (bitmapPreview.Size.Height / 2) + 50), new Point(bitmapPreview.Size.Width, bitmapPreview.Size.Height));
                        g.DrawLine(p, new Point((bitmapPreview.Size.Width / 2) - 50, (bitmapPreview.Size.Height / 2) + 50), new Point(0, bitmapPreview.Size.Height));
                        g.DrawLine(p, new Point((bitmapPreview.Size.Width / 2) + 50, (bitmapPreview.Size.Height / 2) - 50), new Point(bitmapPreview.Size.Width, 0));
                        bitmapPreview.Save(Server.MapPath("~/photos/preview/" + myImage.preview_location), image.RawFormat);
                    }

                }
                DeleteCurrentFile("~/photos", oldName);

            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            try
            {
                tbl_Image dbItem = imagedb.GetImageByID(id);
                string name = dbItem.name;
                imagedb.Delete(dbItem);
                imagedb.Save();
                DeleteCurrentFile("~/photos/thumb", "th_" + name);
                DeleteCurrentFile("~/photos/preview", "pv_" + name);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", new { id = id, Error = "Error deleting Image!" });
            }
        }

        public void SLUpload()
        {
            //Get all Values from Request
            string filename = HttpContext.Request.QueryString["filename"].ToString();
            int Mag_ID = int.Parse(HttpContext.Request.QueryString["Mag_id"].ToString());
            string Mag_Issue = HttpContext.Request.QueryString["Mag_Issue"].ToString();
            string Shoot = HttpContext.Request.QueryString["Shoot"].ToString();
            DateTime ShootDate = DateTime.Parse(HttpContext.Request.QueryString["ShootDate"].ToString());
            string keywords = HttpContext.Request.QueryString["keywords"].ToString();
            string description = HttpContext.Request.QueryString["description"].ToString();
            string photographer = HttpContext.Request.QueryString["photographer"].ToString();

            //Save image as tempImage in "photos" folder
            string tempFileName = System.Guid.NewGuid().ToString("N") + ".jpg";
            using (FileStream fs = System.IO.File.Create(HttpContext.Server.MapPath("~/photos/" + tempFileName)))
            {
                SaveFile(HttpContext.Request.InputStream, fs);
            }

            //Save image details to database
            tbl_Image newImage = new tbl_Image();
            newImage.date_uploaded = DateTime.Now;
            newImage.magazine_id = Mag_ID;
            newImage.magazine_issue = Mag_Issue;
            newImage.shoot_description = Shoot;
            newImage.shoot_date = ShootDate;
            newImage.keywords = keywords;
            newImage.description = description;
            newImage.photographer = userdb.getUserbyUsername(photographer).id;
            newImage.date_updated = DateTime.Now;
            newImage.name = tempFileName;
            imagedb.Add(newImage);
            imagedb.Save();


            //Create Thumbnail and Preview and Delete Temp
            string oldName = newImage.name;
            newImage.name = newImage.tbl_publication.name.Replace(' ', '_') +
            "_" +
            newImage.magazine_issue.Replace(' ', '_') +
            "_" +
            ((DateTime)newImage.shoot_date).ToString("dd_MMM_yyyy") +
            "_" +
            newImage.id.ToString() + ".jpg";
            newImage.preview_location = "pv_" + newImage.name;
            newImage.thumb_location = "th_" + newImage.name;

            CreateThumbAndPreview("~/photos", oldName, newImage.name);

            DeleteCurrentFile("~/photos", oldName);
            imagedb.Save();



        }

        private void CreateThumbAndPreview(string ImagePath, string SourceFileName, string DestinationFileName)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path.Combine(HttpContext.Server.MapPath(ImagePath), Path.GetFileName(SourceFileName))))
            {

                var fileWidth = image.Width;
                var fileHeight = image.Height;

                //Create Thumbnail
                int ThumbHeight = 110;
                int ThumbWidth = 110;

                if (fileWidth > fileHeight)
                {
                    ThumbHeight = imagedb.GetProportionalImageHeight(ThumbWidth, fileHeight, fileWidth);
                }
                else if (fileWidth < fileHeight)
                {
                    ThumbWidth = imagedb.GetProportionalImageWidth(ThumbHeight, fileWidth, fileHeight);
                }

                using (Bitmap bitmapThumb = new Bitmap(image, ThumbWidth, ThumbHeight))
                {

                    bitmapThumb.Save(Server.MapPath("~/photos/thumb/" + "th_" + DestinationFileName), image.RawFormat);
                }




                //Create Preview
                int destinationHeight = 380;
                int destinationWidth = 380;

                if (fileWidth > fileHeight)
                {
                    destinationHeight = imagedb.GetProportionalImageHeight(destinationWidth, fileHeight, fileWidth);
                }
                else if (fileWidth < fileHeight)
                {
                    destinationWidth = imagedb.GetProportionalImageWidth(destinationHeight, fileWidth, fileHeight);
                }

                using (Bitmap bitmapPreview = new Bitmap(image, destinationWidth, destinationHeight))
                {
                    Graphics g = Graphics.FromImage(bitmapPreview);
                    Color WaterMark = Color.FromArgb(80, Color.Gray);
                    Pen p = new Pen(WaterMark, 3);
                    g.DrawString("ImageBank Preview", new Font("Arial", 15), new SolidBrush(Color.DarkGray), new Point((bitmapPreview.Size.Width / 2) - 85, (bitmapPreview.Size.Height / 2) - 10));
                    g.DrawLine(p, new Point(0, 0), new Point((bitmapPreview.Size.Width / 2) - 50, (bitmapPreview.Size.Height / 2) - 50));
                    g.DrawLine(p, new Point((bitmapPreview.Size.Width / 2) + 50, (bitmapPreview.Size.Height / 2) + 50), new Point(bitmapPreview.Size.Width, bitmapPreview.Size.Height));
                    g.DrawLine(p, new Point((bitmapPreview.Size.Width / 2) - 50, (bitmapPreview.Size.Height / 2) + 50), new Point(0, bitmapPreview.Size.Height));
                    g.DrawLine(p, new Point((bitmapPreview.Size.Width / 2) + 50, (bitmapPreview.Size.Height / 2) - 50), new Point(bitmapPreview.Size.Width, 0));
                    bitmapPreview.Save(Server.MapPath("~/photos/preview/" + "pv_" + DestinationFileName), image.RawFormat);
                }

            }
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[stream.Length];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }

        #region Create2

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create2()
        {
            ViewData["Magazines"] = new SelectList(magdb.GetMagazines(), "id", "name");
            ViewData["Users"] = new SelectList(userdb.GetUsers(), "id", "username", userdb.getUserbyUsername(HttpContext.User.Identity.Name.ToString()).id);


            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create2([Bind(Exclude = "id")] tbl_Image myImage)
          {
            
            ViewData["Magazines"] = new SelectList(magdb.GetMagazines(), "id", "name");
            ViewData["Users"] = new SelectList(userdb.GetUsers(), "id", "username", userdb.getUserbyUsername(HttpContext.User.Identity.Name.ToString()).id);

            HttpPostedFileBase hpf = Request.Files["FileUp"] as HttpPostedFileBase;
            string tempFileName = System.Guid.NewGuid().ToString("N") + ".jpg";
            string filePath = "";

            #region Validation
            if (hpf.ContentLength > 0)
            {
                filePath = Path.Combine(HttpContext.Server.MapPath("~/Photos"), Path.GetFileName(tempFileName));
                hpf.SaveAs(filePath);
            }
           
            #endregion

            
            myImage.date_uploaded = DateTime.Now;
            myImage.date_updated = DateTime.Now;
            imagedb.Add(myImage);
            imagedb.Save();

            string oldName = tempFileName;
            myImage.name = myImage.tbl_publication.name.Replace(' ', '_') +
            "_" +
            myImage.magazine_issue.Replace(' ', '_') +
            "_" +
            ((DateTime)myImage.shoot_date).ToString("dd_MMM_yyyy") +
            "_" +
            myImage.id.ToString() + ".jpg";
            myImage.preview_location = "pv_" + myImage.name;
            myImage.thumb_location = "th_" + myImage.name;

            CreateThumbAndPreview("~/photos", oldName, myImage.name);
            
            DeleteCurrentFile("~/photos", oldName);
            imagedb.Save();

            if (Request.Form["Create"] != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create2");
            }
        }

        #endregion

    }
}
