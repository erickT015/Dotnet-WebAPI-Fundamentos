using PrimerCrudWebAPI.DTOs.Productos;
using PrimerCrudWebAPI.DTOs.Common;
using PrimerCrudWebAPI.Models;

namespace PrimerCrudWebAPI.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoResponseDto>> ObtenerProductos(); //servicio para obtener todos los productos

        Task<ProductoResponseDto?> ObtenerProducto(int id); //servicio para obtener un producto por id, devuelve null si no se encuentra

        Task<ProductoResponseDto> CrearProducto(ProductoCreateDto dto); //servicio para crear un nuevo producto, devuelve el producto creado con su id asignado

        Task<bool> EditarProducto(int id, ProductoCreateDto dto); //servicio para editar un producto existente, devuelve true si se editó correctamente o false si no se encontró el producto

        Task<bool> ActualizarProducto(int id, ProductoUpdateDto dto);

        Task<bool> EliminarProducto(int id); //servicio para eliminar un producto por id, devuelve true si se eliminó correctamente o false si no se encontró el producto

        Task<IEnumerable<ProductoResponseDto>> BuscarProductos (string query); //servcio para buscar productos por nombre o descripción, devuelve una lista de productos que coincidan con la consulta

        Task<IEnumerable<ProductoResponseDto>> FiltrarProductos(int? categoriaId, decimal? precioMin, decimal? precioMax); //servicio para filtrar productos por categoría y rango de precio

        Task<IEnumerable<ProductoResponseDto>> ObtenerProductosPaginados(int page, int pageSize); //servicio para obtener productos paginados, recibe el número de página y el tamaño de página

        Task<PagedResponse<ProductoResponseDto>> QueryProductos(ProductoQueryDto queryDto); //servicio para realizar consultas complejas con múltiples criterios, recibe un objeto con los parámetros de consulta y
                                                                                            //devuelve una respuesta paginada con los productos que coincidan
    }
}
