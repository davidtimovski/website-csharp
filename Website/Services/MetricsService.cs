using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Website.Services;

public class MetricsService
{
    private const string MeterName = "Website";
    private const string RouteTag = "route";

    public MetricsService(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        HitsCounter = meter.CreateCounter<int>("website.hits");
    }

    public void LogHit(string route)
    {
        HitsCounter.Add(1, new KeyValuePair<string, object?>(RouteTag, route));
    }

    public Counter<int> HitsCounter { get; private set; }
}
