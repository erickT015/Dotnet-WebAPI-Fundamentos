using PrimerCrudWebAPI.DTOs.Productos;
using PrimerCrudWebAPI.Models;
using AutoMapper;

namespace PrimerCrudWebAPI.Mappings
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<ProductoUpdateDto, Producto>()
                .ForMember(dest => dest.Precio, opt => opt.Ignore())
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
