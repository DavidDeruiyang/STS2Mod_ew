using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class D12 : BombCard
    {
        protected override string PortraitFileName => "SL2 02 D12_zha_dan.png";
        public D12() : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, 3m, 20m)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            var target = cardPlay.Target;
            if (owner == null || target == null)
            {
                return;
            }

            await BombUtils.ApplyBomb(choiceContext, target, owner, this, BombTurns, BombDamage, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars[BombDamageKey].UpgradeValueBy(10m);
        }
    }
}
