using System;
using System.Threading.Tasks;
using Books.API.Entites;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
    [ApiController, Route("api/sync-books")]
    public class SyncBooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        public SyncBooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ??
                              throw new ArgumentNullException(nameof(bookRepository));
        }

        [HttpGet]
        public ActionResult GetBooks()
        {
            var bookEntities = _bookRepository.GetBooks();
            return Ok(bookEntities);
        }
    }
}