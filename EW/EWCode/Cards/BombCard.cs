using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;

namespace EW.EWCode.Cards
{
    public abstract class BombCard(
        int cost,
        CardType type,
        CardRarity rarity,
        TargetType target,
        decimal turns,
        decimal bombDamage
    ) : EWCard(cost, type, rarity, target)
    {
        public const string TurnsKey = "Turns";
        public const string BombDamageKey = "BombDamage";

        public decimal BombTurns => DynamicVars[TurnsKey].BaseValue;
        public decimal BombDamage => DynamicVars[BombDamageKey].BaseValue;

        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(TurnsKey, turns),
            new DynamicVar(BombDamageKey, bombDamage)
        ];
    }
}
