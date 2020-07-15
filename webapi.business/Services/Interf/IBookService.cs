using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IBookService
    {
        //Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int bookId);
        void CreateBook(Book book);
        void DeleteBook(int bookId);
        Task<Author> CreateSampleBookWithAuthor();
    }
}
