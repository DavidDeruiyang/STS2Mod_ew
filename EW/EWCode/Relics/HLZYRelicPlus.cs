using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

namespace EW.EWCode.Relics
{
    public class HLZYRelicPlus : HLZYRelic
    {
        public override RelicRarity Rarity => RelicRarity.Ancient;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            await base.AfterPlayerTurnStart(choiceContext, player);

            if (Owner == null || player != Owner)
            {
                return;
            }

            await SummonManager.SummonHLZYWhenReady(
                SummonSource.Relic,
                summoner: Owner.Creature
            );
        }
    }
}
