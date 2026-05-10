using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class DianXiaDeZhuFu() : EWCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CommonActions.Draw(this, choiceContext);
        }

        protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(2m);
    }
}
