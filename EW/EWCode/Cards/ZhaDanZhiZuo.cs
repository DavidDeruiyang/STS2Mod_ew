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
    public class ZhaDanZhiZuo() : EWCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "SL2 07 zha_dan_zhi_zuo.png";
        private const string GeneratedCardKey = "GeneratedCard";

        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => SingleCardPreview<D12>(IsUpgraded);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new StringVar(GeneratedCardKey, "D12炸弹")
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null)
            {
                return;
            }

            await BombUtils.AddBombCardToHand<D12>(Owner, IsUpgraded, 4);
        }

        protected override void OnUpgrade()
        {
            ((StringVar)DynamicVars[GeneratedCardKey]).StringValue = "D12+炸弹";
        }
    }
}
