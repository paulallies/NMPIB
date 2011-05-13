using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using NMPIB.Models.Repositories;
using NMPIB.Models;

namespace NMPIB
{

    public class Uploader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string filename = context.Request.QueryString["filename"].ToString();
            int Mag_ID = int.Parse(context.Request.QueryString["Mag_id"].ToString());
            string Mag_Issue = context.Request.QueryString["Mag_Issue"].ToString();
            string Shoot = context.Request.QueryString["Shoot"].ToString();
            DateTime ShootDate = DateTime.Parse(context.Request.QueryString["ShootDate"].ToString());
            string keywords = context.Request.QueryString["keywords"].ToString();
            string description = context.Request.QueryString["description"].ToString();
            string photographer = context.Request.QueryString["photographer"].ToString();

            using(FileStream fs = File.Create(HttpContext.Current.Server.MapPath("~/photos/" + filename)))
            {
                SaveFile(context.Request.InputStream, fs);
            }

            IImageRepository db = new ImageRepository();
            IUserRepository userdb = new UserRepository();
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


            db.Add(newImage);
            db.Save();
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
