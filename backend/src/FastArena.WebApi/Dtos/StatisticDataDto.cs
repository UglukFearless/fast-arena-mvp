namespace FastArena.WebApi.Dtos;

public class StatisticDataDto
{
    public required string Title { get; set; }
    public required string ValueTitle { get; set; }
    public string ValueSymbols { get; set; }
    public required ICollection<StatisticDataRowDto> data { get; set; }
}
