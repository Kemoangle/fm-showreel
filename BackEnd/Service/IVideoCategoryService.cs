using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoCategoryService
    {
        IEnumerable<Category> GetPageCategory(string keySearch = "");
        IEnumerable<Category> GetCategoryByVideoId(int id);
        IEnumerable<Category> GetSubCategoryByVideoId(int id);
        Category AddCategory(Category category);
        Category UpdateCategory(Category category);
        void UpdateVideoCategory(Category[] categories, int videoId);
        IEnumerable<Category> GetCategoryByParent(int parentId = 0, int[] arrParentId = null);
        IEnumerable<Category> GetAllParentCategory();
        Category GetCategoryById(int id);
        void UpdateSubCategory(Category[] categories, int parentId);
        
    }
}
