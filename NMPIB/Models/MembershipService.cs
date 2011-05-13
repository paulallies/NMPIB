using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NMPIB.Controllers;
using NMPIB.Models.Repositories;

namespace NMPIB.Models
{
    public class NMPMembershipService : IMembershipService
    {

        IMembershipRepository db = new MembershipRepository();
        public int MinPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public bool ValidateUser(string userName, string password)
        {
            return db.ValidateUser(userName, password);
        }

        public System.Web.Security.MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

    }
}
