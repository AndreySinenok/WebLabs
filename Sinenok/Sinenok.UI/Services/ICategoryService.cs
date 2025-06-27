using Sinenok.Domain.Entities.Models;
using Sinenok.Domain.Entities;

namespace Sinenok.UI.Services
    
{
    public interface ICategoryService
    {
        /// <summary> 
        /// Получение списка всех категорий 
        /// </summary> 
        /// <returns></returns> 
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
