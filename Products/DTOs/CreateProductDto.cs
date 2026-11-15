using System.ComponentModel.DataAnnotations;

namespace Products.DTOs;

public class CreateProductDto
{
  [Required(ErrorMessage = "Title is required")]
  [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
  public string Title { get; set; } = string.Empty;

  [Required(ErrorMessage = "Price is required")]
  [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
  public decimal Price { get; set; }

  [Required(ErrorMessage = "Description is required")]
  [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
  public string Description { get; set; } = string.Empty;

  [Required(ErrorMessage = "Category is required")]
  [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
  public string Category { get; set; } = string.Empty;

  [Url(ErrorMessage = "Image must be a valid URL")]
  [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
  public string Image { get; set; } = string.Empty;
}
