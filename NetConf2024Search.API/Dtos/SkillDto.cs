namespace NetConf2024Search.API.Dtos;

public class SkillDto
{
    public required string Type { get; set; }

    public required List<string> Inputs { get; set; }

    public required List<string> Outputs { get; set; }
}