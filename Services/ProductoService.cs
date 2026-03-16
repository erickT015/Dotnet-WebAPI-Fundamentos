using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimerCrudWebAPI.Data;
using PrimerCrudWebAPI.DTOs.Common;
using PrimerCrudWebAPI.DTOs.Productos;
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
                .AsNoTracking()
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    CategoriaNombre = p.Categoria!.Nombre
                })
                .ToListAsync();
        }

        //metodo para obtener un producto por id, incluyendo el nombre de la categoria
        public async Task<ProductoResponseDto?> ObtenerProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .AsNoTracking() 
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
            bool existe = await _context.Productos
                    .AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.Trim().ToLower());

            if (existe)
                throw new InvalidOperationException($"Ya existe un producto con el nombre '{dto.Nombre}'");

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == dto.CategoriaId);

            if (categoria == null)
                throw new KeyNotFoundException($"La categoría especificada no existe '{dto.CategoriaId}'");

            var producto = new Producto
            {
                Nombre = dto.Nombre.Trim(),
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
                CategoriaNombre = categoria.Nombre
            };
        }

        //metodo para editar un producto, DEBE VALIDAR QUE LA CATEGORIA EXISTA
        public async Task<bool> EditarProducto(int id, ProductoCreateDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            if (producto.Nombre != dto.Nombre)
            {
                bool existe = await _context.Productos.AnyAsync(p => p.Nombre == dto.Nombre);
                if (existe) throw new InvalidOperationException("El nuevo nombre ya está en uso.");
            }

            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == dto.CategoriaId);

            if (!categoriaExiste)
                throw new KeyNotFoundException("La categoría no existe");

            producto.Nombre = dto.Nombre;
            producto.Precio = dto.Precio;
            producto.Descripcion = dto.Descripcion;
            producto.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();

            return true;
        }

        //metodo para eliminar un producto por id
        public async Task<bool> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return true;
        }


        //metodo para actualizar parcialmente un producto, validando que la categoria exista si se actualiza y el nombre no se repita con otro producto
        public async Task<bool> ActualizarProducto(int id, ProductoUpdateDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            if (producto.Nombre != dto.Nombre)
            {
                bool nombreExiste = await _context.Productos.AnyAsync(p => p.Nombre == dto.Nombre);
                if (nombreExiste) throw new InvalidOperationException("El nuevo nombre ya está en uso.");
            }

            if (dto.CategoriaId.HasValue)
            {
                bool categoriaExiste = await _context.Categorias .AnyAsync(c => c.Id == dto.CategoriaId.Value);
                if (!categoriaExiste) throw new KeyNotFoundException("La categoría no existe");
            }

            if (dto.Precio.HasValue)
                producto.Precio = dto.Precio.Value;

            _mapper.Map(dto, producto);

            await _context.SaveChangesAsync();

            return true;

        }


        //metodo para buscar productos por nombre o descripcion, la busqueda es case-insensitive y permite coincidencias parciales
        public async Task<IEnumerable<ProductoResponseDto>> BuscarProductos(string query)
        {
            query = query.Trim().ToLower();

            return await _context.Productos
                .AsNoTracking()
                .Where(p =>
                    p.Nombre.ToLower().Contains(query) ||
                    p.Descripcion.ToLower().Contains(query))
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    CategoriaNombre = p.Categoria!.Nombre
                })
                .ToListAsync();
        }


        //metodo para filtrar productos por categoria y rango de precio
        public async Task<IEnumerable<ProductoResponseDto>> FiltrarProductos(int? categoriaId, decimal? precioMin, decimal? precioMax)
        {
            var query = _context.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .AsQueryable();

            if (categoriaId.HasValue)
                query = query.Where(p => p.CategoriaId == categoriaId.Value);

            if (precioMin.HasValue)
                query = query.Where(p => p.Precio >= precioMin.Value);

            if (precioMax.HasValue)
                query = query.Where(p => p.Precio <= precioMax.Value);

            return await query
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    CategoriaNombre = p.Categoria!.Nombre
                })
                .ToListAsync();
        }


        //metodo para obtener productos paginados, ordenados por id, incluyendo el nombre de la categoria
        public async Task<IEnumerable<ProductoResponseDto>> ObtenerProductosPaginados(int page, int pageSize)
        {
            return await _context.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    CategoriaNombre = p.Categoria!.Nombre
                })
                .ToListAsync();
        }


        //metodo para obtener productos paginados, con busqueda, filtro por categoria y rango de precio, ordenados por precio o nombre
        public async Task<PagedResponse<ProductoResponseDto>> QueryProductos(ProductoQueryDto queryDto)
        {
            var query = _context.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(queryDto.Search))  // SEARCH
            {
                var search = queryDto.Search.Trim().ToLower();

                query = query.Where(p =>
                    p.Nombre.ToLower().Contains(search) || 
                    p.Descripcion.ToLower().Contains(search));
            }

            // FILTRO CATEGORIA
            if (queryDto.CategoriaId.HasValue)  query = query.Where(p => p.CategoriaId == queryDto.CategoriaId.Value);

            // PRECIO MIN
            if (queryDto.PrecioMin.HasValue)   query = query.Where(p => p.Precio >= queryDto.PrecioMin.Value);

            // PRECIO MAX
            if (queryDto.PrecioMax.HasValue)   query = query.Where(p => p.Precio <= queryDto.PrecioMax.Value);

         
            if (!string.IsNullOrWhiteSpace(queryDto.SortBy))    // Ordenamiento
            {
                switch (queryDto.SortBy.ToLower())
                {
                    case "precio":
                        query = queryDto.SortDirection == "desc"
                            ? query.OrderByDescending(p => p.Precio)
                            : query.OrderBy(p => p.Precio);
                        break;

                    case "nombre":
                        query = queryDto.SortDirection == "desc"
                            ? query.OrderByDescending(p => p.Nombre)
                            : query.OrderBy(p => p.Nombre);
                        break;

                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

         
            var totalRecords = await query.CountAsync();    // Total coincidencias para la consulta


            var productos = await query  // paginación
                .Skip((queryDto.Page - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    CategoriaNombre = p.Categoria!.Nombre
                })
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)queryDto.PageSize);

            return new PagedResponse<ProductoResponseDto>
            {
                Data = productos,
                Page = queryDto.Page,
                PageSize = queryDto.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }
    }
}
