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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails obj)
        {
            _db.Update(obj);
        }
    }
}
