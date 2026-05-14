using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ShiDaiDeYanLei() : EWCard(3, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 06 shi_dai_de_yan_lei.png";
        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(10, ValueProp.Move),
            new DynamicVar("StrengthGain", 5m),
            new DynamicVar("EnergyGain", 3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            var combatState = owner?.CombatState;
            if (Owner == null || owner == null || combatState == null) return;

            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this, -99m);

            var teammates = combatState.Players
                .Where(player => player != Owner && player.Creature.Side == owner.Side && player.Creature.IsAlive)
                .ToList();

            foreach (var teammate in teammates)
            {
                await PowerCmd.Apply<StrengthPower>(teammate.Creature, DynamicVars["StrengthGain"].BaseValue, owner, this);
                await PlayerCmd.GainEnergy(DynamicVars["EnergyGain"].BaseValue, teammate);
                await CreatureCmd.GainBlock(teammate.Creature, DynamicVars.Block, cardPlay, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["StrengthGain"].UpgradeValueBy(2m);
        }
    }
}
