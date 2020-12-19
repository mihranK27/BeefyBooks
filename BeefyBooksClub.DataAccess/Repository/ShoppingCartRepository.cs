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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart obj)
        {
            _db.Update(obj);
        }
    }
}
