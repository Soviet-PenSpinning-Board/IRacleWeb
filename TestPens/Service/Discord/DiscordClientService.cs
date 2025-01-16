using Discord.WebSocket;
using Discord;
using Discord.Interactions;
using System.Reflection;
using System.Windows.Input;

namespace TestPens.Service.Discord;

public class DiscordClientService :  IHostedService, IDisposable
{
    private readonly ILogger<DiscordClientService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider services;

    public DiscordSocketClient Client { get; }
    public InteractionService InteractionService { get; }

    public bool IsReady { get; private set; }

    public DiscordClientService(
        DiscordSocketClient client,
        IConfiguration configuration,
        ILogger<DiscordClientService> logger, IServiceProvider services)
    {
        _configuration = configuration;
        _logger = logger;
        this.services = services;

        Client = client;
        InteractionService = new InteractionService(Client);

        Client.Ready += Ready;
        Client.Log += Log;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        string token = File.ReadAllText(_configuration.GetValue<string>("TokenPath")!);
        if (string.IsNullOrEmpty(token))
            return;

        await Client.LoginAsync(TokenType.Bot, token);

        // add the public modules that inherit InteractionModuleBase<T> to the InteractionService
        await InteractionService.AddModulesAsync(Assembly.GetEntryAssembly(), services);

        // process the InteractionCreated payloads to execute Interactions commands
        Client.InteractionCreated += HandleInteraction;

        await Client.StartAsync();
    }

    private Task Log(LogMessage arg)
    {
        // За гранью понимания
        LogLevel logLevel = (LogLevel)(5 - (int)arg.Severity);

        _logger.Log(logLevel, arg.ToString(null, true, false));

        return Task.CompletedTask;
    }

    private async Task Ready()
    {
        await InteractionService.RegisterCommandsGloballyAsync(true);
        IsReady = true;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Client.Log -= Log;
        Client.Ready -= Ready;
        await Client.StopAsync();
    }

    public void Dispose()
    {
        Client?.Dispose();
    }

    private async Task HandleInteraction(SocketInteraction arg)
    {
        try
        {
            // create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
            var ctx = new SocketInteractionContext(Client, arg);
            await InteractionService.ExecuteCommandAsync(ctx, services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка!");
        }
    }
}