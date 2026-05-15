using EW.EWCode.Keywords;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class BaoLieLiMing() : EWCard(2, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        private const string GeneratedCardKey = "GeneratedCard";

        protected override string PortraitFileName => "SL4 01 bao_lie_li_ming.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.SoulShadow];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => SingleCardPreview<BaoLieLiMingDanYao>(IsUpgraded);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new StringVar(GeneratedCardKey, "爆裂黎明弹药")
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            await AddCardsToHand<BaoLieLiMingDanYao>(Owner, 6, IsUpgraded);
            _ = SummonManager.SummonHLZYWhenReady(SummonSource.Card, summoner: Owner.Creature, cardSource: this);
            _ = SummonManager.SummonHLZYWhenReady(SummonSource.Card, summoner: Owner.Creature, cardSource: this);
        }

        protected override void OnUpgrade()
        {
            ((StringVar)DynamicVars[GeneratedCardKey]).StringValue = "爆裂黎明弹药+";
        }
    }
}
