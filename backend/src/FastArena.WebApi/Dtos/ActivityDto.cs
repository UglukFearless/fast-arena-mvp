namespace FastArena.WebApi.Dtos;

public class ActivityDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string ActivationTitle { get; set; }
    public string Description { get; set; }
    public string DangerLevelName { get; set; }
    public string AwardLevelName { get; set; }
    public string TypeName { get; set; }
    public string IconUrl { get; set; }
}
