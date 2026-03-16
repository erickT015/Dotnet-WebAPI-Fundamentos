using PrimerCrudWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using PrimerCrudWebAPI.Services;
using PrimerCrudWebAPI.Services.Interfaces;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args); //ESTE ES EL PUNTO DE ENTRADA DE LA APLICACION, AQUI SE CONFIGURAN LOS SERVICIOS Y EL PIPELINE DE MIDDLEWARE

// Add services to the container.

builder.Services.AddControllers(); // REGISTRA LOS CONTROLLERS EN EL SISTEMA DE DEPENDENCIAS PARA QUE ASP.NET PUEDA USARLOS COMO ENDPOINTS HTTP
builder.Services.AddDbContext<DBContext>( o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}); //CONFIGURAMOS LA CONEXION A LA BASE DE DATOS, LE DECIMOS QUE USE SQL SERVER Y LE PASAMOS LA CADENA DE CONEXION DESDE EL ARCHIVO DE CONFIGURACION


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(Program)); //SE USA PARA CONFIGURAR AUTOMAPPER, LE DECIMOS QUE BUSQUE LOS PROFILES DE MAPEADO EN EL ENSAMBLAJE DONDE SE ENCUENTRA LA CLASE PROGRAM
builder.Services.AddScoped<IProductoService, ProductoService>(); //SIRVE QUE ProductoService SE INYECTE EN LOS CONTROLADORES QUE LO NECESITEN, CADA VEZ QUE SE SOLICITE UN IProductoService SE CREARA UNA NUEVA INSTANCIA DE ProductoService 
builder.Services.AddScoped<ICategoriaService, CategoriaService>(); //SIRVE QUE CategoriaService SE INYECTE EN LOS CONTROLADORES QUE LO NECESITEN, CADA VEZ QUE SE SOLICITE UN ICategoriaService SE CREARA UNA NUEVA INSTANCIA DE CategoriaService
builder.Services.AddEndpointsApiExplorer(); //PERMITE QUE SWAGGER DETECTE LOS ENDPOINTS DE LA APLICACION PARA GENERAR LA DOCUMENTACION DE LA API
builder.Services.AddSwaggerGen(); //CONFIGURAMOS SWAGGER PARA GENERAR LA DOCUMENTACION DE LA API, SE PUEDE CONFIGURAR CON MAS DETALLES COMO EL TITULO, LA VERSION, LOS AUTENTICADORES, ETC.

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()   
              .AllowAnyHeader()    
              .AllowAnyMethod();   
    });

    options.AddPolicy("ProdCorsPolicy", policy =>
    {
        policy.WithOrigins("https://mi-verdadero-dominio.com") 
              .AllowAnyHeader()    
              .AllowAnyMethod(); 
    });
});
// CONFIGURAMOS POLÍTICAS CORS.
// DevCorsPolicy → abierta para desarrollo (evita errores del navegador mientras programas).
// ProdCorsPolicy → restringida para producción, solo permite el dominio real del frontend.



var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); //PERMITE MIDDLEWARE GLOBAL DE MANEJO DE ERRORES

app.UseHttpsRedirection(); //REDIRIGE DE HTTP A HTTPS

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCorsPolicy"); // POLÍTCICA DEFINIDA ARRIBA PARA DESARROLLO
}
else
{
    app.UseCors("ProdCorsPolicy"); // POLÍTCICA DEFINIDA ARRIBA PARA PRODUCCIÓN
}

app.UseAuthorization(); //HABILITA LA AUTORIZACION, SE USA PARA PROTEGER ENDPOINTS CON POLITICAS DE AUTORIZACION

app.MapControllers(); //MAPEA LOS CONTROLLERS COMO ENDPOINT SCCESIBLES POR HTTP

app.Run();
