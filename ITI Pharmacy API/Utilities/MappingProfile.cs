using AutoMapper;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;

namespace ITI.Pharmacy.API
{
    public class MappingProfile1 : Profile
    {
        public MappingProfile1()
        {
            // Product mappings
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.ProductStocks));

            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ProductImage))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ProductImage))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProductStock, ProductStockDTO>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : ""));

       
            CreateMap<Order, OrderDTO>();
            CreateMap<User, UserDTO>();
        }
    }
}