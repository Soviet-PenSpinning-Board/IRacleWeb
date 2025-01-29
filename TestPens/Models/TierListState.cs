using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;

namespace TestPens.Models
{
    public class TierListState
    {
        public List<ChangeBaseModel>? cachedChanges;
        public IReadOnlyDictionary<Tier, List<PersonModel>> TierList { get; }

        public TierListState(Dictionary<Tier, List<PersonModel>> tierList)
        {
            TierList = tierList;
        }

        public TierListState(TierListState head, IReadOnlyCollection<ChangeBaseDto> changes, bool revert)
        {
            TierList = head.ShadowCopy();

            cachedChanges = new(changes.Count);

            foreach (ChangeBaseDto change in changes)
            {
                if (!change.Validate(this, out string reason))
                {
                    throw new InvalidOperationException(reason);
                }

                var changeModel = change.CreateFrom(this);
                cachedChanges.Add(changeModel);

                if (changeModel.IsAffective())
                {
                    changeModel.Apply(this, revert);
                }
            }
        }

        public TierListState ApplyChange(ChangeBaseDto change)
        {
            return ApplyChanges([change]);
        }

        public TierListState ApplyChanges(IReadOnlyCollection<ChangeBaseDto> changes, bool revert = false)
        {
            return new TierListState(this, changes, revert);
        }

        public TierListState ApplyChanges(IReadOnlyCollection<ChangeBaseModel> changes, bool revert = false)
        {
            var @new = new TierListState(ShadowCopy());
            foreach (ChangeBaseModel change in changes)
            {
                if (change.IsAffective())
                {
                    change.Apply(@new, revert);
                }
            }

            return @new;
        }

        private Dictionary<Tier, List<PersonModel>> ShadowCopy()
        {
            Dictionary<Tier, List<PersonModel>> result = new(TierList.Count);
            foreach (var item in TierList)
            {
                result.Add(item.Key, new(TierList[item.Key]));
            }

            return result;
        }
    }
}
