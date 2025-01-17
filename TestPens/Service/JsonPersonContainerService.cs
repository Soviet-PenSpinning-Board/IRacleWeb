
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
    private TierListState? headState;

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

    public IReadOnlyCollection<BaseChange> GetAllChanges()
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
        if (headState != null)
            return headState;

        if (!File.Exists(HeadPath))
        {
            File.WriteAllText(HeadPath, "{}");
            return headState = new TierListState(new(40));
        }

        string content = File.ReadAllText(HeadPath);
        var data = JsonSerializer.Deserialize<Dictionary<Tier, List<PersonModel>>>(content, Program.JsonOptions)!;
        return headState = new TierListState(data);
    }

    public void AddChange(BaseChange change)
    {
        _ = GetAllChanges();
        cachedChanges!.Add(change);
        GetHead().MakeChange(change);
        Save();
    }

    public void Save()
    {
        TierListState head = GetHead();
        var changes = GetAllChanges();
        File.WriteAllText(HeadPath, JsonSerializer.Serialize(head.TierList, Program.JsonOptions));
        File.WriteAllText(ChangesPath, JsonSerializer.Serialize(changes, Program.JsonOptions));
    }
}
