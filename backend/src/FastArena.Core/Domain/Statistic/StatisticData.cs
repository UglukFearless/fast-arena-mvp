namespace FastArena.Core.Domain.Statistic;

public class StatisticData
{
    public required string Title { get; set; }
    public required string ValueTitle { get; set; }
    public string ValueSymbols { get; set; }
    public required ICollection<StatisticDataRow> Rows { get; set; }
}
