using BaseLib.Patches.Localization;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Powers
{
    public class EWTimedBombPower : EWPower, IAddDumbVariablesToPowerDescription
    {
        private const string BombDamageKey = "BombDamage";
        private const string CountdownKey = "Countdown";

        private decimal _damage;
        private CardModel? _cardSource;

        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => Colors.White;
        public override bool IsInstanced => true;
        public override string CustomPackedIconPath => "res://EW/images/powers/timed_bomb.png";
        public override string CustomBigIconPath => "res://EW/images/powers/big/timed_bomb.png";

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(BombDamageKey, 0m)
        ];

        public decimal BombDamage => _damage;

        public EWTimedBombPower SetBomb(decimal damage, CardModel? cardSource)
        {
            _damage = damage;
            _cardSource = cardSource;
            DynamicVars[BombDamageKey].BaseValue = damage;
            return this;
        }

        public void AddDumbVariablesToPowerDescription(LocString description)
        {
            description.Add(CountdownKey, Amount);
            description.Add(BombDamageKey, _damage);
        }

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (Owner == null || Owner.IsDead || side != Owner.Side)
            {
                return;
            }

            if (Amount > 1)
            {
                await PowerCmd.Decrement(this);
                return;
            }

            await Detonate(choiceContext);
        }

        public async Task Detonate(PlayerChoiceContext choiceContext, CardModel? cardSource = null)
        {
            if (Owner == null || Owner.IsDead)
            {
                return;
            }

            var source = cardSource ?? _cardSource;
            var attack = DamageCmd.Attack(_damage);

            if (source != null)
            {
                attack.FromCard(source);
            }

            attack.Targeting(Owner)
                .Unpowered()
                .WithNoAttackerAnim();

            await attack.Execute(choiceContext);

            if (Applier != null)
            {
                foreach (var power in Applier.GetPowerInstances<EWBombCounterArmorPower>())
                {
                    await power.GainBombBlock(choiceContext, source, Owner);
                }
            }

            if (Owner != null && Owner.Powers.Contains(this))
            {
                await PowerCmd.Remove(this);
            }
        }
    }
}
