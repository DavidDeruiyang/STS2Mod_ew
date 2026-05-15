using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ShuangChongDaJi() : EWCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL4 06 shuang_chong_da_ji.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Target == null) return;
            await CommonActions.CardAttack(this, cardPlay.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);
            await ChooseCardFromEWPoolToHand(choiceContext, CardType.Attack, IsUpgraded);
        }

        protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
