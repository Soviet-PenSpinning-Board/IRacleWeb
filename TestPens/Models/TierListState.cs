using System.Collections.Generic;

using TestPens.Models.Abstractions;
using TestPens.Models.Simple;

namespace TestPens.Models
{
    public class TierListState
    {
        private TierListState? nextState;
        private BaseChange? nextChange;
        private Dictionary<Tier, List<PersonModel>>? calculatedTierList;

        public Dictionary<Tier, List<PersonModel>> TierList
        {
            get
            {
                if (calculatedTierList != null)
                    return calculatedTierList!;

                TierListState head = this;

                List<TierListState> states = new(100);

                while (!head.IsHead)
                {
                    states.Add(head);
                    head = head.nextState!;
                }

                calculatedTierList = new Dictionary<Tier, List<PersonModel>>(head.TierList);

                for (int i = states.Count; i >= 0; i--)
                {
                    states[i].nextChange!.Revert(calculatedTierList);
                }

                return calculatedTierList;
            }
        }

        public Guid Signature { get; }

        public bool IsHead => nextState == null;

        public TierListState(Dictionary<Tier, List<PersonModel>> tierList, Guid guid)
        {
            Signature = guid;
            calculatedTierList = tierList;
        }

        public TierListState(TierListState nextTierList, BaseChange nextChange)
        {
            Signature = nextChange.Signature;
            this.nextChange = nextChange;
            nextState = nextTierList;
        }

        public TierListState MakeChange(BaseChange change)
        {
            Dictionary<Tier, List<PersonModel>>? newDict = new(TierList);
            change.Apply(newDict);
            TierListState next = new(newDict, change.Signature);

            nextChange = change;
            nextState = next;

            return next;
        }

        public TierListState Revert(BaseChange change)
        {
            if (change.Signature != Signature)
                throw new InvalidOperationException("Сигнатура изменения не соответствует сигнатуре данных!");

            TierListState prev = new(this, change);

            return prev;
        }
    }
}
