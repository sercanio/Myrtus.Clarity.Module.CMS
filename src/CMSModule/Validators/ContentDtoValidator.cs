using CMSModule.DTOs;
using FluentValidation;

namespace CMSModule.Validators;
public class ContentDtoValidator : AbstractValidator<ContentDto>
{
    public ContentDtoValidator()
    {
        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Content type is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be URL-friendly.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.");

        // SEO Fields can have optional rules
        RuleFor(x => x.MetaTitle)
            .MaximumLength(60).WithMessage("Meta title cannot exceed 60 characters.");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(160).WithMessage("Meta description cannot exceed 160 characters.");

        RuleFor(x => x.MetaKeywords)
            .MaximumLength(255).WithMessage("Meta keywords cannot exceed 255 characters.");
    }
}