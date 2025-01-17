using System.Text.Json;

using TestPens.Service.Abstractions;

namespace TestPens.Service
{
    public class JsonTokenManager : ITokenManager
    {
        private ILogger<JsonTokenManager> _logger;

        public Dictionary<string, Permissions> Tokens;

        public JsonTokenManager(ILogger<JsonTokenManager> logger, IConfiguration configuration)
        {
            _logger = logger;

            string path = configuration.GetValue<string>("TokensPath")!;
            if (string.IsNullOrWhiteSpace(path))
            {
                _logger.LogError("Конфигурация пути файла токенов не задана! ('TokensPath')");
                Tokens = new();
                return;
            }

            if (!File.Exists(path))
            {
                Tokens = new(3)
                {
                    { "sigmaBoy", Permissions.Battles },
                    { "чтовмешочек", Permissions.MembersEdit },
                    { "p", Permissions.EditAll },
                    { "admin", Permissions.All },
                };
                File.WriteAllText(path, JsonSerializer.Serialize(Tokens, Program.JsonOptions));
                _logger.LogError("Файл с токенами не найден, генерация пустого файла...");
                return;
            }

            string content = File.ReadAllText(path);

            try
            {
                Tokens = JsonSerializer.Deserialize<Dictionary<string, Permissions>>(content, Program.JsonOptions)!;

            }
            catch (Exception ex)
            {
                Tokens = new();
                _logger.LogError(ex, "Ошибка десериализации токенов: ");
            }

            if (Tokens == null)
            {
                _logger.LogError("Ошибка, словарь с токенами не заполнен");
                Tokens = new();
                return;
            }

            _logger.LogInformation("Токены успешно загружены!");
        }

        public Permissions CheckToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || !Tokens.TryGetValue(token, out Permissions permissions))
            {
                return Permissions.None;
            }

            return permissions;
        }
    }
}
