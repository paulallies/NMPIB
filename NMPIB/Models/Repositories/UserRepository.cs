using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMPIB.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        nmpibDataContext db = new nmpibDataContext();
        public string GetCurrentUser()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public tbl_user getUserbyUsername(string username)
        {
            return db.tbl_users.SingleOrDefault(u => u.UserName == username);
        }

        public IQueryable<tbl_user> GetUsers()
        {
            return db.tbl_users.OrderBy(u => u.UserName);
        }

    }
}
