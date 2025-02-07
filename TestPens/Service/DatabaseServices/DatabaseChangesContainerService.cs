
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using TestPens.Extensions;
using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Service.DatabaseServices;

public class DatabaseChangesContainerService : IChangesContainerService
{
    private readonly ILogger<JsonTierListContainerService> _logger;
    private readonly ApplicationContext _applicationContext;
    private readonly ITierListContainerService _tierListContainer;

    public DatabaseChangesContainerService(ILogger<JsonTierListContainerService> logger, ApplicationContext applicationContext, ITierListContainerService tierListContainer)
    {
        _logger = logger;
        _applicationContext = applicationContext;
        _tierListContainer = tierListContainer;
    }

    public async Task<IEnumerable<ChangeBaseModel>> GetAllChanges(int offset = 0, int limit = 100)
    {
        var genericList = (await _applicationContext.Changes.AsSplitQuery().AsNoTracking().OrderByDescending(ch => ch.UtcTime).Skip(offset).Take(limit).ToListAsync());
        return genericList.Select(change => ChangeBaseModel.Create(change)!);
    }

    public async Task<int> GetCount()
    {
        return await _applicationContext.Changes.CountAsync();
    }

    public async Task AddChanges(IReadOnlyCollection<ChangeBaseDto> changes)
    {
        if (changes.Count == 0)
            return;

        ulong chunk = (ulong)DateTime.UtcNow.Ticks;
        var newHead = _tierListContainer.GetHead().ApplyChanges(changes);
        _applicationContext.Changes!.AddRange(newHead.cachedChanges!.Select(change => change.ToGeneric(chunk)));
        newHead.cachedChanges!.Clear();
        _tierListContainer.Update(newHead);
        await _applicationContext.SaveChangesAsync();
    }

    public async Task RevertLast(int count)
    {
        if (count == 0)
            return;

        var result = await GenericRevert(col => col.Take(count));

        _applicationContext.Changes.RemoveRange(result.Item2);

        _tierListContainer.Update(result.Item1);

        await _applicationContext.SaveChangesAsync();
    }

    public async Task<TierListState> RevertLastNode(int count)
    {
        if (count == 0)
            return _tierListContainer.GetHead();

        var result = await GenericRevert(col => col.Take(count));

        return result.Item1;
    }

    public async Task RevertAllAfter(DateTime utcTime)
    {
        var result = await GenericRevert(col => col.Where(ch => ch.UtcTime > utcTime));

        _applicationContext.Changes.RemoveRange(result.Item2);

        _tierListContainer.Update(result.Item1);

        await _applicationContext.SaveChangesAsync();
    }

    public async Task<TierListState> RevertAllAfterNode(DateTime utcTime)
    {
        var result = await GenericRevert(col => col.Where(ch => ch.UtcTime > utcTime));

        return result.Item1;
    }

    private async Task<(TierListState, List<GenericChangeDatabase>)> GenericRevert(Func<IQueryable<GenericChangeDatabase>, IQueryable<GenericChangeDatabase>> func)
    {
        var genericList = await func(_applicationContext.Changes.OrderByDescending(change => change.UtcTime)).ToListAsync();

        var modelList = genericList.Select(change => ChangeBaseModel.Create(change)!).ToList();

        return (_tierListContainer.GetHead().ApplyChanges(modelList, true), genericList);
    }

}
