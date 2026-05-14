using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class WoDuBuDongYanJiangGao() : EWCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 07 yan_jiang.png";

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            CardPreview<ShengYuHeiYe>(),
            CardPreview<GeiWoWanMingRiFangZhou>(),
            CardPreview<SiHunLingDeYuXi>()
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            await ChooseCardFromEWPoolToHand(choiceContext, CardType.Power, IsUpgraded);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
