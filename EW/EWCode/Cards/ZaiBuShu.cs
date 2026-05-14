using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZaiBuShu() : EWCard(3, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 05 zai_bu_shu.png";

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var player = Owner;
            if (player == null) return;

            foreach (var power in player.Creature.Powers.ToList())
            {
                await PowerCmd.Remove(power);
            }

            var combatState = CombatManager.Instance.DebugOnlyGetState();
            var combat = player.PlayerCombatState;
            if (combatState == null || combat == null) return;

            var hand = CardPile.GetCards(player, [PileType.Hand]).ToList();
            if (hand.Count > 0)
            {
                await CardCmd.Discard(choiceContext, hand);
            }

            combat.ResetEnergy();
            await Hook.AfterEnergyReset(combatState, player);
            await Hook.BeforeHandDraw(combatState, player, choiceContext);
            await CardPileCmd.Draw(choiceContext, 5m, player, true);
            await Hook.AfterPlayerTurnStart(combatState, choiceContext, player);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}
