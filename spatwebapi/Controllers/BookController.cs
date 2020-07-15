using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService bookService;

        public BookController(IBookService bookService)
        { this.bookService = bookService; }

        [HttpGet("GetAllBooks")]
        [AllowAnonymous]
        //public async Task<IEnumerable<Book>> GetAllBooks() => await bookService.GetAllBooks();

        [HttpGet]
        [Route("id/{bookId}")]
        public async Task<Book> GetBookById(int bookId) => await bookService.GetBookById(bookId);

        [HttpPost]
        [AllowAnonymous]
        public void AddBook([FromBody] Book book) => bookService.CreateBook(book);

        [HttpDelete]
        [Route("{bookId}")]
        [AllowAnonymous]
        public void DeleteBook(int bookId) => bookService.DeleteBook(bookId);


        [HttpGet("sample")]
        public Task<Author> CreateSample() => bookService.CreateSampleBookWithAuthor();
    }
}
