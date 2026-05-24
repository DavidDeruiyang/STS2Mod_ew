using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class SuiJiTouZhi() : BombCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.None, 3m, 20m)
    {
        protected override string PortraitFileName => "SL2 08 sui_ji_tou_zhi.png";
        private const string GeneratedCardKey = "GeneratedCard";

        protected override bool HasEnergyCostX => true;

        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => SingleCardPreview<D12>(IsUpgraded);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(TurnsKey, 3m),
            new DynamicVar(BombDamageKey, 20m),
            new StringVar(GeneratedCardKey, "D12炸弹")
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null)
            {
                return;
            }

            var count = GetResolvedEnergyXValue(cardPlay);

            for (var i = 0; i < count; i++)
            {
                var target = BombUtils.RandomLivingEnemyOf(owner);
                if (target == null)
                {
                    return;
                }

                await BombUtils.ApplyBomb(choiceContext, target, owner, this, BombTurns, BombDamage, cardPlay);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars[BombDamageKey].UpgradeValueBy(10m);
            ((StringVar)DynamicVars[GeneratedCardKey]).StringValue = "D12+炸弹";
        }
    }
}
