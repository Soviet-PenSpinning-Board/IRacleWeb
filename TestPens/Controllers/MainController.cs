using System.Diagnostics;
using System.IO.Pipes;

using Microsoft.AspNetCore.Mvc;

using TestPens.Models.Simple;

namespace TestPens.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;

        private Dictionary<Tier, List<PersonModel>> _persons = new(5)
        {
            {
                Tier.SPlus,
                new() {
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,
                        TierPosition = 1,
                        Tier = Tier.SPlus,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.SPlus,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    }
                }
            },
            {
                Tier.S,
                new() {
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,
                        TierPosition = 1,
                        Tier = Tier.S,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.S,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.S,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    }
                }
            },
            {
                Tier.A,
                new() {
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,
                        TierPosition = 1,
                        Tier = Tier.A,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.A,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,
                        TierPosition = 1,
                        Tier = Tier.A,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.A,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    }
                }
            },
            {
                Tier.B,
                new() {
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,
                        TierPosition = 1,
                        Tier = Tier.B,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.B,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    }
                }
            },
            {
                Tier.C,
                new() {
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,
                        TierPosition = 1,
                        Tier = Tier.C,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.C,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    }
                }
            },
            {
                Tier.D,
                new() {
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,
                        TierPosition = 1,
                        Tier = Tier.D,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.D,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    }
                }
            },
            {
                Tier.E,
                new() {
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,
                        TierPosition = 1,
                        Tier = Tier.E,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,
                        TierPosition = 1,
                        Tier = Tier.E,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    }
                }
            },
        };

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_persons);
        }
    }
}
