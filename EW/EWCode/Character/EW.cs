using BaseLib.Abstracts;
using BaseLib.Utils;
using BaseLib.Utils.NodeFactories;
using EW.EWCode.Cards;
using EW.EWCode.Extensions;
using EW.EWCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System.Threading.Tasks;

namespace EW.EWCode.Character
{
    public class EW : PlaceholderCharacterModel
    {
        public const string CharacterId = "维什戴尔";
        private static readonly StringName RestartAnimationMethod = "ew_restart_animation";
        private static readonly StringName AttackAnimation = "attack";

        public static readonly Color Color = new("a11f2b");

        // background
        //public override string CustomCharacterSelectBg =>
        //    "res://EW/scenes/screens/char_sel/bg_anim_full.tscn";
        public override string CustomCharacterSelectBg =>
            "res://EW/scenes/screens/char_sel/hq_bg.tscn";

        // combat visual
        public override string CustomVisualPath =>
            "res://EW/scenes/character/ew_combat_visual.tscn";

        public override string CustomMerchantAnimPath =>
            "res://EW/scenes/merchant/characters/ew_merchant.tscn";

        public override string CustomRestSiteAnimPath =>
            "res://EW/scenes/rest_site/characters/ew_rest_site.tscn";

        public override string CustomEnergyCounterPath =>
            "res://EW/scenes/combat/energy_counters/ew_energy_counter.tscn";

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
        public override int StartingHp => 99;


        // initial card
        //public override IEnumerable<CardModel> StartingDeck => [
        //    // ModelDb.Card<DieZhouJi>(),
        //    // ModelDb.Card<QiangLiJi>(),
        //    // ModelDb.Card<SummonHLZY>(), // test card, will be removed later
        //    // ModelDb.Card<HongTaoK>(),
        //    // ModelDb.Card<ZhanShuSheJi>(),
        //    ModelDb.Card<FanWeiDaJi>(),
        //    // ModelDb.Card<SuperBigCup>(),
        //    // ModelDb.Card<CiSha>(),
        //    ModelDb.Card<LengCiDunRen>(),
        //    // ModelDb.Card<JiuHuiWanZheGe>(),
        //    // ModelDb.Card<SuiYiKaiHuo>(),
        //    // ModelDb.Card<YanWuDan>(),
        //    ModelDb.Card<LinShiYanTi>(),
        //    // ModelDb.Card<ZhiShiXueBao>(),
        //    // ModelDb.Card<WeiWeiMei>(),
        //    ModelDb.Card<ZaiBuShu>(),
        //    ModelDb.Card<ShiDaiDeYanLei>(),
        //    ModelDb.Card<WoDuBuDongYanJiangGao>(),
        //    // ModelDb.Card<ShengYuHeiYe>(),
        //    ModelDb.Card<GeiWoWanMingRiFangZhou>(),
        //    // ModelDb.Card<YingZhi>(),
        //    // ModelDb.Card<HunHeShuangDa>(),
        //    // ModelDb.Card<DieMengJi>(),
        //    // ModelDb.Card<JiFengErShi>(),
        //    // ModelDb.Card<MaFangYu>(),
        //    // ModelDb.Card<AnYeWuMing>(),
        //    ModelDb.Card<YingShao>(),
        //    // ModelDb.Card<DianXiaDeZhuFu>(),
        //    // ModelDb.Card<DianXiaQiDeMingZi>(),
        //    ModelDb.Card<KaZiDaiErYiZhang>(),
        //    ModelDb.Card<KaZiDaiErDeXiWang>(),
        //    // ModelDb.Card<YongBingTuanAlpha>(),
        //    // ModelDb.Card<YongBingTuanBeta>(),
        //    ModelDb.Card<BiJiBen>(),
        //    // ModelDb.Card<ZuZongFaSheQi>(),
        //    // ModelDb.Card<GongTongChuJi>(),
        //    ModelDb.Card<ZuZongLeiJi>(),
        //    // ModelDb.Card<BaoHeFuChou>(),
        //    // ModelDb.Card<BaoLieLiMingDanYao>(),
        //    ModelDb.Card<ShuangChongDaJi>(),
        //    ModelDb.Card<GongFangJianBei>(),
        //    // ModelDb.Card<BaoLieLiMing>(),
        //    // ModelDb.Card<JinGongYuWang>(),
        //    // ModelDb.Card<JiNengDuiZhou>(),
        //    // ModelDb.Card<GunDongXianZu>(),
        //    // ModelDb.Card<HuoLiBuZu>(),
        //    ModelDb.Card<ZuZongXianLing>(),
        //    // ModelDb.Card<SiHunLingDeYuXi>(),
        //    ModelDb.Card<TouZhiShou>(),
        //    // ModelDb.Card<HunLingBiYou>(),
        //    ModelDb.Card<HaoLi>(),
        //    // ModelDb.Card<XianZuDeBiHu>(),
        //    ModelDb.Card<HongMingZhiShou>(),
        //    ModelDb.Card<CanYing>(),
        //    // tested
        //    //ModelDb.Card<DismissHLZY>(),  // test card, will be removed later
        //    //ModelDb.Card<D12>(),
        //    //ModelDb.Card<BaoFanZhuangJia>(),
        //    //ModelDb.Card<D6Bomb>(),
        //    //ModelDb.Card<SuperBigBoom>(),
        //    //ModelDb.Card<FangYuXingBaoZha>(),
        //    //ModelDb.Card<TongGuiYuJin>(),
        //    //ModelDb.Card<YuanChengYinBao>(),
        //    //ModelDb.Card<ZhanChangZhiZuo>(),
        //    //ModelDb.Card<ZhaDanLianJie>(),
        //    //ModelDb.Card<ZhaDanKuangRen>(),
        //    //ModelDb.Card<ZhaDanMoWangTing>(),
        //    //ModelDb.Card<ZhaDanZhiZuo>(),
        //    //ModelDb.Card<QiBao>(),
        //    //ModelDb.Card<JiKeBaoZha>(),
        //    //ModelDb.Card<SanErYi>(),
        //    //ModelDb.Card<SuiJiTouZhi>(),
        //    //ModelDb.Card<BaoZhaTianCai>()
        //];

