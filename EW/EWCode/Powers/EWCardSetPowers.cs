using BaseLib.Utils;
using EW.EWCode.Cards;
using EW.EWCode.Powers;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Powers
{
    public class EWEndCombatHealPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWKazdelSpeakerPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || player.Creature != Owner) return;
            await EWCard.AddGeneratedCards<DieZhouJi>(player, PileType.Hand, 1, false, Amount > 0m ? -1 : 0);
        }
    }

    public class EWKazdelHopePower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWKazdelStrengthPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Card.Owner?.Creature != Owner || !IsKazdelCard(cardPlay.Card)) return;
            await CommonActions.ApplySelf<StrengthPower>(choiceContext, cardPlay.Card, Amount);
        }

        protected static bool IsKazdelCard(CardModel card)
        {
            return card is DieZhouJi or YingZhi or HunHeShuangDa or DieMengJi or JiFengErShi
                or MaFangYu or AnYeWuMing or YingShao or DianXiaDeZhuFu or DianXiaQiDeMingZi;
        }
    }

    public class EWKazdelDexterityPower : EWKazdelStrengthPower
    {
        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Card.Owner?.Creature != Owner || !IsKazdelCard(cardPlay.Card)) return;
            await CommonActions.ApplySelf<DexterityPower>(choiceContext, cardPlay.Card, Amount);
        }
    }

    public class EWNotebookPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (Owner == null || side != Owner.Side) return;
            await PowerCmd.Apply<EWNextTurnEnergyPower>(Owner, Amount, Owner, null);
        }
    }

    public class EWSoulRemainsPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || player.Creature != Owner || SummonManager.CountHLZY() <= 0) return;
            await PowerCmd.Apply<EWCamouflagePower>(Owner, 1m, Owner, null);
        }
    }

    public class EWSoulBlessingPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || player.Creature != Owner || SummonManager.CountHLZY() <= 0) return;
            await PlayerCmd.GainEnergy(1m, player);
        }
    }

    public class EWAncestorGuardPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (Owner == null || side != Owner.Side) return;
            var block = Amount * SummonManager.CountHLZY();
            if (block > 0m) await CreatureCmd.GainBlock(Owner, block, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move, null, false);
        }
    }

    public class EWHLZYSplashPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWHLZYRepeatAttackPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWHLZYRoaringHandPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWAfterimagePower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }
}
