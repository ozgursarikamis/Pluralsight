using System;
using System.Collections.Generic;
using CourseLibrary2.API.Models;

namespace CourseLibrary2.API.Models
{
    public class AuthorForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
        public ICollection<CourseForCreationDto> Courses { get; set; }
          = new List<CourseForCreationDto>();

    }
}
