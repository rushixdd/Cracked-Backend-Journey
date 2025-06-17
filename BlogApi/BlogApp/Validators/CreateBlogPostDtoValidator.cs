using BlogApp.DTOs;
using FluentValidation;

namespace BlogApp.Validators
{
    public class CreateBlogPostDtoValidator : AbstractValidator<CreatePostDto>
    {
        public CreateBlogPostDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.");

            RuleForEach(x => x.Tags)
                .NotEmpty().WithMessage("Tags cannot contain empty values.");
        }
    }
}
