using FastArena.Core.Domain.Activities;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal class ActivityProfile
{
    public static ActivityDto Map(Activity activity)
    {
        if (activity == null)
            return null;

        return new ActivityDto
        {
            Id = activity.Id,
            Title = activity.Title,
            Description = activity.Description,
            ActivationTitle = activity.ActivationTitle,
            DangerLevelName = ConverToString(activity.DangerLevel),
            AwardLevelName = ConverToString(activity.AwardLevel),
            TypeName = ConverToString(activity.Type),
            IconUrl = activity.IconUrl,
        };
    }

    public static List<ActivityDto> Map(List<Activity> heroes) => heroes.ConvertAll(Map);

    private static string ConverToString(ActivityDangerLevel level) => level switch
    {
        ActivityDangerLevel.LOW => "Низкий",
        ActivityDangerLevel.MEDIUM => "Средний",
        ActivityDangerLevel.HIGH => "Высокий",
        _ => throw new NotImplementedException(),
    };

    private static string ConverToString(ActivityAwardLevel level) => level switch
    {
        ActivityAwardLevel.LOW => "Низкая",
        ActivityAwardLevel.MEDIUM => "Средняя",
        ActivityAwardLevel.HIGH => "Высокая",
        _ => throw new NotImplementedException(),
    };

    private static string ConverToString(ActivityType type) => type switch
    {
        ActivityType.REGULAR => "Регулярное",
        ActivityType.ONE_TIME => "Разовое",
        _ => throw new NotImplementedException(),
    };
}
