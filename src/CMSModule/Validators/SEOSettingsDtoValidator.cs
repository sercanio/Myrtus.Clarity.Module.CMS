using FluentValidation;
using global::CMSModule.DTOs;

namespace CMSModule.Validators;

public class SEOSettingsDtoValidator : AbstractValidator<SEOSettingsDto>
{
    public SEOSettingsDtoValidator()
    {
        RuleFor(x => x.DefaultMetaTitle)
            .NotEmpty().WithMessage("Default Meta Title is required.")
            .MaximumLength(60).WithMessage("Meta title cannot exceed 60 characters.");

        RuleFor(x => x.DefaultMetaDescription)
            .NotEmpty().WithMessage("Default Meta Description is required.")
            .MaximumLength(160).WithMessage("Meta description cannot exceed 160 characters.");

        RuleFor(x => x.DefaultMetaKeywords)
            .NotEmpty().WithMessage("Default Meta Keywords are required.")
            .MaximumLength(255).WithMessage("Meta keywords cannot exceed 255 characters.");
    }
}
