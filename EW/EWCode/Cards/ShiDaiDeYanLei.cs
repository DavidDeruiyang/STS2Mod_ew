using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ShiDaiDeYanLei() : EWCard(3, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(10, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move), new DynamicVar("StrengthGain", 5m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this, -99m);
            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this, DynamicVars["StrengthGain"].BaseValue);
            await PlayerCmd.GainEnergy(3m, Owner);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["StrengthGain"].UpgradeValueBy(2m);
        }
    }
}
