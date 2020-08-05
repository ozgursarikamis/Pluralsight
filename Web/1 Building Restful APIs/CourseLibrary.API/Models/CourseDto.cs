using System;
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
    [CourseTitleMustBeDifferentFromDescription
        (ErrorMessage = "Title must be different from the description.")]
    public class CourseForCreationDto // : IValidatableObject
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters")]
        public string Title { get; set; }
        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters")]
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

    [CourseTitleMustBeDifferentFromDescription
        (ErrorMessage = "Title must be different from the description.")]
    public class CourseForUpdateDto
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You should fill out a description.")]
        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters")]
        public string Description { get; set; }
    }
}