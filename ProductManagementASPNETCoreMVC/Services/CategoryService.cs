using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repository;
        public CategoryService()
        {
            _repository = new CategoryRepository();
        }
        public List<Category> GetCategories()
        {
            return _repository.GetCategories();
        }
    }
}
