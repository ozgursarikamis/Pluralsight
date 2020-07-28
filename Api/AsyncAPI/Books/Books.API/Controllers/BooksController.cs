using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.API.Entites;
using Books.API.Filters;
using Books.API.ModelBinders;
using Books.API.Models;
using Microsoft.AspNetCore.Mvc;
using Book = Books.API.Entites.Book;

namespace Books.API.Controllers
{
    [ApiController, Route("api/books")]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(
            IBookRepository bookRepository,
            IMapper mapper
            )
        {
            _bookRepository = bookRepository ??
                              throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[HttpGet]
        //[BooksResultFilter]
        //public async Task<ActionResult> GetBooks()
        //{
        //    var bookEntities = await _bookRepository.GetBooksAsync();
        //    return Ok(bookEntities);
        //}

        [HttpGet]
        [BooksResultFilter]
        public IActionResult GetBooks()
        {
            var bookEntities = _bookRepository.GetBooksAsync().Result;
            return Ok(bookEntities);
        }

        //[TypeFilter(typeof(BookResultFilterAttribute))]
        [BookWithCoversResultFilter]
        [HttpGet, Route("{id}", Name = "GetBook")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var bookEntity = await _bookRepository.GetBookAsync(id);
            if (bookEntity == null)
            {
                return NotFound();
            }
            var bookCovers = await _bookRepository.GetBookCoversAsync(id);

            //var propertyBag = new Tuple<Entities.Book, IEnumerable<ExternalModels.BookCover>>
            //    (bookEntity, bookCovers);

            //(Entities.Book book, IEnumerable<ExternalModels.BookCover> bookCovers) 
            //    propertyBag = (bookEntity, bookCovers);

            return Ok((bookEntity, bookCovers));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookForCreation bookForCreation)
        {
            var bookEntity = _mapper.Map<Book>(bookForCreation);
            _bookRepository.AddBook(bookEntity);
            await _bookRepository.SaveChangesAsync();

            // this is called so that author information won't be null in the response.
            await _bookRepository.GetBookAsync(bookEntity.Id);

            // put information data to response headers
            // about the book that is just created
            return CreatedAtRoute("GetBook", new {id = bookEntity.Id}, bookEntity);
        }
    }
    [Route("api/bookcollections")]
    [ApiController]
    [BooksResultFilter]
    public class BookCollectionsController : ControllerBase
    {
        private readonly IBookRepository _booksRepository;
        private readonly IMapper _mapper;
        public BookCollectionsController(IBookRepository booksRepository,
            IMapper mapper)
        {
            _booksRepository = booksRepository ??
                               throw new ArgumentNullException(nameof(booksRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        // api/bookcollections/(id1,id2,...)
        [HttpGet("({bookIds})", Name = "GetBookCollection")]
        public async Task<IActionResult> GetBookCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> bookIds)
        {
            var enumerable = bookIds as Guid[] ?? bookIds.ToArray();
            var bookEntities = await _booksRepository.GetBooksAsync(enumerable);

            if (enumerable.Count() != bookEntities.Count())
            {
                return NotFound();
            }

            return Ok(bookEntities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookCollection(
            IEnumerable<BookForCreation> bookCollection)
        {
            var bookEntities = _mapper.Map<IEnumerable<Book>>(bookCollection);

            var enumerable = bookEntities as Book[] ?? bookEntities.ToArray();
            foreach (var bookEntity in enumerable)
            {
                _booksRepository.AddBook(bookEntity);
            }

            await _booksRepository.SaveChangesAsync();

            var booksToReturn = await _booksRepository.GetBooksAsync(
                enumerable.Select(b => b.Id).ToList());

            var bookIds = string.Join(",", booksToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetBookCollection",
                new { bookIds },
                booksToReturn);
        }
    }
}
