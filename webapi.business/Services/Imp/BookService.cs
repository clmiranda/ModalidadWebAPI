using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class BookService : IBookService
    {
        private IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        { _unitOfWork = unitOfWork; }

        //public Task<IEnumerable<Book>> GetAllBooks() => _unitOfWork.BookRepository.GetAll();
        public Task<Book> GetBookById(int bookId) => _unitOfWork.BookRepository.GetById(bookId);
        public void CreateBook(Book book)
        {
            _unitOfWork.BookRepository.Insert(book);
            _unitOfWork.SaveAll();
            return;
        }

        public void DeleteBook(int bookId)
        {
            //_unitOfWork.BookRepository.Delete(bookId);
            //_unitOfWork.Commit();

        }
        public Task<Author> CreateSampleBookWithAuthor()
        {
            Book gambler = new Book("The Gambler");
            Author fyodor = new Author("Fyodor Dostoyevsky", "Russia", new List<Book>() { gambler });

            try
            {
                _unitOfWork.BookRepository.Insert(gambler);
                _unitOfWork.AuthorRepository.Insert(fyodor);
                _unitOfWork.SaveAll();
            }
            catch
            {
                _unitOfWork.Rollback();
                return Task.FromResult(new Author());
            }

            return _unitOfWork.AuthorRepository.GetByName(fyodor.Name);
        }
    }
}
