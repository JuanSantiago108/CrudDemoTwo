#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CrudDemoTwo.Models;


public class Post
{
    [Key]
    public int PostId { get; set; }

    [Required(ErrorMessage ="is required")]
    [MinLength(2, ErrorMessage ="must be longer than 2 chracters")]
    [MaxLength(45, ErrorMessage ="must be shorter than 45 chracters")]
    public string Topic { get; set; } 

    [Required(ErrorMessage ="is required")]
    [MinLength(2, ErrorMessage ="must be longer than 2 chracters")]
    public string Body { get; set; } 
    [Display(Name = "Image Url" )]
    public string? ImageUrl { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}