﻿using TestPens.Models.Dto;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
using TestPens.Models.Simple;
using TestPens.Service.Abstractions;

namespace TestPens.Models.Real.Changes
{
    public class NoneChangeModel : ChangeBaseModel
    {
        public override ChangeType Type { get; set; } = ChangeType.None;

        public override void Apply(TierListState state, bool revert) { }

        public override ChangeBaseDto ToForm()
        {
            return new NoneChangeDto
            {
                UtcTime = UtcTime,
                TargetPerson = TargetPerson.ToForm(),
                TargetPosition = TargetPosition.ToForm(),
            };
        }
    }
}
