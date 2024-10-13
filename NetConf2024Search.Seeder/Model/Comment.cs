using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NetConf2024Search.Seeder.Model;

[Table("Comments")]
public class Comment
{
    [Key]
    public Guid Id { get; set; }

    public required string Text { get; set; }

    public required string Author { get; set; }

    public DateTime PublishedOn { get; set; }
}