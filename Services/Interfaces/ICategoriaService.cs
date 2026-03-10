using PrimerCrudWebAPI.DTOs.Categorias;

namespace PrimerCrudWebAPI.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaResponseDto>> ObtenerCategorias(); 

        Task<CategoriaResponseDto?> ObtenerCategoria(int id); 

        Task<CategoriaResponseDto> CrearCategoria(CategoriaCreateDto dto); 

        Task<bool> EditarCategoria(int id, CategoriaCreateDto dto); 

        Task<bool> EliminarCategoria(int id); 
    }
}
