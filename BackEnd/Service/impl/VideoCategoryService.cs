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

        public void AddCategory(Category videocategory)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetAllCategory()
        {
            return _context.Categories.ToList();
        }

        public IEnumerable<Category> GetCategoryByVideoId(int id)
        {
            var query = from v in _context.Videocategories
                        join c in _context.Categories on v.CategoryId equals c.Id
                        select new Category{
                            Id = c.Id,
                            Name = c.Name
                        };

            return query.ToList();
        }



        public void UpdateCategory(Category videocategory)
        {
            throw new NotImplementedException();
        }
    }
}
