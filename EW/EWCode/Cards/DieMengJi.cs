using BaseLib.Utils;
using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class DieMengJi() : EWCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL3 04 die_meng_ji.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.KazdelCard];


        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Target == null) return;
            await CommonActions.CardAttack(this, cardPlay.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);

            var selectableCards = CardPile.GetCards(Owner, [PileType.Hand])
                .Where(card => card != this)
                .ToList();

            if (selectableCards.Count == 0)
            {
                return;
            }

            var prefs = new CardSelectorPrefs(
                new LocString("cards", "EW-DIE_MENG_JI.prompt"),
                minCount: 1,
                maxCount: 1
            )
            {
                Cancelable = false,
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

        protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
