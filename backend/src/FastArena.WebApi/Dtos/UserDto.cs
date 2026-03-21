namespace FastArena.WebApi.Dtos;

public class UserDto
{
    public Guid Id { set; get; }
    public required string Login { set; get; }
    public Guid? SelectedHeroId { set; get; }
}
