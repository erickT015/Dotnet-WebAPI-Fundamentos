# C# WebAPI – Fundamentos


Este repositorio documenta mi aprendizaje desde *ASP.NET MVC* hacia *ASP.NET Core Web API*, como parte de un proceso de actualización hacia arquitecturas más modernas.


El objetivo de este proyecto no es construir un sistema complejo, sino *entender los fundamentos del desarrollo de APIs REST con .NET* antes de integrarlas con un frontend moderno.


---


## Tecnologías

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger

---


## Conceptos practicados


- Creación de una *Web API REST*
- Implementación de *CRUD básico*
- Uso de *Entity Framework Core*
- *Relación uno a muchos* (Productos → Categorías)
- Uso de *DTOs* para separar los modelos de la API
- Separación de responsabilidades usando **Controllers y Services**
- Implementación de **búsqueda, filtros y ordenamiento** en endpoints
- Implementación de **paginación de resultados**
- Uso de **PATCH** para actualizaciones parciales
- Manejo global de errores con **middleware de excepciones**
- Validación de datos con *DataAnnotations*
- Uso de *Swagger* para documentar y probar endpoints


---

<details>
<summary>📁 Estructura del proyecto</summary>

Controllers/
Services/
Services/Interfaces/
Models/
DTOs/
Data/
Mappings/
Middlewares/
Migrations/

</details>

---

<details>
<summary>✅​ Ver endpoints principales</summary>

```
### Categorías

GET /api/Categorias  
Obtiene todas las categorías

GET /api/Categorias/{id}  
Obtiene una categoría por id

POST /api/Categorias  
Crea una nueva categoría

PUT /api/Categorias/{id}  
Actualiza una categoría

DELETE /api/Categorias/{id}  
Elimina una categoría


### Productos

GET /api/Productos  
Obtiene todos los productos

GET /api/Productos/{id}  
Obtiene un producto por id

POST /api/Productos  
Crea un producto

PUT /api/Productos/{id}  
Actualiza un producto completo

PATCH /api/Productos/{id}  
Actualiza parcialmente un producto


### Consultas

GET /api/Productos/search?query=texto  

GET /api/Productos/filter?categoriaId=1&precioMin=10&precioMax=100  

GET /api/Productos/paged?page=1&pageSize=5  

GET /api/Productos/query 
```

</details>

---

## Objetivo de aprendizaje


Este proyecto forma parte de una progresión personal:

MVC 5 → ASP.NET Core MVC → ASP.NET Core Web API → React + Web API

La idea es construir una base sólida y entender APIs backend modernas antes de integrarlas con un frontend moderno como es React.


---


## Estado del proyecto

Proyecto educativo enfocado en **practicar los fundamentos de Web APIs con .NET**.

El código fue desarrollado principalmente con fines de aprendizaje, implementando patrones comunes como **DTOs, servicios y separación de capas**.



