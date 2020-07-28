using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Books.API.ExternalModels;
using Books.Legacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Books.API.Entites
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks();
        Task<IEnumerable<Book>> GetBooksAsync();
        // Task<Book> GetBookAsync(Guid id);
        void AddBook(Book book);
        Task<bool> SaveChangesAsync();
        Task<Book> GetBookAsync(Guid id);
        Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds);
        Task<BookCover> GetBookCoverAsync(string coverId);
        Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId);
    }
    public class BookRepository : IBookRepository, IDisposable
    {
        private BooksContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(
            BooksContext context, 
            IHttpClientFactory httpClientFactory,
            ILogger<BookRepository> logger
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Book> GetBooks()
        {
            _context.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:02'");
            return _context.Books.Include(b => b.Author).ToList();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            _context.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:02'");
            return await _context.Books.Include(b => b.Author).ToListAsync();
        }

        private Task<int> GetBookPages()
        {
            return Task.Run(() =>
            {
                var pageCalculator = new ComplicatedPageCalculator();
                return pageCalculator.CalculateBookPages();
            });
        }
        public async Task<Book> GetBookAsync(Guid id)
        {
            //var pageCalculator = new ComplicatedPageCalculator();
            //var amountOfPages = pageCalculator.CalculateBookPages();

            var bookPages = await GetBookPages();

            return await _context.Books.Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds)
        {
            return await _context.Books.Where(b => bookIds.Contains(b.Id))
                .Include(b => b.Author).ToListAsync();
        }

        public async Task<BookCover> GetBookCoverAsync(string coverId)
        { 
            _cancellationTokenSource = new CancellationTokenSource();
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient
                .GetAsync($"http://localhost:52644/api/bookcovers/{coverId}");

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(), 
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }

            return null;
        }

        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var bookCovers = new List<BookCover>();

            // create a list of fake bookcovers
            var bookCoverUrls = new[]
            {
                $"http://localhost:52644/api/bookcovers/{bookId}-dummycover1",
                // $"http://localhost:52644/api/bookcovers/{bookId}-dummycover2?returnFault=true",
                $"http://localhost:52644/api/bookcovers/{bookId}-dummycover2",
                $"http://localhost:52644/api/bookcovers/{bookId}-dummycover3",
                $"http://localhost:52644/api/bookcovers/{bookId}-dummycover4",
                $"http://localhost:52644/api/bookcovers/{bookId}-dummycover5"
            };

            var downloadTasks = from bookCoverUrl in bookCoverUrls
                select DownloadBookCoverAsync(
                    httpClient, bookCoverUrl, _cancellationTokenSource.Token);

            var enumerable = downloadTasks as Task<BookCover>[] ?? downloadTasks.ToArray();
            try
            {
                return await Task.WhenAll(enumerable.ToList());
            }
            catch (OperationCanceledException operationCanceledException)
            {
                _logger.LogInformation($"{operationCanceledException.Message}");
                foreach (var task in enumerable.ToList())
                {
                    _logger.LogInformation($"Task {task.Id} has status {task.Status}");
                }

                return new List<BookCover>();
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception.Message}");
                throw;
            }
            //foreach (var bookCoverUrl in bookCoverUrls)
            //{
            //    var response = await httpClient.GetAsync(bookCoverUrl);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        bookCovers.Add(JsonSerializer.Deserialize<BookCover>(
            //            await response.Content.ReadAsStringAsync(),
            //            new JsonSerializerOptions {PropertyNameCaseInsensitive = true}
            //        ));
            //    }
            //}

            //return bookCovers;
        }

        private async Task<BookCover> DownloadBookCoverAsync(
            HttpClient httpClient, 
            string bookCoverUrl, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(bookCoverUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var bookCover = JsonSerializer.Deserialize<ExternalModels.BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
                return bookCover;
            }
            _cancellationTokenSource.Cancel();
            return null;
        }

        public void AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _context.AddAsync(book);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed:
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // tell GC that this object is already cleaned up.
        }
        public virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            if (_cancellationTokenSource == null) return;
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
}