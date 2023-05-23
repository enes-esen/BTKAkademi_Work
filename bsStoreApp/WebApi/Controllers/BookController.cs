using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public BookController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")]int id)
        {
            try
            {
                var book = _context.Books
                    .Where(b => b.Id.Equals(id))
                    .SingleOrDefault();

                if (book is null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
                _context.Books.Add(book);
                _context.SaveChanges();
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] Book book)
        {
            try
            {
                //Check book?
                var entity = _context
                    .Books
                    .Where(b => b.Id.Equals(id))
                    .SingleOrDefault();

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


                //Mapleme işlemi.
                entity.Title = book.Title;
                entity.Price = book.Price;
                //Parametreden gelen kitap bilgisini listeye eklemiş oluyoruz.
                _context.SaveChanges();

                return Ok("Kitap bilgisi güncellendi.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //Seçilen kitabın silinmesi.
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                //Check book?
                var entity = _context
                    .Books
                    .Where(b => b.Id.Equals(id))
                    .SingleOrDefault();

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
                _context.Books.Remove(entity);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
