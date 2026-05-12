using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class BaoZhaTianCai() : EWCard(2, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "PL2 01 bao_zha_tian_cai.png";

        private const string GeneratedCardKey = "GeneratedCard";

        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => SingleCardPreview<D6Bomb>(IsUpgraded);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new StringVar(GeneratedCardKey, "D6炸弹")
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null)
            {
                return;
            }

            var power = await PowerCmd.Apply<EWExplosionGeniusPower>(owner, 1m, owner, this);
            power?.SetBombUpgraded(IsUpgraded);
        }

        protected override void OnUpgrade()
        {
            ((StringVar)DynamicVars[GeneratedCardKey]).StringValue = "D6+炸弹";
        }
    }
}
