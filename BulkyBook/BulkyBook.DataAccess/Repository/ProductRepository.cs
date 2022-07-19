using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
             var productFromDb = _db.products.FirstOrDefault(u=>u.Id == obj.Id);
            if(productFromDb != null)
            {
                productFromDb.Title = obj.Title;
                productFromDb.Description = obj.Description;
                productFromDb.ISBN = obj.ISBN;
                productFromDb.Author = obj.Author;
                productFromDb.ListPrice = obj.ListPrice;
                productFromDb.Price = obj.Price;
                productFromDb.Price50 = obj.Price50;
                productFromDb.Price100 = obj.Price100;
                productFromDb.CategoryId = obj.CategoryId;
                productFromDb.CoverId = obj.CoverId;
                if(productFromDb.ImageUrl != null)
                {
                    productFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
