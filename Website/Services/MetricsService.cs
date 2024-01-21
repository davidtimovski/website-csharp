using System.Diagnostics.Metrics;

namespace Website.Services;

public class MetricsService
{
    public const string MeterName = "Website";
    public const string RouteTag = "route";

    public MetricsService(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        HitsCounter = meter.CreateCounter<int>("website.hits");
    }

    public Counter<int> HitsCounter { get; private set; }
}
