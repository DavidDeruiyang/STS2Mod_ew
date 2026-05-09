using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using EW.EWCode.Character;
using EW.EWCode.Extensions;
using EW.EWCode.Summons;
using EW.EWCode.Vfx;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    [Pool(typeof(EWCardPool))]
    public abstract class EWCard(int cost, CardType type, CardRarity rarity, TargetType target) :
        CustomCardModel(cost, type, rarity, target)
    {
        //Image size:
        //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
        //Full art: 606x852
        public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

        //Smaller variants of card images for efficiency:
        //Smaller variant of fullart: 250x350
        //Smaller variant of normalart: 250x190

        //Uses card_portraits/card_name.png as image path. These should be smaller images.
        public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
        public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

        protected static IHoverTip CardPreview<T>(bool upgraded = false) where T : CardModel
        {
            var card = ModelDb.Card<T>();
            if (upgraded)
            {
                card = (T)card.ToMutable();
                CardCmd.Upgrade(card, CardPreviewStyle.None);
            }

            return new CardHoverTip(card);
        }

        protected static IEnumerable<IHoverTip> SingleCardPreview<T>(bool upgraded = false) where T : CardModel
        {
            yield return CardPreview<T>(upgraded);
        }

        protected static async Task PlayHLZYAttack(PlayerChoiceContext choiceContext, Creature? target, CardModel cardSource)
        {
            var hitCount = SummonManager.CountHLZY();
            if (target == null || target.IsDead || hitCount <= 0)
            {
                return;
            }

            HLZYAttackVfx.PlayFromAllHLZYTo(target);

            await DamageCmd.Attack(1m)
                .FromCard(cardSource)
                .Targeting(target)
                .Unpowered()
                .WithNoAttackerAnim()
                .WithHitCount(hitCount)
                .Execute(choiceContext);
        }
    }
}
