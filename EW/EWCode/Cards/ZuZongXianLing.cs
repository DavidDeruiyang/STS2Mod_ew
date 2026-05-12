using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZuZongXianLing() : EWCard(3, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "SL4 06 zu_zong_xian_ling.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            CardPreview<SiHunLingDeYuXi>(),
            CardPreview<XianZuDeBiHu>(),
            CardPreview<HongMingZhiShou>()
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            await AddCardsToHand<SiHunLingDeYuXi>(Owner, 1, IsUpgraded, -99);
            await AddCardsToHand<XianZuDeBiHu>(Owner, 1, IsUpgraded, -99);
        }
    }
}
