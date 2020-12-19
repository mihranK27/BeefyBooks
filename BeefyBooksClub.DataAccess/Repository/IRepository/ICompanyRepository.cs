using BeefyBooksClub.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeefyBooksClub.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company company);
    }
}
