using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimerCrudWebAPI.Data;
using PrimerCrudWebAPI.DTOs.Productos;
using PrimerCrudWebAPI.Migrations;
using PrimerCrudWebAPI.Models;
using PrimerCrudWebAPI.Services.Interfaces;

namespace PrimerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {

        //private readonly DBContext _context;
        private readonly IProductoService _service; //EL CONTROLER AHORA DEPENDE DEL SERVICIO, NO DE LA BASE DE DATOS DIRECTAMENTE

        public ProductosController(IProductoService service) //INYECCION DE DEPENDENCIAS DEL SERVICIO EN EL CONTROLADOR
        {
            _service = service;
        }

        [HttpGet] //OBTENER TODOS LOS PRODUCTOS
        public async Task<IActionResult> ObtenerProductos()
        {
            var productos = await _service.ObtenerProductos(); //EL CONTROLADOR LLAMA AL SERVICIO PARA OBTENER LOS PRODUCTOS,
                                                               //EL SERVICIO SE ENCARGA DE LA LOGICA DE NEGOCIO Y DE LA INTERACCION CON LA BASE DE DATOS
            return Ok(productos);
        }


        [HttpGet("{id}")] //OBTENER UN PRODUCTO POR ID
        public async Task<IActionResult> ObtenerProducto(int id)
        {
            var producto = await _service.ObtenerProducto(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }


        [HttpPost] //CREAR UN NUEVO PRODUCTO
        public async Task<IActionResult> CrearProducto(ProductoCreateDto dto)
        {
            var producto = await _service.CrearProducto(dto);

            return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.Id }, producto); //DEVUELVE UN 201 CREATED CON LA RUTA PARA OBTENER EL PRODUCTO RECIEN CREADO
        }



        [HttpPut("{id}")] //EDITAR UN PRODUCTO POR ID
        public async Task<IActionResult> EditarProducto(int id, ProductoCreateDto dto)
        {
            var actualizado = await _service.EditarProducto(id, dto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }


        [HttpDelete("{id}")] //ELIMINAR UN PRODUCTO POR ID
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var eliminado = await _service.EliminarProducto(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }


        [HttpPatch("{id}")] //ACTUALIZAR UN PRODUCTO POR ID
        public async Task<IActionResult> ActualizarProducto(int id, ProductoUpdateDto dto)
        {
            var actualizado = await _service.ActualizarProducto(id, dto);
            if (!actualizado)
                return NotFound();
            return NoContent();
        }
    }
}
