using AutoMapper;
using OnSale.Common.Entities;
using OnSale.Web.Models;

namespace OnSale.Web.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, CategoryViewModel>();
            
        }
    }
}
