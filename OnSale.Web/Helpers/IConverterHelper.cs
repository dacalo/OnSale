using OnSale.Common.Entities;
using OnSale.Web.Models;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public interface IConverterHelper
    {
        Category ToCategory(CategoryViewModel model, string urlImage, bool isNew);

        CategoryViewModel ToCategoryViewModel(Category category);
        
        Task<Product> ToProductAsync(ProductViewModel model, bool isNew);

        ProductViewModel ToProductViewModel(Product product);

    }
}
