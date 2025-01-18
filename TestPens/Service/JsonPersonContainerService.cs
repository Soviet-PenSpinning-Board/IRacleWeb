
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

    public IReadOnlyList<BaseChange> GetAllChanges()
    {
        if (cachedChanges != null)
            return cachedChanges;

        if (!File.Exists(ChangesPath))
        {
            File.WriteAllText(ChangesPath, "[]");
            return cachedChanges = new List<BaseChange>();
        }

        string content = File.ReadAllText(ChangesPath);
        return cachedChanges = JsonSerializer.Deserialize<List<BaseChange>>(content, Program.JsonOptions)!;
    }

    public TierListState GetHead()
    {
        if (cachedHeadState != null)
            return cachedHeadState;

        if (!File.Exists(HeadPath))
        {
            File.WriteAllText(HeadPath, "{}");
            return cachedHeadState = new TierListState(new(40));
        }

        string content = File.ReadAllText(HeadPath);
        var data = JsonSerializer.Deserialize<Dictionary<Tier, List<PersonModel>>>(content, Program.JsonOptions)!;
        return cachedHeadState = new TierListState(data);
    }

    public void AddChanges(IEnumerable<BaseChange> changes)
    {
        _ = GetAllChanges();
        foreach (BaseChange change in changes)
        {
            change.Initialize(GetHead());
            cachedChanges!.Add(change);
            GetHead().MakeChange(change);
        }
        Save();
    }

    public void Save()
    {
        TierListState head = GetHead();
        var changes = GetAllChanges();
        File.WriteAllText(HeadPath, JsonSerializer.Serialize(head.TierList, Program.JsonOptions));
        File.WriteAllText(ChangesPath, JsonSerializer.Serialize(changes, Program.JsonOptions));
    }

    public void RevertLast(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RevertLastOne(false);
        }

        Save();
    }

    public void RevertAllAfter(DateTime utsTime)
    {
    }

    private void RevertLastOne(bool save = true)
    {
        var changes = GetAllChanges();
        if (changes.Count == 0)
            return;

        BaseChange last = changes.Last();

        var head = GetHead();

        head.Revert(last);

        cachedChanges!.RemoveAt(cachedChanges.Count - 1);

        if (save)
            Save();
    }
}
