using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMPIB.Models.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        nmpibDataContext db = new nmpibDataContext();
        public bool ValidateUser(string UserName, string Password)
        {
            var user = from u in db.tbl_users
                       where
                       (u.UserName.ToUpper().Equals(UserName.ToUpper())
                       &&
                       u.Password.Equals(Password))
                       select u;
            return user.Count() > 0;
        }

    }
}
