
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;

namespace FastArena.Core.Services;

public class MonsterFightResultService : IMonsterFightResultService
{
    private readonly IMonsterFightResultStorage _resultStorage;
    public MonsterFightResultService(IMonsterFightResultStorage resultStorage)
    {
        _resultStorage = resultStorage;
    }

    public async Task<MonsterFightResult> AddLoseResultAsync(MonsterFight fight)
    {
        var resultCreationModel = BuildResultCreationModel(fight);
        resultCreationModel.Type = MonsterFightResultType.DEFEAT;
        var result = await _resultStorage.CreateAsync(resultCreationModel);
        return result;
    }

    public async Task<MonsterFightResult> AddWinResultAsync(MonsterFight fight)
    {
        var resultCreationModel = BuildResultCreationModel(fight);
        resultCreationModel.Type = MonsterFightResultType.VICTORY;
        var result = await _resultStorage.CreateAsync(resultCreationModel);
        return result;
    }

    private MonsterFightResultCreationModel BuildResultCreationModel(MonsterFight fight)
    {
        var lastState = fight.State.MaxBy(s => s.Key);
        ValidateTheLastState(lastState.Value);
        return new MonsterFightResultCreationModel
        {
            HeroId = fight.Hero.Id,
            Monster = fight.Monster,
        };
    }

    private void ValidateTheLastState(MonsterFightActionState state)
    {
        if (state.Result == null)
        {
            throw new Exception("The last state doesn't have a result! It can't be finalized!");
        }

        if (state.HeroHealth > 0 && state.MonsterHealth > 0)
        {
            throw new Exception("The hero and the monster both are alive! It's impossible to determine the winner.");
        }
    }
}
