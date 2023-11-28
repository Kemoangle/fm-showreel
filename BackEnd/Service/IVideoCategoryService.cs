﻿using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoCategoryService
    {
        IEnumerable<Category> GetAllCategory();
        IEnumerable<Category> GetCategoryByVideoId(int id);
        void AddCategory(Category category);
        void AddVideoCategory(Videocategory videocategory);
        void UpdateCategory(Category category);
        void UpdateVideoCategory(Category[] categories, int videoId);
        void DeleteCategory(int id);
    }
}
