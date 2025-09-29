using System.Data;
using FluentValidation;
using Module_01.Requests;

namespace Module_01;

public class CreateProductRequestValidator: AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .Length(1, 255).WithMessage("Product name must be between 1 and 255.");

        RuleFor(x => x.Description)
            .Length(1, 1000).WithMessage("Description can't exceed 1000 characters.");

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.")
            .Matches(@"^PRD-\d{5}$").WithMessage("SKU must be 'PRD-' followed by 5 digits 'PRD-XXXXX'.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price value must be at least 0.01.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity value must be at least 0.");

        RuleFor(x => x.LaunchDate)
            .Must(BeInTodayOrFuture).WithMessage("Launch date is required.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid product category.");

        RuleFor(x => x.ImageUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl must be a valid Url.");

        RuleFor(x => x.Weight)
            .InclusiveBetween(0.01m, 1000).WithMessage("Weight must be between 0.01 and 1000 kg.");

        RuleFor(x => x.WarrantyPeriodMonths)
            .Must(MustBe12_24_36).WithMessage("warrant must be 0, 12, 24, 36 months.");

        When(x => x.IsReturnable, () =>
        {
            RuleFor(x => x.ReturnPolicyDescription)
                .NotEmpty().WithMessage("Return policy description is required if IsReturnable is true.");
        });

        RuleFor(x => x.Tags)
            .Must(tags => tags.Count <= 5).WithMessage("A maximum of 5 tags is allowed");
    }

    private bool MustBe12_24_36(int value)
    => value == 0 || value == 12 || value == 24 || value == 36; 

    private bool BeValidUrl(string? url)
    => Uri.TryCreate(url, UriKind.Absolute, out _);

    private bool BeInTodayOrFuture(DateTime time)
    => time.Date >= DateTime.UtcNow.Date; 
}
