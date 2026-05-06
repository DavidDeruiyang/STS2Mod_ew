using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using EW.EWCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace EW.EWCode.Character
{
    public class EW : PlaceholderCharacterModel
    {
        public const string CharacterId = "维什戴尔";

        public static readonly Color Color = new("ffffff");

        public override string CustomCharacterSelectBg =>
            "res://scenes/screens/char_sel/char_select_bg_ew.tscn";


        public override Color NameColor => Color;
        public override CharacterGender Gender => CharacterGender.Neutral;
        public override int StartingHp => 90;

        public override IEnumerable<CardModel> StartingDeck => [
            ModelDb.Card<StrikeIronclad>(),
            ModelDb.Card<StrikeIronclad>(),
            ModelDb.Card<StrikeIronclad>(),
            ModelDb.Card<StrikeIronclad>(),
            ModelDb.Card<StrikeIronclad>(),
            ModelDb.Card<DefendIronclad>(),
            ModelDb.Card<DefendIronclad>(),
            ModelDb.Card<DefendIronclad>(),
            ModelDb.Card<DefendIronclad>(),
            ModelDb.Card<DefendIronclad>()
        ];

        public override IReadOnlyList<RelicModel> StartingRelics =>
        [
            ModelDb.Relic<BurningBlood>()
        ];

        public override CardPoolModel CardPool => ModelDb.CardPool<EWCardPool>();
        public override RelicPoolModel RelicPool => ModelDb.RelicPool<EWRelicPool>();
        public override PotionPoolModel PotionPool => ModelDb.PotionPool<EWPotionPool>();

        /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
            override all the other methods that define those assets. 
            These are just some of the simplest assets, given some placeholders to differentiate your character with. 
            You don't have to, but you're suggested to rename these images. */
        public override Control CustomIcon
        {
            get
            {
                var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
                icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
                return icon;
            }
        }
        public override string CustomIconTexturePath => "立绘_维什戴尔_skin2.png".AssetsPath(); // need 精2立绘
        public override string CustomCharacterSelectIconPath => "立绘_维什戴尔_skin2.png".AssetsPath(); // need 精2立绘
        public override string CustomCharacterSelectLockedIconPath => "立绘_维什戴尔_skin2.png".AssetsPath(); // need 精2立绘
        public override string CustomMapMarkerPath => "立绘_维什戴尔_skin2.png".AssetsPath(); // need 精2立绘
    }
}