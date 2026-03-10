using PrimerCrudWebAPI.DTOs.Productos;

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
    }
}
