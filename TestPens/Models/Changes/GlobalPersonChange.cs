
using System;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Changes
{
    public class GlobalPersonChange : BaseChange
    {
        public GlobalPersonChange() :
            base(DateTime.UtcNow)
        {
        }

        public bool IsNew { get; set; }

        public override ChangeType Type { get; set; } = ChangeType.GlobalPerson;

        public override Permissions GetPermission() =>
             Permissions.GlobalMember;

        public override void Initialize(Dictionary<Tier, List<PersonModel>> head)
        {
            if (IsNew)
                return;

            base.Initialize(head);
        }

        public override void Apply(Dictionary<Tier, List<PersonModel>> tierListState)
        {
            List<PersonModel> tier = tierListState[TargetPosition.Tier];

            if (IsNew)
                tier.Insert(TargetPosition.TierPosition, TargetPerson!.Copy());
            else
                tier.RemoveAt(TargetPosition.TierPosition);
        }

        public override BaseChange RevertedChange()
        {
            return new GlobalPersonChange
            {
                UtcTime = UtcTime,
                TargetPosition = TargetPosition,
                TargetPerson = TargetPerson,
                IsNew = !IsNew,
            };
        }
    }
}
