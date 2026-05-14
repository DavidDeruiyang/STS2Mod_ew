using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class TouZhiShou() : EWCard(1, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        private const string SplashPercentKey = "SplashPercent";

        protected override string PortraitFileName => "PL4 02 tou_zhi_shou.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(SplashPercentKey, 50m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWHLZYSplashPower>(Owner.Creature, DynamicVars[SplashPercentKey].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade() => DynamicVars[SplashPercentKey].UpgradeValueBy(10m);
    }
}
