using BusinessObjects.Models;
using DataAccessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using var context = new MyStoreDbContext();
                listProducts = context.Products.Include(p => p.Category).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProducts;
        }

        public static void SaveProduct(Product product)
        {
            try
            {
                using var context = new MyStoreDbContext();
                context.Products.Add(product);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateProduct(Product product)
        {
            try
            {
                using var context = new MyStoreDbContext();
                context.Entry<Product>(product).State
                    = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteProduct(Product product)
        {
            try
            {
                using var context = new MyStoreDbContext();
                var p1 = context.Products.SingleOrDefault(p => p.ProductId == product.ProductId);
                context.Products.Remove(p1);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static Product GetProductById (int id)
        {
            using var context = new MyStoreDbContext();
            return context.Products.FirstOrDefault(p => p.ProductId.Equals(id));
        }
    }
}
