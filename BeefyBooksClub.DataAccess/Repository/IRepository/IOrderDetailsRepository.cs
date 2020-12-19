using BeefyBooksClub.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeefyBooksClub.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails obj);
    }
}
