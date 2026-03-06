using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimerCrudWebAPI.Data;
using PrimerCrudWebAPI.Migrations;
using PrimerCrudWebAPI.Models;

namespace PrimerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {

        private readonly DBContext _context;

        public ProductosController(DBContext context)
        {
            _context = context;
        }

        [HttpPost] // CREAR PRODUCTO
        public async Task<IActionResult> CrearProducto(Producto producto)
        {
            await _context.Productos.AddAsync(producto); //aggregar objetos a la bse de datos
            await _context.SaveChangesAsync(); //guardar los cambios en la base de datos

           // return Ok();
            return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.Id }, producto);
        }

        [HttpGet] //OBTENER TODOS LOS PRODUCTOS
        public async Task<ActionResult<IEnumerable<Producto>>> ObtenerProductos()
        {
            //tarigame todos los prod de db y los pone en lista en la vairble productos
            var productos = await _context.Productos.ToListAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")] //OBTENER UN PRODUCTO POR ID
        public async Task<IActionResult>ObtenerProducto(int id)
        {
            Producto producto = await _context.Productos.FindAsync(id);

            if(producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPut("{id}")] //EDITAR UN PRODUCTO POR ID
        public async Task<IActionResult>EditarProducto(int id, Producto producto)
        {
            var productoExistente = await _context.Productos.FindAsync(id);

            if (productoExistente == null)
                return NotFound();

            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;

            await _context.SaveChangesAsync();

            return Ok();

        }


        [HttpDelete("{id}")] //ELIMINAR UN PRODUCTO POR ID
        public async Task<IActionResult>EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
