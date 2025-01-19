
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using TestPens.Extensions;
using TestPens.Models;
using TestPens.Models.Abstractions;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Service;

public class JsonPersonContainerService : IPersonContainerService
{
    private List<BaseChange>? cachedChanges;
    private TierListState? cachedHeadState;

    private ILogger<JsonPersonContainerService> _logger;
    private IConfiguration configuration;

    private string HeadPath => configuration.GetValue<string>("HeadPath")
                ?? throw new NullReferenceException("Конфигурация HeadPath не задана!");

    private string ChangesPath => configuration.GetValue<string>("ChangesPath")
            ?? throw new NullReferenceException("Конфигурация HeadPath не задана!");

    public JsonPersonContainerService(ILogger<JsonPersonContainerService> logger, IConfiguration configuration   )
    {
        _logger = logger;
        this.configuration = configuration;
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

    public IEnumerable<BaseChange> GetAllChanges(int offset = 0, int limit = int.MaxValue, DateTime? afterTime = null!)
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
        cachedChanges = JsonSerializer.Deserialize<List<BaseChange>>(content, Program.JsonOptions)!;
    }

    public void AddChanges(IEnumerable<BaseChange> changes)
    {
        EnsureCachedChanges();
        cachedHeadState = GetHead().ApplyChanges(changes);
        cachedChanges!.AddRange(changes);
        Save();
    }


    public void RevertLast(int count)
    {
        EnsureCachedChanges();
        if (cachedChanges!.Count == 0)
            return;

        int startIndex = Math.Max(cachedChanges!.Count - count, 0);
        int endIndex = cachedChanges!.Count - 1;
        int length = endIndex - startIndex + 1;

        List<BaseChange> toRevertChanges = cachedChanges!.Slice(startIndex, length);
        
        toRevertChanges.Reverse();

        cachedHeadState = GetHead().RevertChanges(toRevertChanges);

        cachedChanges.RemoveRange(startIndex, length);

        Save();
    }

    public void RevertAllAfter(DateTime utsTime)
    {
        // TODO: когда займусь машиной времени
    }

    public void Save()
    {
        TierListState head = GetHead();
        var changes = GetAllChanges();
        File.WriteAllText(HeadPath, JsonSerializer.Serialize(head.TierList, Program.JsonOptions));
        File.WriteAllText(ChangesPath, JsonSerializer.Serialize(changes, Program.JsonOptions));
    }
}
