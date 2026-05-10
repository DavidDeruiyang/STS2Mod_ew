using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZhiShiXueBao() : EWCard(0, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 03 zhi_shi_xue_bao.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            await CommonActions.Draw(this, choiceContext);
            await CommonActions.ApplySelf<WeakPower>(choiceContext, this, 2m);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1m);
        }
    }
}
