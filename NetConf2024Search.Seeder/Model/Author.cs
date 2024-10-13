using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetConf2024Search.Seeder.Model;

[Table("Authors")]
public class Author
{
    [Key]
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Bio { get; set; }
}
