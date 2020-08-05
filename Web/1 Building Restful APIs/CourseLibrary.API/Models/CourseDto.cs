using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.ValidationAttributes;

namespace CourseLibrary.API.Models
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
    }

    // custom attributes executed before Validate method gets called
    [CourseTitleMustBeDifferentFromDescription]
    public class CourseForCreationDto // : IValidatableObject
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "The provided description should be different from the title",
        //            new[] {"CourseCreationForDto" }
        //        );
        //    }
        //}
    }
}