using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using EW.EWCode.Cards;
using EW.EWCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace EW.EWCode.Character
{
    public class EW : PlaceholderCharacterModel
    {
        public const string CharacterId = "维什戴尔";

        public static readonly Color Color = new("ffffff");

        // background
        public override string CustomCharacterSelectBg =>
            "res://EW/scenes/screens/char_sel/bg_anim_full.tscn";

        // combat visual
        public override string CustomVisualPath =>
            "res://EW/scenes/character/ew_combat_visual.tscn";
        // combat animation
        public override CreatureAnimator? SetupCustomAnimationStates(MegaSprite controller)
        {
            return SetupAnimationState(
                controller,
                idleName: "idle",
                deadName: "die",
                deadLoop: false,
                hitName: "hurt",
                hitLoop: false,
                attackName: "attack",
                attackLoop: false,
                castName: "cast",
                castLoop: false,
                relaxedName: "idle",
                relaxedLoop: true
            );
        }

        // basic setting
        public override Color NameColor => Color;
        public override CharacterGender Gender => CharacterGender.Neutral;
        public override int StartingHp => 15;


        // initial card
        public override IEnumerable<CardModel> StartingDeck => [
            ModelDb.Card<DieZhouJi>(),
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

        // starting relic
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
        public override string CustomIconTexturePath => "大头.png".AssetsPath(); // ingame 左上 小图
        public override string CustomCharacterSelectIconPath => "半身像3.png".AssetsPath(); // need 精2立绘
        public override string CustomCharacterSelectLockedIconPath => "半身像3.png".AssetsPath(); // need 精2立绘
        public override string CustomMapMarkerPath => "大头2.png".AssetsPath(); // 地图标识
    }
}