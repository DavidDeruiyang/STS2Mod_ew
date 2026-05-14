using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class BasicAttack() : EWCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL1 00 gong_ji.png";
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(7, ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Target == null) return;

            await CommonActions.CardAttack(
                this,
                cardPlay.Target,
                vfx: "vfx/vfx_attack_slash"
            ).Execute(choiceContext);

            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
