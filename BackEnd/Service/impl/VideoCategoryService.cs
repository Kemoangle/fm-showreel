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

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void AddVideoCategory(Videocategory videocategory)
        {
            _context.Videocategories.Add(videocategory);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var categoryDelete = _context.Categories.Find(id);
            if (categoryDelete != null)
            {
                _context.Categories.Remove(categoryDelete);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Category> GetAllCategory()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Category> GetCategoryByVideoId(int id)
        {
            var query = from v in _context.Videocategories
                        join c in _context.Categories on v.CategoryId equals c.Id
                        where v.VideoId == id
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
            query = query.OrderByDescending(b => b.Id);
            return query.ToList();
        }

        public void UpdateCategory(Category category)
        {
            _context.Update(category);
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
    }
}
