using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class JinGongYuWang() : EWCard(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        private const string GeneratedCardKey = "GeneratedCard";

        protected override string PortraitFileName => "SL4 02 jin_gong_yu_wang.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => SingleCardPreview<QiangLiJi>(IsUpgraded);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new StringVar(GeneratedCardKey, "强力击")
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await AddCardsToHand<QiangLiJi>(Owner, 2, IsUpgraded);
            }
        }

        protected override void OnUpgrade()
        {
            ((StringVar)DynamicVars[GeneratedCardKey]).StringValue = "强力击+";
        }
    }
}
