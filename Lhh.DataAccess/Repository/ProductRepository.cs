using Lhh.DataAccess.Data;
using Lhh.DataAccess.Repository.IRepository;
using Lhh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lhh.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var objFromDB = _db.Products.FirstOrDefault(u => u.Id == product.Id);
            if (objFromDB != null)
            {
                objFromDB.Title = product.Title;
                objFromDB.ISBN = product.ISBN;
                objFromDB.Author = product.Author;
                objFromDB.ListPrice = product.ListPrice;
                objFromDB.Category = product.Category;
                objFromDB.Price = product.Price;
                objFromDB.Price50 = product.Price50;
                objFromDB.Price100 = product.Price100;
                objFromDB.Description = product.Description;
                if (product.Image != null)
                {
                    objFromDB.Image = product.Image;
                }
            }
        }
    }
}
