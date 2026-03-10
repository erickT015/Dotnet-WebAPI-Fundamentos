using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimerCrudWebAPI.Data;
using PrimerCrudWebAPI.DTOs.Productos;
using PrimerCrudWebAPI.Migrations;
using PrimerCrudWebAPI.Models;
using PrimerCrudWebAPI.Services.Interfaces;

namespace PrimerCrudWebAPI.Services
{
    public class ProductoService : IProductoService
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public ProductoService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //metodo para obtener todos los productos, incluyendo el nombre de la categoria
        public async Task<IEnumerable<ProductoResponseDto>> ObtenerProductos()
        {
            return await _context.Productos
    .Select(p => new ProductoResponseDto
    {
        Id = p.Id,
        Nombre = p.Nombre,
        Descripcion = p.Descripcion,
        Precio = p.Precio,
        CategoriaNombre = p.Categoria.Nombre
    })
    .ToListAsync();
        }

        //metodo para obtener un producto por id, incluyendo el nombre de la categoria
        public async Task<ProductoResponseDto?> ObtenerProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return null;

            return new ProductoResponseDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Descripcion = producto.Descripcion,
                CategoriaNombre = producto.Categoria.Nombre
            };
        }

        //metodo para crear un producto, validando que la categoria exista
        public async Task<ProductoResponseDto> CrearProducto(ProductoCreateDto dto)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == dto.CategoriaId);

            if (!categoriaExiste)
                throw new Exception("La categoría no existe");

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId
            };

            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            return new ProductoResponseDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                CategoriaNombre = producto.Categoria.Nombre
            };
        }

        //metodo para editar un producto, DEBE VALIDAR QUE LA CATEGORIA EXISTA
        public async Task<bool> EditarProducto(int id, ProductoCreateDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            producto.Nombre = dto.Nombre;
            producto.Precio = dto.Precio;
            producto.Descripcion = dto.Descripcion;
            producto.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActualizarProducto(int id, ProductoUpdateDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            _mapper.Map(dto, producto);

            await _context.SaveChangesAsync();

            return true;

        }
    }
}
