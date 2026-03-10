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
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
