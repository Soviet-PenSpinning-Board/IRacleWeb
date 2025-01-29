
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using TestPens.Extensions;
using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Service;

public class JsonPersonContainerService : IPersonContainerService
{
    private List<ChangeBaseModel>? cachedChanges;
    private TierListState? cachedHeadState;

    private ILogger<JsonPersonContainerService> _logger;

    private string HeadPath { get; }

    private string ChangesPath { get; }

    public JsonPersonContainerService(ILogger<JsonPersonContainerService> logger, IConfiguration configuration   )
    {
        _logger = logger;

        HeadPath = Path.Combine(configuration.GetValue<string>("ConfigPath")!, "main.json");
        ChangesPath = Path.Combine(configuration.GetValue<string>("ConfigPath")!, "changes.json");
    }

    public TierListState GetHead()
    {
        EnsureCachedHead();

        return cachedHeadState!;
    }

    private void EnsureCachedHead()
    {
        if (cachedHeadState != null)
            return;

        if (!File.Exists(HeadPath))
        {
            File.WriteAllText(HeadPath, "{}");
            cachedHeadState = new TierListState(new());
            return;
        }

        string content = File.ReadAllText(HeadPath);
        var data = JsonSerializer.Deserialize<Dictionary<Tier, List<PersonModel>>>(content, Program.JsonOptions)!;
        cachedHeadState = new TierListState(data);
    }

    public IEnumerable<ChangeBaseModel> GetAllChanges(int offset = 0, int limit = int.MaxValue, DateTime? afterTime = null!)
    {
        EnsureCachedChanges();

        var enumerable = afterTime != null ? cachedChanges!.Where(c => c.UtcTime > afterTime) : cachedChanges!;

        return enumerable.Skip(offset).Take(limit);
    }

    private void EnsureCachedChanges()
    {
        if (cachedChanges != null)
            return;

        if (!File.Exists(ChangesPath))
        {
            File.WriteAllText(ChangesPath, "[]");
            cachedChanges = new();
            return;
        }

        string content = File.ReadAllText(ChangesPath);
        cachedChanges = JsonSerializer.Deserialize<List<ChangeBaseModel>>(content, Program.JsonOptions)!;
    }

    public void AddChanges(IReadOnlyCollection<ChangeBaseDto> changes)
    {
        EnsureCachedChanges();
        cachedHeadState = (GetHead().ApplyChanges(changes));
        cachedChanges!.AddRange(cachedHeadState.cachedChanges!);
        cachedHeadState.cachedChanges!.Clear();
        Save();
    }


    public void RevertLast(int count)
    {
        EnsureCachedChanges();
        if (cachedChanges!.Count == 0 || count == 0)
            return;

        // блин
        int startIndex = Math.Max(cachedChanges!.Count - count, 0);
        int endIndex = cachedChanges!.Count - 1;
        int length = endIndex - startIndex + 1;

        cachedHeadState = RevertLastNode(count);

        cachedChanges.RemoveRange(startIndex, length);

        Save();
    }

    public TierListState RevertLastNode(int count)
    {
        EnsureCachedChanges();
        if (cachedChanges!.Count == 0 || count == 0)
            return GetHead();

        // блин
        int startIndex = Math.Max(cachedChanges!.Count - count, 0);
        int endIndex = cachedChanges!.Count - 1;
        int length = endIndex - startIndex + 1;

        List<ChangeBaseModel> toRevertChanges = cachedChanges!.Slice(startIndex, length);

        toRevertChanges.Reverse();

        return GetHead().ApplyChanges(toRevertChanges, true);
    }

    public void RevertAllAfter(DateTime utsTime)
    {
        EnsureCachedChanges();
        for (int i = cachedChanges!.Count - 1; i >= 0; i--)
        {
            if (cachedChanges![i].UtcTime < utsTime)
            {
                RevertLast(cachedChanges!.Count - 1 - i);
                return;
            }
        }
    }

    public TierListState RevertAllAfterNode(DateTime utsTime)
    {
        EnsureCachedChanges();
        for (int i = cachedChanges!.Count - 1; i >= 0; i--)
        {
            if (cachedChanges![i].UtcTime < utsTime)
            {
                return RevertLastNode(cachedChanges!.Count - 1 - i);
            }
        }

        return RevertLastNode(cachedChanges!.Count);
    }

    public void Save()
    {
        TierListState head = GetHead();
        File.WriteAllText(HeadPath, JsonSerializer.Serialize(head.TierList, Program.JsonOptions));
        File.WriteAllText(ChangesPath, JsonSerializer.Serialize(cachedChanges, Program.JsonOptions));
    }
}
