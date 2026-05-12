using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class BaoFanZhuangJia() : EWCard(3, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "PL2 04 bao_fan_zhuang_jia.png";
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(EWBombCounterArmorPower.BlockKey, 3m)
        ];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null)
            {
                return;
            }

            var power = await PowerCmd.Apply<EWBombCounterArmorPower>(owner, 1m, owner, this);
            if (power != null)
            {
                power.DynamicVars[EWBombCounterArmorPower.BlockKey].BaseValue =
                    DynamicVars[EWBombCounterArmorPower.BlockKey].BaseValue;
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars[EWBombCounterArmorPower.BlockKey].UpgradeValueBy(1m);
        }
    }
}
