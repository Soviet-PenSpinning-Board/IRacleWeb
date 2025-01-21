
using System.Text.Json.Serialization;

using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Abstractions
{
    public abstract class BaseChange
    {
        public DateTime UtcTime { get; set; }

        public abstract ChangeType Type { get; set; }

        public PersonModel? TargetPerson { get; set; }

        public ShortPositionModel TargetPosition { get; set; } = null!;

        public virtual bool Validate(Dictionary<Tier, List<PersonModel>> head, out string reason)
        {
            if (!Enum.IsDefined(TargetPosition.Tier))
            {
                reason = $"TargetPosition.Tier указан неправильно! ({TargetPosition.Tier})";
                return false;
            }

            if (!head.TryGetValue(TargetPosition.Tier, out var list))
            {
                reason = $"TargetPosition.Tier указан неправильно, в теории это сообщение невозможно? ({TargetPosition.Tier})";
                return false;
            }

            if (list.Count <= TargetPosition.TierPosition)
            {
                reason = $"TargetPosition.TierPosition указан неправильно! ({TargetPosition.TierPosition}/{list.Count})";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public virtual void Initialize(Dictionary<Tier, List<PersonModel>> head)
        {
            if (TargetPerson != null)
                return;

            PersonModel currentPerson = head[TargetPosition.Tier][TargetPosition.TierPosition];
            TargetPerson = new PersonModel
            {
                Guid = currentPerson.Guid,
                Nickname = currentPerson.Nickname,
                AvatarUrl = currentPerson.AvatarUrl,
                InDrop = currentPerson.InDrop,
                VideoLink = currentPerson.VideoLink,
            };
        }

        public virtual bool IsAffective() => true;

        protected BaseChange(DateTime dateTime)
        {
            UtcTime = dateTime;
        }

        public abstract Permissions GetPermission();

        public abstract void Apply(Dictionary<Tier, List<PersonModel>> tierListState);

        public abstract BaseChange RevertedChange();
    }

    public enum ChangeType
    {
        None,
        ChangePosition,
        GlobalPerson,
        PersonProperties
    }
}