        public override IEnumerable<CardModel> StartingDeck => [
            //ModelDb.Card<FanWeiDaJi>(),   //卡牌描述问题
            ModelDb.Card<SuperBigCup>(),
            ModelDb.Card<LengCiDunRen>(),  // 破盾问题， propvalue.unbblockable好像没用
            //ModelDb.Card<LinShiYanTi>(),  //减费生效问题
            //ModelDb.Card<ZaiBuShu>(),
            //ModelDb.Card<ShiDaiDeYanLei>(),
            //ModelDb.Card<WoDuBuDongYanJiangGao>(),
            //ModelDb.Card<GeiWoWanMingRiFangZhou>(),
            //ModelDb.Card<YingShao>(),
            //ModelDb.Card<KaZiDaiErYiZhang>(),
            //ModelDb.Card<KaZiDaiErDeXiWang>(),
            ModelDb.Card<BiJiBen>(), //不检测是否空手牌
            //ModelDb.Card<ZuZongLeiJi>(),
            //ModelDb.Card<ShuangChongDaJi>(),
            //ModelDb.Card<GongFangJianBei>(),
            //ModelDb.Card<ZuZongXianLing>(),
            //ModelDb.Card<TouZhiShou>(),
            //ModelDb.Card<HaoLi>(),
            //ModelDb.Card<HongMingZhiShou>(),
            //ModelDb.Card<CanYing>(),
        ];
        //  card is DieZhouJi or YingZhi or HunHeShuangDa or DieMengJi or JiFengErShi or MaFangYu or AnYeWuMing or YingShao or DianXiaDeZhuFu or DianXiaQiDeMingZi;

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
