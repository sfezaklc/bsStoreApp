using Azure;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryManager _manager;

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager;
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _manager.Book.GetAllBooks(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook(int id)
        {
            try
            {
                var book = _manager.Book.GetOneBookById(id, false);
                if (book is null)
                    return NotFound();

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
                if (book == null)
                    return BadRequest("Book cannot be null");

                Console.WriteLine(book.ToString());
                _manager.Book.CreateOneBook(book);
                _manager.Save();
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut("{id:int}")]
        public IActionResult UpdateOnBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
           
            try
            {
                var entity = _manager.Book.GetOneBookById(id,false);
                if (id != book.Id)
                    return NotFound(); //404
                if (entity is null)
                    return BadRequest(); //400

                entity.Title = book.Title;
                entity.Price = book.Price;
                _manager.Save();
                return Ok(book);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var entity = _manager.Book.GetOneBookById(id, false);

            if (entity is null)
                return NotFound(new
                {
                    message = $"Book with id:{id} could not found",
                    StatusCode = "404"
                }); //404

            _manager.Book.DeleteOneBook(entity);
            _manager.Save();
            return NoContent(); //204
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,[FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                var entity = _manager.Book.GetOneBookById(id, false);
                if(entity is null)
                    return NotFound();
                bookPatch.ApplyTo(entity);
                _manager.Book.Update(entity);
                return NoContent();

            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }
    }
}
