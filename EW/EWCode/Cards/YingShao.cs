using BaseLib.Utils;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class YingShao() : EWCard(3, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "SL3 03 ying_shao.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("StrengthLoss", 20m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null) return;

            var targets = LivingEnemiesOf(owner).ToList();
            foreach (var enemy in targets)
            {
                if (!enemy.IsAlive)
                {
                    continue;
                }

                var amount = DynamicVars["StrengthLoss"].BaseValue;
                await PowerCmd.Apply<StrengthPower>(enemy, -amount, owner, this);
                // await PowerCmd.Apply<PiercingWailPower>(enemy, amount, owner, this);
            }
        }

        protected override void OnUpgrade() => DynamicVars["StrengthLoss"].UpgradeValueBy(10m);
    }
}
