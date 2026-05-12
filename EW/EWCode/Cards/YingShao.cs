using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class YingShao() : EWCard(3, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "SL3 03 ying_shao.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("StrengthLoss", 20m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            foreach (var enemy in LivingEnemiesOf(Owner.Creature))
            {
                await CommonActions.Apply<TemporaryStrengthPower>(choiceContext, enemy, this, -DynamicVars["StrengthLoss"].BaseValue);
            }
        }

        protected override void OnUpgrade() => DynamicVars["StrengthLoss"].UpgradeValueBy(10m);
    }
}
