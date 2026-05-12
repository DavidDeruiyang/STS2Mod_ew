using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class BaoLieLiMingDanYao() : EWCard(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL4 05 bao_lie_li_ming_dan_yao.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Target == null) return;
            await CommonActions.CardAttack(this, cardPlay.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);
        }

        protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
