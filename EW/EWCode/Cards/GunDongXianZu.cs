using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class GunDongXianZu() : EWCard(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "SL4 04 gun_dong_xian_zu.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue * SummonManager.CountHLZY(), MegaCrit.Sts2.Core.ValueProps.ValueProp.Move, cardPlay, false);
            }
        }

        protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2m);
    }
}
