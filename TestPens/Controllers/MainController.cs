using System.Diagnostics;
using System.IO.Pipes;

using Microsoft.AspNetCore.Mvc;

using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly IPersonContainerService containerService;

        private Dictionary<Tier, List<PersonModel>> _persons = new(5)
        {
            {
                Tier.SPlus,
                new() {
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,

                        Tier = Tier.SPlus,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

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

                        Tier = Tier.S,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

                        Tier = Tier.S,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

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

                        Tier = Tier.A,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

                        Tier = Tier.A,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = false,

                        Tier = Tier.A,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

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

                        Tier = Tier.B,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

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

                        Tier = Tier.C,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

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

                        Tier = Tier.D,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

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

                        Tier = Tier.E,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    },
                    new PersonModel
                    {
                        Nickname = "Art",
                        InDrop = true,

                        Tier = Tier.E,
                        VideoLink = "https://www.youtube.com/embed/TgJ6a1gT7oM",
                    }
                }
            },
        };

        public MainController(ILogger<MainController> logger, IPersonContainerService containerService)
        {
            _logger = logger;
            this.containerService = containerService;
        }

        public IActionResult Index()
        {
            return View(containerService.GetHead().TierList);
        }
    }
}
