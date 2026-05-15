using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class BaoZhaXiaoHui() : EWCard(0, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        private const int ExhaustCount = 2;

        protected override string PortraitFileName => "SL2 09 bao_zha_xiao_hui.png";

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null)
            {
                return;
            }

            var selectableCards = CardPile.GetCards(Owner, [PileType.Hand])
                .Where(card => card != this)
                .ToList();

            if (selectableCards.Count == 0)
            {
                return;
            }

            var prefs = new CardSelectorPrefs(
                new LocString("cards", "EW-BAO_ZHA_XIAO_HUI.prompt"),
                minCount: 0,
                maxCount: int.Min(ExhaustCount, selectableCards.Count)
            )
            {
                Cancelable = true,
                RequireManualConfirmation = true
            };

            var selectedCards = await CardSelectCmd.FromHand(
                choiceContext,
                Owner,
                prefs,
                card => card != this,
                this
            );

            foreach (var selectedCard in selectedCards)
            {
                await CardCmd.Exhaust(choiceContext, selectedCard, true, false);
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}
