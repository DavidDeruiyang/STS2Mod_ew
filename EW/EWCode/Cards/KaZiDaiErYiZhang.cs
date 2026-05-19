using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class KaZiDaiErYiZhang() : EWCard(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        private const string UpgradeTextKey = "UpgradeText";

        protected override string PortraitFileName => "PL3 01 ka_zi_dai_er_yi_zhang.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.KazdelCard];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(UpgradeTextKey, "。")];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await PowerCmd.Apply<EWKazdelSpeakerPower>(Owner.Creature, IsUpgraded ? 2m : 1m, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            ((StringVar)DynamicVars[UpgradeTextKey]).StringValue = "，该牌在当前回合减1费。";
        }
    }
}
