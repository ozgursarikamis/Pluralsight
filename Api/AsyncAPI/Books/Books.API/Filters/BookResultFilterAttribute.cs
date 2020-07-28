using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Books.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Books.API.Filters
{
    public class BooksResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var resultFromTheAction = context.Result as ObjectResult;
            if (resultFromTheAction?.Value == null
                || resultFromTheAction.StatusCode < 200
                || resultFromTheAction.StatusCode >= 300
            )
            {
                await next();
                return;
            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();
            resultFromTheAction.Value = mapper.Map<IEnumerable<Book>>(resultFromTheAction.Value);
            await next();
        }
    }
    public class BookResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction?.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            resultFromAction.Value = mapper.Map<Models.Book>(resultFromAction.Value);

            await next();
        }
    }

    public class BookWithCoversResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {

            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction?.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            //var (book, bookCovers) = ((Entities.Book book, 
            //    IEnumerable<ExternalModels.BookCover> bookCovers))resultFromAction.Value;

            //var temp = ((Entities.Book, 
            //    IEnumerable<ExternalModels.BookCover>))resultFromAction.Value;

            var (book, bookCovers) = ((Book, IEnumerable<ExternalModels.BookCover>))resultFromAction.Value;

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            var mappedBook = mapper.Map<BookWithCovers>(book);
            resultFromAction.Value = mapper.Map(bookCovers, mappedBook);

            await next();
        }
    }
}
