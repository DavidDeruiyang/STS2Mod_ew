using BaseLib.Utils;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ShengYuHeiYe() : EWCard(1, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "PL1 01 sheng_yu_hei_ye.png";
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || !HasPower<EWCamouflagePower>(Owner.Creature)) return;

            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this, 1m);
            await CommonActions.ApplySelf<DexterityPower>(choiceContext, this, IsUpgraded ? 2m : 1m);
        }
    }
}
