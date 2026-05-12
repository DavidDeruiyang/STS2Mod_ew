using EW.EWCode.Keywords;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class JiNengDuiZhou() : EWCard(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "SL4 03 ji_neng_dui_zhou.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => IsUpgraded ? [EWKeywords.SoulShadow] : [EWKeywords.SoulShadow, CardKeyword.Exhaust];

        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            _ = SummonManager.SummonHLZYWhenReady(SummonSource.Card, summoner: Owner?.Creature, cardSource: this);
            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}
