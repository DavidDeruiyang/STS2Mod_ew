using BaseLib.Utils;
using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZhanChangZhiZuo() : EWCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        private const string GeneratedCardKey = "GeneratedCard";

        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            IsUpgraded ? SingleCardPreview<D12>() : SingleCardPreview<D6Bomb>();

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(7, ValueProp.Move),
            new StringVar(GeneratedCardKey, "D6炸弹")
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Target == null)
            {
                return;
            }

            await CommonActions.CardAttack(this, cardPlay.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);

            if (IsUpgraded)
            {
                await BombUtils.AddBombCardToHand<D12>(Owner);
                return;
            }

            await BombUtils.AddBombCardToHand<D6Bomb>(Owner);
        }

        protected override void OnUpgrade()
        {
            ((StringVar)DynamicVars[GeneratedCardKey]).StringValue = "D12炸弹";
        }
    }
}
