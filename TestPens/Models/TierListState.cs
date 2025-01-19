using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;

namespace TestPens.Models
{
    public class TierListState
    {
        private TierListState? headState;
        private IEnumerable<BaseChange>? deltaHeadChanges;
        private Dictionary<Tier, List<PersonModel>>? calculatedTierList;

        public IReadOnlyDictionary<Tier, List<PersonModel>> TierList
        {
            get
            {
                if (calculatedTierList != null)
                    return calculatedTierList!;

                TierListState head = headState!;

                calculatedTierList = new(head.TierList);

                foreach (BaseChange change in deltaHeadChanges!)
                {
                    change.Initialize(calculatedTierList);
                    if (change.IsAffective())
                        change.Apply(calculatedTierList);
                }

                // с этого момента объект может забывать про родителя и изменения, и становится полноценным для нормальной отчистки, мб не надо
                headState = null;
                deltaHeadChanges = null;

                return calculatedTierList;
            }
        }

        public bool IsHead => headState == null || deltaHeadChanges == null;

        public TierListState(Dictionary<Tier, List<PersonModel>> tierList)
        {
            calculatedTierList = tierList;
        }

        public TierListState(TierListState head, IEnumerable<BaseChange> deltaHeadChanges)
        {
            headState = head;
            this.deltaHeadChanges = deltaHeadChanges;
        }

        public bool TryGetMember(ShortPositionModel position, [NotNullWhen(true)] out PersonModel? person)
        {
            person = null;
            if (!TierList.TryGetValue(position.Tier, out var list) || position.TierPosition >= list.Count)
            {
                person = null;
                return false;
            }

            person = list[position.TierPosition];
            return true;
        }

        public PersonModel? GetMember(ShortPositionModel position)
        {
            TryGetMember(position, out PersonModel? result);

            return result;
        }

        public TierListState ApplyChange(BaseChange change)
        {
            return ApplyChanges([change]);
        }

        public TierListState ApplyChanges(IEnumerable<BaseChange> changes)
        {
            return new TierListState(this, changes);
        }

        public TierListState RevertChanges(IEnumerable<BaseChange> changes)
        {
            return ApplyChanges(changes.Select(change => change.RevertedChange()));
        }
    }
}
