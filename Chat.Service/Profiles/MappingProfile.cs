using AutoMapper;
using Chat.Data.Models;
using Chat.Service.Models.Product;

namespace Chat.Service.Profiles
{
    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            CreateMap<Product, ProductDto>().ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}
