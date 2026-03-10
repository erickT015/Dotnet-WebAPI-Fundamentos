using Microsoft.EntityFrameworkCore;
using PrimerCrudWebAPI.Data;
using PrimerCrudWebAPI.DTOs.Categorias;
using PrimerCrudWebAPI.Models;
using PrimerCrudWebAPI.Services.Interfaces;


namespace PrimerCrudWebAPI.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly DBContext _context;
        public CategoriaService(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaResponseDto>> ObtenerCategorias()
        {
            return await _context.Categorias
                .Select(c => new CategoriaResponseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion
                })
                .ToListAsync();
        }

        public async Task<CategoriaResponseDto?> ObtenerCategoria(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
                return null;

            return new CategoriaResponseDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };
        }

        public async Task<CategoriaResponseDto> CrearCategoria(CategoriaCreateDto dto)
        {
            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Nombre == dto.Nombre);
            if (categoriaExiste)
                throw new Exception("Ya existe una categoría con ese nombre");

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return new CategoriaResponseDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };
        }

        public async Task<bool> EditarCategoria(int id, CategoriaCreateDto dto)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return false;

            categoria.Nombre = dto.Nombre;
            categoria.Descripcion = dto.Descripcion;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return false;

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
