using EW.EWCode.Keywords;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class BaoLieLiMing() : EWCard(2, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.SoulShadow];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => SingleCardPreview<BaoLieLiMingDanYao>(IsUpgraded);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            await AddCardsToHand<BaoLieLiMingDanYao>(Owner, 6, IsUpgraded);
            _ = SummonManager.SummonHLZYWhenReady(SummonSource.Card, summoner: Owner.Creature, cardSource: this);
            _ = SummonManager.SummonHLZYWhenReady(SummonSource.Card, summoner: Owner.Creature, cardSource: this);
        }
    }
}
