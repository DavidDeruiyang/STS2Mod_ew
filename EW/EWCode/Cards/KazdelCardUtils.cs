using EW.EWCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public static class KazdelCardUtils
    {
        public static bool IsKazdelCard(CardModel card)
        {
            return card is DieZhouJi or YingZhi or HunHeShuangDa or DieMengJi or JiFengErShi
                or MaFangYu or AnYeWuMing or YingShao or DianXiaDeZhuFu or DianXiaQiDeMingZi
                or BiJiBen or CanYing or KaZiDaiErYiZhang or KaZiDaiErDeXiWang or YongBingTuanAlpha or YongBingTuanBeta;
        }

        public static async Task AddRandomKazdelCardToHand(Player player, bool upgraded = false, int costDeltaThisTurn = 0)
        {
            if (player.Creature.CombatState == null)
            {
                return;
            }

            var candidates = ModelDb.CardPool<EWCardPool>()
                .AllCards
                .Where(IsKazdelCard)
                .ToList();
            player.RunState.Rng.CombatCardSelection.Shuffle(candidates);

            var card = candidates.FirstOrDefault();
            if (card == null)
            {
                return;
            }

            var generated = player.Creature.CombatState.CreateCard(card, player);
            if (upgraded)
            {
                CardCmd.Upgrade(generated, CardPreviewStyle.None);
            }

            if (costDeltaThisTurn != 0)
            {
                generated.EnergyCost.AddThisTurn(costDeltaThisTurn);
            }

            await CardPileCmd.AddGeneratedCardsToCombat([generated], PileType.Hand, true, CardPilePosition.Top);
        }
    }
}
