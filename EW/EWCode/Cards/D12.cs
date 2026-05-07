using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class D12() : EWCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        private const string TurnsKey = "Turns";
        private const string BombDamageKey = "BombDamage";

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(TurnsKey, 3m),
            new DynamicVar(BombDamageKey, 20m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null)
            {
                return;
            }

            (await PowerCmd.Apply<TheBombPower>(
                owner,
                DynamicVars[TurnsKey].BaseValue,
                owner,
                this
            )).SetDamage(DynamicVars[BombDamageKey].BaseValue);
        }

        protected override void OnUpgrade()
        {
            DynamicVars[BombDamageKey].UpgradeValueBy(10m);
        }
    }
}