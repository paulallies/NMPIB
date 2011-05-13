using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Web.Mvc;

namespace NMPIB.Models.Repositories
{
    public class ImageRepository : IImageRepository
    {
        nmpibDataContext db = new nmpibDataContext();

        public void Add(tbl_Image image)
        {
            db.tbl_Images.InsertOnSubmit(image);
        }

        public IQueryable<tbl_Image> GetAllImages()
        {
            return db.tbl_Images;
        }

        public IQueryable<tbl_Image> GetImagesByKW(string search)
        {
            int wordLimit = 5;
            string[] keywords = new string[wordLimit];
            for (int i = 0; i < wordLimit; i++) keywords[i] = string.Empty;
            string[] inputKeywords = search.Split(new
                char[] { ' ', ',' },
                wordLimit + 1,
                StringSplitOptions.RemoveEmptyEntries);
            int max = inputKeywords.Length > wordLimit ? wordLimit : inputKeywords.Length;
            for (int i = 0; i < max; i++)
                keywords[i] = inputKeywords[i];



            return from i in this.GetAllImages()
                   where
                       (i.keywords.Contains(keywords[0]) && keywords[0] != "") ||
                       (i.keywords.Contains(keywords[1]) && keywords[1] != "") ||
                       (i.keywords.Contains(keywords[2]) && keywords[2] != "") ||
                       (i.keywords.Contains(keywords[3]) && keywords[3] != "") ||
                       (i.keywords.Contains(keywords[4]) && keywords[4] != "")
                   select i;
        }

        public tbl_Image GetImageByID(int id)
        {
            return db.tbl_Images.SingleOrDefault(item => item.id == id);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public void Delete(tbl_Image image)
        {
            db.tbl_Images.DeleteOnSubmit(image);
        }

        public IQueryable<tbl_Image> GetAllImagesPaged(int pageIndex, int defaultPageSize, string sord)
        {
            return ReturnSortQuery(this.GetAllImages(), sord).Skip((pageIndex - 1) * defaultPageSize).Take(defaultPageSize);
        }

        public IQueryable<tbl_Image> GetImagesBySearch(string searchString)
        {
            return db.tbl_Images.Where(
                i => i.name.Contains(searchString) ||
                i.keywords.Contains(searchString) ||
                i.tbl_user.UserName.Contains(searchString) ||
                i.shoot_description.Contains(searchString) ||
                i.magazine_issue.Contains(searchString) ||
                i.tbl_publication.name.Contains(searchString));

        }

        public int GetProportionalImageHeight(int thWidth, int fHeight, int fWidth)
        {
            return ((thWidth * fHeight) / fWidth);
        }

        public int GetProportionalImageWidth(int thHeight, int fWidth, int fHeight)
        {
            return ((thHeight * fWidth) / fHeight);
        }

        public IQueryable<tbl_Image> GetImagesBySearchOrderPaged(int pageIndex, int defaultPageSize, string searchString, string sord)
        {
            //Search Linq - Get all images in database and search
            var searchquery = ReturnSearchQuery(db.tbl_Images, searchString);

            //Sort Linq - Once we have sorted list order the list
            var searchsortquery = ReturnSortQuery(searchquery, sord);

            //Page Linq - Once we have the searched sorted list then page it
            var searchsortpagequery = searchsortquery.Skip((pageIndex - 1) * defaultPageSize).Take(defaultPageSize);

            return searchsortpagequery;
        }

        private IQueryable<tbl_Image> ReturnSearchQuery(IQueryable<tbl_Image> myQuery, string searchString)
        {
            return myQuery.Where(
                i => i.name.Contains(searchString) ||
                i.keywords.Contains(searchString) ||
                i.tbl_user.UserName.Contains(searchString) ||
                i.shoot_description.Contains(searchString) ||
                i.magazine_issue.Contains(searchString) ||
                i.description.Contains(searchString) ||
                i.tbl_publication.name.Contains(searchString));
        }

        private IQueryable<tbl_Image> ReturnSortQuery(IQueryable<tbl_Image> myQuery, string sortString)
        {
            var q = myQuery.OrderByDescending(i => i.shoot_date);//default
            switch (sortString)
            {
                case "magazine":
                    q = myQuery.OrderBy(i => i.tbl_publication.name);
                    break;
                case "datedesc"://default
                    q = myQuery.OrderByDescending(i => i.shoot_date);
                    break;
                case "dateasc":
                    q = myQuery.OrderBy(i => i.shoot_date);
                    break;
            }

            return q;
        }

    }
}
