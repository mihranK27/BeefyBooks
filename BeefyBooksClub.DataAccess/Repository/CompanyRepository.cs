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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.Update(company);        
        }
    }
}
