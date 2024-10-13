using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NetConf2024Search.Seeder.Model;

[Table("Books")]
public class Book : CheckSumEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AuthorId { get; set; }

    [Required]
    [ForeignKey("AuthorId")]
    public required Author Author { get; set; }

    public required string Title { get; init; }

    public required string Reference { get; init; }

    public string? Summary { get; init; }

    public required string Gendre { get; init; }

    public DateTime PublishedOn { get; init; }

    public bool InStock { get; init; }

    public int NumberOfPages { get; init; }

    public double Price { get; init; }

    [ConcurrencyCheck]
    public DateTimeOffset? LastModifiedDt { get; set; }

    public required string Languages { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

}