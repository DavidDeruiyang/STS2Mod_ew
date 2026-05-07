using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using BaseLib.Utils;
using EW.EWCode.Cards;
using EW.EWCode.Extensions;
using EW.EWCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using System.Threading.Tasks;

namespace EW.EWCode.Character
{
    public class EW : PlaceholderCharacterModel
    {
        public const string CharacterId = "维什戴尔";
        private static readonly StringName RestartAnimationMethod = "ew_restart_animation";
        private static readonly StringName AttackAnimation = "attack";

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

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (cardPlay.Card.Type == CardType.Attack)
            {
                RestartCombatAnimation(AttackAnimation);
            }

            return Task.CompletedTask;
        }

        private static void RestartCombatAnimation(StringName animationName)
        {
            var room = NCombatRoom.Instance;
            if (room == null)
            {
                return;
            }

            if (!TryCallRestartAnimation(room, animationName))
            {
                MainFile.Logger.Info($"EW animation restart skipped: {animationName} body method was not found.");
            }
        }

        private static bool TryCallRestartAnimation(Node node, StringName animationName)
        {
            if (node.HasMethod(RestartAnimationMethod))
            {
                node.Call(RestartAnimationMethod, animationName);
                return true;
            }

            foreach (var child in node.GetChildren())
            {
                if (child is Node childNode && TryCallRestartAnimation(childNode, animationName))
                {
                    return true;
                }
            }

            return false;
        }

        // basic setting
        public override Color NameColor => Color;
        public override CharacterGender Gender => CharacterGender.Neutral;
        public override int StartingHp => 15;


        // initial card
        public override IEnumerable<CardModel> StartingDeck => [
            ModelDb.Card<DieZhouJi>(),
            ModelDb.Card<DieZhouJi>(),
            ModelDb.Card<DieZhouJi>(),
            ModelDb.Card<QiangLiJi>(),
            ModelDb.Card<SummonHLZY>(),
            ModelDb.Card<DismissHLZY>(),
            ModelDb.Card<D12>()
        ];

        // starting relic
        public override IReadOnlyList<RelicModel> StartingRelics =>
        [
            ModelDb.Relic<HLZYRelic>()
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
