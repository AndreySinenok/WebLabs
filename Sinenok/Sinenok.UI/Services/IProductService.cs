using Sinenok.Domain.Entities.Models;
using Sinenok.Domain.Entities;

namespace Sinenok.UI.Services
{
    public interface IProductService
    {
        public Task<ResponseData<ListModel<Gadget>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
    }
}
