using BeefyBooksClub.DataAccess.Data;
using BeefyBooksClub.DataAccess.Repository;
using BeefyBooksClub.DataAccess.Repository.IRepository;
using BeefyBooksClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeefyBooksClub.DataAccess
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
