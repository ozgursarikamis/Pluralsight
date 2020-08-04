using System;
using System.Collections.Generic;
using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public CoursesController(IMapper mapper, ICourseLibraryRepository courseLibraryRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository)); ;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CoursesDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourses(authorId);
            return Ok(_mapper.Map<IEnumerable<CoursesDto>>(courseForAuthorFromRepo));
        }

        [HttpGet("{courseId}")]
        public ActionResult<CoursesDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (_courseLibraryRepository == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CoursesDto>(courseForAuthorFromRepo));
        }
    }
}