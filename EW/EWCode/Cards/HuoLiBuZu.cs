using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class HuoLiBuZu() : EWCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "SL4 05 huo_li_bu_zu.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => SingleCardPreview<QiangLiJi>(IsUpgraded);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await AddCardsToDrawPile<QiangLiJi>(Owner, 3, IsUpgraded);
            }
        }
    }
}
