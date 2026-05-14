using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class FangYuXingBaoZha() : BombCard(1, CardType.Skill, CardRarity.Common, TargetType.None, 2m, 10m)
    {
        protected override string PortraitFileName => "SL2 06 fang_yu_xing_bao_zha.png";

        private const string BlockKey = "Block";

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(16, ValueProp.Move),
            new DynamicVar(TurnsKey, 2m),
            new DynamicVar(BombDamageKey, 10m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null)
            {
                return;
            }

            await CreatureCmd.GainBlock(owner, DynamicVars.Block, cardPlay, false);
            await BombUtils.ApplyBomb(choiceContext, owner, owner, this, BombTurns, BombDamage, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars[TurnsKey].UpgradeValueBy(1m);
        }
    }
}
