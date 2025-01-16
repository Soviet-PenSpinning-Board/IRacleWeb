using System.Runtime.CompilerServices;

using Discord.WebSocket;

using TestPens.Service.Discord;

namespace TestPens.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDiscordRelated(this IServiceCollection services, DiscordSocketConfig config)
        {
            return services
                .AddSingleton(new DiscordSocketClient(config))
                .AddSingleton<DiscordClientService>()
                .AddHostedService(p => p.GetRequiredService<DiscordClientService>());
        }
    }
}
