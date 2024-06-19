using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EjercicioClase1Modulo3.Controllers
{
    [Route("v1/libros")]
    [ApiController]
    public class BookController : ControllerBase
    {
        //Books contiene una lista de libros. Esta información viene del archivo libros.json ubicado dentro de la carpeta Data.
        public List<Book> Books { get; set; }

        //filePath contiene la ubicación del archivo libros.json. No mover el archivo libros.json de esa carpeta.
        string filePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, @"Data\libros.json" );

        public BookController()
        {
            //Instanciación e inicialización de la lista de libros deserializando el archivo libros.json
            Books = JsonSerializer.Deserialize<List<Book>>( System.IO.File.ReadAllText( filePath ) );
        }

        #region Ejercicio 1
        /*
        Completar y modificar el método siguiente para crear un endpoint que liste todos los libros y tenga la siguiente estructura:
        [GET] v1/libros
        */

        [HttpGet]
        public ActionResult<List<Book>> GetBooks()
        {
            return Ok(Books.ToList());
        }

        #endregion

        #region Ejercicio 2
        /*
         Crear un endpoint para Obtener un libro por su número de id usando route parameters que tenga la siguiente estructura:
        [GET] v1/libros/{id}
        Ejemplo: v1/libros/8 (devuelve toda la información del libro cuyo id es 8. Es decir: El diario de Ana Frank)
        */
        [HttpGet]
        [Route("/{id}")]
        public ActionResult<Book> getBook([FromRoute] int id)
        {
            return Ok(Books.Where(book => book.id == id).First());
        }

        #endregion

        #region Ejercicio 3
        /*
         Crear un endpoint para listar todos libros de un género en particular usando route parameters que tenga la siguiente estructura:
        [GET] v1/libros/genero/{genero} 
        Ejemplo: v1/libros/genero/fantasía (devuelve una lista de todos los libros del género fantasía)
         */
        [HttpGet]
        [Route("/genero/{genero}")]
        public ActionResult<List<Book>> getBooksByGenero([FromRoute] String genero)
        {
            return Ok(Books.Where(book => book.genero.Equals(genero)).ToList());
        }


        #endregion

        #region Ejercicio 4
        /*
        Crear un endpoint para Listar todos los libros de un autor usando query parameters que tenga la siguiente estructura:
        [GET] v1/libros?autor={autor}
        Ejemplo: v1/libros?autor=Paulo Coelho (devuelve una lista de todos los libros del autor Paulo Coelho)
         */

        [HttpGet]
        [Route("/autor")]
        public ActionResult<List<Book>> getBooksByAutor([FromQuery] String autor)
        {
            return Ok(Books.Where(book => book.autor.Equals(autor)).ToList());
        }
        #endregion


        #region Ejercicio 5
        /*
        Crear un endpoint para Listar unicamente todos los géneros de libros disponibles que tenga la siguiene estructura:
        [GET] v1/libros/generos
        Idealmente, el listado de géneros que retorne el endpoint, no debe contener repetidos.
         */

        [HttpGet]
        [Route("/generos")]
        public ActionResult<List<Book>> getGroupGeneroWithOutDuplicate()
        {
            return Ok(Books.GroupBy(book => book.genero,(key, group) => group.FirstOrDefault()).ToList());
        }
        #endregion

        #region Ejercicio 6
        /*
        Crear un endpoint para listar todos los libros implementando paginación usando route parameters con la siguiente estructura:
        [GET] v1/libros?pagina={numero-pagina}&cantidad={cantidad-por-pagina}
        Ejemplos: 
        v1/libros?pagina=1&cantidad=10 (devuelve una lista de los primeros diez libros)
        v1/libros?pagina=2&cantidad=10 (devuelve una lista de diez libros, salteando los primeros 10)
        v1/libros?pagina=3&cantidad=10 (devuelve una lista de diez libros, salteando los primeros 20)
         */

        [HttpGet]
        [Route("/paginacion")]
        public ActionResult<List<Book>> getBooksWithPagination([FromQuery] int pagina, [FromQuery] int cantidad)
        {

            if (Books.Count() >= (pagina * cantidad))
                return Ok(Books.Skip((pagina -1) * cantidad).Take(cantidad).ToList());

            else
                return NoContent();
        }
        #endregion
    }
}
