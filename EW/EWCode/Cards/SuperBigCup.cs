using BaseLib.Utils;
using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class SuperBigCup() : EWCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL1 05 super_big_cup.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.SoulShadow, EWKeywords.Camouflage, CardKeyword.Exhaust];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("Strength", 1m),
            new DynamicVar("Energy", 1m),
            new BlockVar(1, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new CardsVar(1),
            new HealVar(1),
            new DynamicVar("Debuff", 1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            var owner = Owner.Creature;
            var debuff = DynamicVars["Debuff"].BaseValue;

            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this, DynamicVars["Strength"].BaseValue);
            await PlayerCmd.GainEnergy(DynamicVars["Energy"].BaseValue, Owner);
            await CreatureCmd.GainBlock(owner, DynamicVars.Block, cardPlay, false);
            await CommonActions.Draw(this, choiceContext);
            await CreatureCmd.Heal(owner, DynamicVars.Heal.BaseValue, false);
            _ = SummonManager.SummonHLZYWhenReady(SummonSource.Card, summoner: owner, cardSource: this);
            await PowerCmd.Apply<EWCamouflagePower>(owner, 1m, owner, this);

            if (cardPlay.Target != null)
            {
                await CommonActions.Apply<WeakPower>(choiceContext, cardPlay.Target, this, debuff);
                await CommonActions.Apply<VulnerablePower>(choiceContext, cardPlay.Target, this, debuff);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Strength"].UpgradeValueBy(1m);
            DynamicVars["Energy"].UpgradeValueBy(1m);
            DynamicVars.Block.UpgradeValueBy(1m);
            DynamicVars.Cards.UpgradeValueBy(1m);
            DynamicVars.Heal.UpgradeValueBy(1m);
            DynamicVars["Debuff"].UpgradeValueBy(1m);
        }
    }
}
