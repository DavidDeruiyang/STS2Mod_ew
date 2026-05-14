using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class WeiWeiMei() : EWCard(2, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 04 wei_wei_mei.png";

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this, 5m);
            foreach (var enemy in LivingEnemiesOf(Owner.Creature))
            {
                await CommonActions.Apply<StrengthPower>(choiceContext, enemy, this, 5m);
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }
    }
}
