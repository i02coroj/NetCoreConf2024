using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace NetConf2024Search.API.Helpers;

public static class DiagnosticsConfig
{
    public const string ServiceName = "NetConf2024Search";

    public static Meter Meter = new(ServiceName);

    public static readonly Lazy<ActivitySource> _source = new(() => new ActivitySource(ServiceName));

    public static ActivitySource ActivitySource => _source.Value;
}
