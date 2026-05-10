using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class HongMingZhiShou() : EWCard(3, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.SoulShadow];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWHLZYRoaringHandPower>(Owner.Creature, 1m, Owner.Creature, this);
        }

        protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    }
}
