using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NMPIB.Models.Repositories
{
    public interface IMembershipRepository
    {
        bool ValidateUser(string UserName, string Password);

    }

    public interface IImageRepository
    {
        void Add(tbl_Image image);
        IQueryable<tbl_Image> GetAllImages();
        IQueryable<tbl_Image> GetImagesByKW(string search);
        tbl_Image GetImageByID(int id);
        IQueryable<tbl_Image> GetImagesBySearch(string kw);
        void Save();
        void Delete(tbl_Image image);

        IQueryable<tbl_Image> GetAllImagesPaged(int pageIndex, int defaultPageSize, string sord);
        IQueryable<tbl_Image> GetImagesBySearchOrderPaged(int pageIndex, int defaultPageSize, string searchString, string sord);
        int GetProportionalImageHeight(int thWidth, int fHeight, int fWidth);
        int GetProportionalImageWidth(int thHeight, int fWidth, int fHeight);
    }

    public interface IUserRepository
    {
        string GetCurrentUser();
        tbl_user getUserbyUsername(string username);
        IQueryable<tbl_user> GetUsers();
        
    }

    public interface IMagazineRepository
    {
        IQueryable<tbl_publication> GetMagazines();
    }


}
