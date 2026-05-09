using EW.EWCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Powers
{
    public class EWExplosionGeniusPower : EWPower
    {
        private const string UpgradedBombKey = "UpgradedBomb";

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(UpgradedBombKey, 0m)
        ];

        public EWExplosionGeniusPower SetBombUpgraded(bool upgraded)
        {
            DynamicVars[UpgradedBombKey].BaseValue = upgraded ? 1m : 0m;
            return this;
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || player.Creature != Owner)
            {
                return;
            }

            await BombUtils.AddBombCardToHand<D6Bomb>(player, DynamicVars[UpgradedBombKey].BaseValue > 0m);
        }
    }
}
