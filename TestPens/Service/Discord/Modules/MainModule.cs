using Discord;
using Discord.Interactions;

using TestPens.Models;
using TestPens.Models.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Service.Discord.Modules
{
    public class MainModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IPersonContainerService personContainer;

        public MainModule(IPersonContainerService personContainer)
        {
            this.personContainer = personContainer;
        }

        [SlashCommand("changeposition", "Изменяет позицию в тирлисте человека!")]
        public async Task ChangePosition([Summary("Тир", "Тир человека которого нужно переместить")] Tier tier)
        {
            TierListState head = personContainer.GetHead();

            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("Выберите человека:")
                .WithCustomId($"spsb_{nameof(ChangePosition)}")
                .WithMinValues(1)
                .WithMaxValues(1);

            int it = 0;
            foreach (PersonModel person in head.TierList[tier])
            {
                menuBuilder.AddOption(person.Nickname, $"{tier}_{it}");
                it++;
            }

            var builder = new ComponentBuilder()
                .WithSelectMenu(menuBuilder);

            await RespondAsync("Тир найден, выберите человека:", ephemeral: true, components: builder.Build());
        }

        [ComponentInteraction($"spsb_{nameof(ChangePosition)}")]
        public async Task PersonSelection(string id)
        {
            TierListState head = personContainer.GetHead();

            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("Выберите тир:")
                .WithCustomId($"spsb_{nameof(PersonSelection)}")
                .WithMinValues(1)
                .WithMaxValues(1);

            int it = 0;
            foreach (Tier tier in Enum.GetValues(typeof(Tier)))
            {
                menuBuilder.AddOption(tier.GetName(), $"{id}_{tier}");
                it++;
            }

            var builder = new ComponentBuilder()
                .WithSelectMenu(menuBuilder);

            await RespondAsync("Человек найден, выберите тир в который нужно его переместить", ephemeral: true, components: builder.Build());
        }

        [ComponentInteraction($"spsb_{nameof(PersonSelection)}")]
        public async Task TierSelection(string id)
        {
            Tier tier = Enum.Parse<Tier>(id.Split('_').Last());

            TierListState head = personContainer.GetHead();

            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("Выберите человека, на место которого встанет выбраный вами:")
                .WithCustomId($"spsb_{nameof(TierSelection)}")
                .WithMinValues(1)
                .WithMaxValues(1);

            int it = 0;
            foreach (PersonModel person in head.TierList[tier])
            {
                menuBuilder.AddOption(person.Nickname, $"{id}_{it}");
                it++;
            }

            menuBuilder.AddOption("Конец тира", $"{id}_{it}");

            var builder = new ComponentBuilder()
                .WithSelectMenu(menuBuilder);

            await RespondAsync("Выберите человека, на место которого встанет выбраный вами:", ephemeral: true, components: builder.Build());
        }

        [ComponentInteraction($"spsb_{nameof(TierSelection)}")]
        public async Task FinalHandle(string id)
        {
            string[] data = id.Split("_");

            ShortPositionModule oldPosition = new ShortPositionModule
            {
                Tier = Enum.Parse<Tier>(data[0]),
                TierPosition = int.Parse(data[1]),
            };

            ShortPositionModule newPosition = new ShortPositionModule
            {
                Tier = Enum.Parse<Tier>(data[2]),
                TierPosition = int.Parse(data[3]),
            };

            PositionChange change = new(newPosition, oldPosition);

            personContainer.AddChange(change);

            await RespondAsync("Успешно!", ephemeral: true);
        }
    }
}
