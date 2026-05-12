using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZhaDanMoWangTing() : EWCard(3, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "PL2 03 zha_dan_mo_wang_ting.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null)
            {
                return;
            }

            await PowerCmd.Apply<EWBombDemonCourtPower>(owner, 1m, owner, this);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }
    }
}
