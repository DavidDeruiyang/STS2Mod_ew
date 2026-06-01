using BaseLib.Utils;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZuZongLeiJi() : EWCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        private const decimal BaseDamage = 4m;
        private const string DamagePerAttackKey = "DamagePerAttack";

        protected override string PortraitFileName => "AL4 03 zu_zong_lei_ji.png";

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(BaseDamage, ValueProp.Move),
            new DynamicVar(DamagePerAttackKey, 1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Target == null) return;

            RefreshDamage();
            await CommonActions.CardAttack(this, cardPlay.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);
            RefreshDamage();
        }

        protected override void OnUpgrade()
        {
            DynamicVars[DamagePerAttackKey].UpgradeValueBy(1m);
        }

        private void RefreshDamage()
        {
            DynamicVars.Damage.BaseValue = BaseDamage + SummonManager.GetTotalHLZYAttackCount(Owner?.Creature) * DynamicVars[DamagePerAttackKey].BaseValue;
        }

        public static void RefreshDamageForPlayer(Player player)
        {
            var pileTypes = new[] { PileType.Hand, PileType.Draw, PileType.Discard };
            foreach (var card in CardPile.GetCards(player, pileTypes).OfType<ZuZongLeiJi>())
            {
                card.RefreshDamage();
            }
        }
    }
}
