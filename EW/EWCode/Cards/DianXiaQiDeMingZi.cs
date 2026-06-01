using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class DianXiaQiDeMingZi() : EWCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "SL3 05 dian_xia_qi_de_ming_zi.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.KazdelCard];

        private const string PlatingKey = "Plating";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(PlatingKey, 3m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await PowerCmd.Apply<PlatingPower>(Owner.Creature, DynamicVars[PlatingKey].BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade() => DynamicVars[PlatingKey].UpgradeValueBy(1m);
    }
}
