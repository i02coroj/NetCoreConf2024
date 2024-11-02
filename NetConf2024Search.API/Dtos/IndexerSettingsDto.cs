namespace NetConf2024Search.API.Dtos;

public class IndexerSettingsDto
{
    public required string DataSourceConnectionString { get; set; }

    public required string DataSourceType { get; set; }

    public required string DataSourceName { get; set; }

    public required string DataSourceContainerName { get; set; }

    public required string IndexerName { get; set; }

    public required string TargetIndex { get; set; }

    public required string WaterMarkField { get; set; }

    public required string SoftDeleteField { get; set; }

    public required string SoftDeleteMarkerValue { get; set; }

    public int? IndexingScheduleInMin { get; set; }

    public string? AttachedAIServicesKey { get; set; }

    public List<SkillDto>? Skills { get; set; }
}