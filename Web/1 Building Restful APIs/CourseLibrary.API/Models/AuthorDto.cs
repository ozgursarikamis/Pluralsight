using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Models
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string MainCategory { get; set; }
    }

    public class AuthorForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
        public ICollection<CourseForCreationDto> Courses { get; set; }
         = new List<CourseForCreationDto>();
    }

    public class AuthorForCreationWithDateOfDeathDto : AuthorForCreationDto
    {
        public DateTimeOffset? DateOfDeath { get; set; }
    }

    public class AuthorFullDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
    }
}
