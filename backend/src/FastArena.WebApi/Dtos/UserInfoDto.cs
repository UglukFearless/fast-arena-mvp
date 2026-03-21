namespace FastArena.WebApi.Dtos;

public class UserInfoDto
{
    public required UserDto User {  get; set; }
    public required ICollection<HeroDto> Heroes { get; set; }
}
