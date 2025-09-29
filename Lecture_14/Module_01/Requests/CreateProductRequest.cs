using System.ComponentModel.DataAnnotations;
using Module_01.Enums;
using Module_01.Validators;

namespace Module_01.Requests;

public class CreateProductRequest
{
    /*
    
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(maximumLength:255, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 255.")]
        public string? Name { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage ="Description can't exceed 1000 characters.")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "SKU is required.")]
        [RegularExpression(pattern: @"^PRD-\d{5}$", ErrorMessage = "SKU must be 'PRD-' followed by 5 digits 'PRD-XXXXX'. ")]
        public string? SKU { get; set; }

        [Range(minimum: 0.01, maximum: double.MaxValue, ErrorMessage = "Price value must be at least 0.01.")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        
        [Required(ErrorMessage = "Launch date is required.")]
        [CustomValidation(typeof(LaunchDateValidator), nameof(LaunchDateValidator.MustBeTodayOrFuture))]
        public DateTime LaunchDate { get; set; }

        [EnumDataType(typeof(ProductCategory), ErrorMessage = "Invalid product category.")]
        public ProductCategory Category { get; set; }

        [Url(ErrorMessage = "ImageUrl must be a valid Url.")]
        public string? ImageUrl { get; set; }

        [Range(minimum: 0.01, maximum: 1000, ErrorMessage = "Weight must be between 0.01 and 1000 kg.")]
        public decimal Weight { get; set; }

        [Range(minimum: 0, maximum: 36, ErrorMessage = "warrant must be 0, 12, 24, 36 months.")]
        [CustomValidation(typeof(WarrantyValidator), nameof(WarrantyValidator.MustBe12_24_36))]
        public int WarrantyPeriodMonths { get; set; }
        public bool IsReturnable { get; set; }

        [RequiredIf("IsReturnable", true, ErrorMessage = "Return policy description is required if IsReturnable is true.")]
        public string? ReturnPolicyDescription { get; set; }

        [MaxLength(length: 5, ErrorMessage = "A maximum of 5 tags is allowed")]
        public List<string> Tags { get; set; } = new();
        
    */

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime LaunchDate { get; set; }
    public ProductCategory Category { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Weight { get; set; }
    public int WarrantyPeriodMonths { get; set; }
    public bool IsReturnable { get; set; }
    public string? ReturnPolicyDescription { get; set; }
    public List<string> Tags { get; set; } = new();
}
