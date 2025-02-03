using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TestPens.Models.Real.Changes;
using TestPens.Models.Shared;
using TestPens.Models.Simple;

namespace TestPens.Models.Real
{
    public class GenericChangeDatabase
    {
        [Key]
        public ulong ID { get; set; }

        public ulong Chunk { get; set; }

        [Required]
        public ChangeType Type { get; set; }

        public DateTime UtcTime { get; set; }

        [Required]
        [Column(TypeName = "JSON")]
        public ExtraData Data { get; set; } = null!;

        public class ExtraData
        {
            public PersonModel? TargetPerson { get; set; }

            public PositionModel TargetPosition { get; set; } = null!;

            public PersonModel? NewPerson { get; set; }

            public PositionModel? NewPosition { get; set; }
        }
    }
}
