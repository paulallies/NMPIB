using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMPIB.Models.Repositories
{
    public class MagazineRepository : IMagazineRepository
    {

        nmpibDataContext db = new nmpibDataContext();
        public IQueryable<tbl_publication> GetMagazines()
        {
            return db.tbl_publications.Where(m => m.active == true).OrderBy(m => m.name);
        }

    }
}
