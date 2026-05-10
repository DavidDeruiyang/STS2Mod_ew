using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZuZongLeiJi() : EWCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL4 03 zu_zong_lei_ji.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Target == null) return;
            await CommonActions.CardAttack(this, cardPlay.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
            var before = DynamicVars.Damage.BaseValue;
            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);
            var gained = EW.EWCode.Summons.SummonManager.CountHLZY();
            if (gained > 0)
            {
                DynamicVars.Damage.BaseValue = before + gained;
            }
        }

        protected override void OnUpgrade()
        {
        }
    }
}
