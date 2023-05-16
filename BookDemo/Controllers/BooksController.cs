using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookDemo.Data;
using BookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            //Kitapları tutabileceğim bir değişken.
            var books = ApplicationContext.Books.ToList();
            //Burada ApplicationContexten bir liste döner.

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var books = ApplicationContext
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();

            if (books is null)
            {
                return NotFound(); 
            }            
            return Ok(books);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest(); //400
                }
                ApplicationContext.Books.Add(book);
                return StatusCode(201, book); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")]int id,
            [FromBody] Book book)
        {
            //Check book?
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));
            if (entity is null)
            {
                //Log varsa buraya entegre edilebilir.
                return NotFound("Böyle bir kitap yok"); //404
            }

            //Check Id?
            if (id != book.Id)
            {
                return BadRequest("Id uyuşmuyor"); //400
            }

            //Belirtilen id'yi silme işlemi
            ApplicationContext.Books.Remove(entity);
            //Mapleme işlemi.
            book.Id = entity.Id;

            //Parametreden gelen kitap bilgisini listeye eklemiş oluyoruz.
            ApplicationContext.Books.Add(book);
            
            return Ok("Kitap bilgisi güncellendi.");
        }

        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent(); //204
        }
        //Seçilen kitabın silinmesi.
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")]int id)
        {
            //Check book?
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));

            if (entity is null)
            {
                //Olmayan bir kaynağı silme işlemi mi gerçekleşiyor.
                return NotFound(new
                {
                    statusCode = 404,
                    message = $"Id:{id} kitabı bulunamadı."
                }); //404
            }

            //Silme işlemi
            ApplicationContext.Books.Remove(entity);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")]int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            //check entity
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));
            if (entity is null)
            {
                return NotFound();//404
            }

            //ApplyTo ile yapılan değişikliği yansıtırz.
            bookPatch.ApplyTo(entity);
            return NoContent(); //204

        }

    }
}
