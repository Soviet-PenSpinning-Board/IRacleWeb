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

        public bool IsHead => nextState == null;

        public TierListState(Dictionary<Tier, List<PersonModel>> tierList)
        {
            calculatedTierList = tierList;
        }

        public TierListState(TierListState nextTierList, BaseChange nextChange)
        {
            this.nextChange = nextChange;
            nextState = nextTierList;
        }

        public TierListState MakeChangeNextNode(BaseChange change)
        {
            Dictionary<Tier, List<PersonModel>>? newDict = new(TierList);
            change.Apply(newDict);
            TierListState next = new(newDict);

            nextChange = change;
            nextState = next;

            return next;
        }

        public void MakeChange(BaseChange change)
        {
            change.Apply(TierList);
        }

        public TierListState Revert(BaseChange change)
        {
            TierListState prev = new(this, change);

            return prev;
        }
    }
}
