namespace FSH.WebApi.Application.Dashboard;

public class StatsDto
{
    public int GroupCount { get; set; }
    public int TraderCount { get; set; }
    public int OfferCount { get; set; }
    public int InquiryCount { get; set; }
    public List<ChartSeries> DataEnterBarChart { get; set; } = new();
    public Dictionary<string, double>? InquiryByTraderGroupPieChart { get; set; }
}

public class ChartSeries
{
    public string? Name { get; set; }
    public double[]? Data { get; set; }
}