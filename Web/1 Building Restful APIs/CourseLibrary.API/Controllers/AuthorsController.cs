using System;
using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    // inheriting from Controller would bring things
    // that we won't need while developing API
    // such as Views, model state access etc

    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(
            ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper
            )
        { 
            _courseLibraryRepository = courseLibraryRepository ??
                                       throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetAuthors")] 
        [HttpHead] // HttpHead: Identifies an action that supports HTTP HEAD method
        public IActionResult GetAuthors(
            [FromQuery] AuthorsResourceParameters parameters
        )
        {
            var authorsFromRepo = _courseLibraryRepository
                .GetAuthors(parameters);

            var previousPageLink = authorsFromRepo.HasPrevious
                ? CreateAuthorsResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;
            var nextPageLink = authorsFromRepo.HasNext
                ? CreateAuthorsResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                authorsFromRepo.TotalCount,
                authorsFromRepo.PageSize,
                authorsFromRepo.CurrentPage,
                authorsFromRepo.TotalPages,
                PreviousPageLink = previousPageLink,
                NextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(
                _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo)
                    .ShapeData(parameters.Fields)
                );
        }

        [HttpGet("{authorId:guid}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId, string fields)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);
            if (authorFromRepo == null)
            {
                return NotFound("Author Not Found");
            }
            return Ok(_mapper.Map<AuthorDto>(authorFromRepo).ShapeData(fields));
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            var authorEntity = _mapper.Map<Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();
             
            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", new {authorId = authorToReturn.Id}, authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteAuthor(authorFromRepo);
            _courseLibraryRepository.Save();

            return NoContent();
        }

        private string CreateAuthorsResourceUri(
            AuthorsResourceParameters parameters, ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link("GetAuthors",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        mainCategory = parameters.MainCategory,
                        searchQuery = parameters.SearchQuery
                    }),
                ResourceUriType.NextPage => Url.Link("GetAuthors",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        mainCategory = parameters.MainCategory,
                        searchQuery = parameters.SearchQuery
                    }),
                _ => Url.Link("GetAuthors",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        mainCategory = parameters.MainCategory,
                        searchQuery = parameters.SearchQuery
                    })
            };
        }
    }
}