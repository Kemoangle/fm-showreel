using System.Linq;
using Showreel.Models;

namespace Showreel.Service.impl
{
    public class VideoCategoryService : IVideoCategoryService
    {
        private readonly ShowreelContext _context;
        public VideoCategoryService(ShowreelContext context)
        {
            _context = context;
        }

        public Category AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Category> GetCategoryByVideoId(int id)
        {
            var query = from v in _context.Videocategories
                        join c in _context.Categories on v.CategoryId equals c.Id
                        where v.VideoId == id && (c.Parent == null || c.Parent == 0)
                        select new Category
                        {
                            Id = c.Id,
                            Name = c.Name
                        };

            return query.ToList();
        }

        public IEnumerable<Category> GetPageCategory(string keySearch = "")
        {
            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(c => c.Name.Contains(keySearch));
            }
            query = query.Where(c => c.Parent == 0 || c.Parent == null).OrderByDescending(b => b.Id);
            return query.ToList();
        }

        public IEnumerable<Category> GetCategoryByParent(int parentId = 0, int[] arrParentId = null)
        {
            var query = _context.Categories.AsQueryable();
            if(arrParentId != null && arrParentId.Any()){
                query = query.Where(q => q.Parent > 0 && arrParentId.Contains((int)q.Parent));
            }
            if(parentId > 0){
                query = query.Where(c => c.Parent == parentId);
            }
            
            return query;
        }

        public IEnumerable<Category> GetSubCategoryByVideoId(int id)
        {
            var query = from v in _context.Videocategories
                        join c in _context.Categories on v.CategoryId equals c.Id
                        where v.VideoId == id && c.Parent > 0
                        select new Category
                        {
                            Id = c.Id,
                            Name = c.Name
                        };

            return query.ToList();
        }

        public Category UpdateCategory(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();
            return category;
        }

        public void UpdateSubCategory(Category[] categories, int parentId)
        {
            var existingSubCategories = _context.Categories
                                            .Where(v => v.Parent == parentId)
                                            .ToList();
            if (existingSubCategories.Any())
            {
                _context.Categories.RemoveRange(existingSubCategories);
            }

            foreach (var category in categories)
            {
                var newSubCategory = new Category
                {
                    Name = category.Name,
                    Parent = parentId,
                    Restrictions = new List<Restriction>(),
                    Rules = new List<Rule>(),
                    Videocategories = new List<Videocategory>()
                };

                _context.Categories.Add(newSubCategory);
            }

            _context.SaveChanges();
        }

        public void UpdateVideoCategory(Category[] categories, int videoId)
        {
            var existingCategories = _context.Videocategories
                                            .Where(v => v.VideoId == videoId)
                                            .ToList();
            if (existingCategories.Any())
            {
                _context.Videocategories.RemoveRange(existingCategories);
            }

            foreach (var category in categories)
            {
                var newVideoCategory = new Videocategory
                {
                    VideoId = videoId,
                    CategoryId = category.Id
                };

                _context.Videocategories.Add(newVideoCategory);
            }

            _context.SaveChanges();
        }

        public IEnumerable<Category> GetAllParentCategory()
        {
            return _context.Categories.Where(c => c.Parent == 0 || c.Parent == null);
        }
    }
}
