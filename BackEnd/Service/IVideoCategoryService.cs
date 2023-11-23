using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoCategoryService
    {
        IEnumerable<Category> GetAllCategory();
        IEnumerable<Category> GetCategoryByVideoId(int id);
        void AddCategory(Category videocategory);
        void UpdateCategory(Category videocategory);
        void DeleteCategory(int id);
    }
}
