
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using TestPens.Extensions;
using TestPens.Models;
using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Service;

public class JsonTierListContainerService : ITierListContainerService
{
    private TierListState? cachedHeadState;

    private ILogger<JsonTierListContainerService> _logger;

    private string HeadPath { get; }

    public JsonTierListContainerService(ILogger<JsonTierListContainerService> logger, IConfiguration configuration   )
    {
        _logger = logger;

        HeadPath = Path.Combine(configuration.GetValue<string>("ConfigPath")!, "main.json");
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


    public void Save()
    {
        TierListState head = GetHead();
        File.WriteAllText(HeadPath, JsonSerializer.Serialize(head.TierList, Program.JsonOptions));
    }

    public void Update(TierListState newState)
    {
        cachedHeadState = newState;
        Save();
    }
}
