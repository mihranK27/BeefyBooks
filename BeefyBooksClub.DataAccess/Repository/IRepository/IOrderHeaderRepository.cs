using BeefyBooksClub.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeefyBooksClub.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
    }
}
