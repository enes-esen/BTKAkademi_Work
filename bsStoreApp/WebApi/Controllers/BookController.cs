using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.EFCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepositoryManager _manager;

        public BookController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.Book.GetAllBooks(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = _manager
                    .Book
                    .GetOneBookById(id, false);

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

                _manager.Book.CreateOneBook(book);
                _manager.Save();

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
                var entity = _manager
                    .Book
                    .GetOneBookById(id, true);

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
                _manager.Save();

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
                var entity = _manager
                    .Book
                    .GetOneBookById(id, false);

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
                _manager.Book.DeleteOneBook(entity);
                _manager.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                //check entity
                var entity = _manager
                    .Book
                    .GetOneBookById(id, true);

                if (entity is null)
                {
                    return NotFound();//404
                }

                //ApplyTo ile yapılan değişikliği yansıtırz.
                bookPatch.ApplyTo(entity);
                _manager.Book.Update(entity);

                return NoContent(); //204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
