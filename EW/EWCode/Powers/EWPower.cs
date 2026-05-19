using BaseLib.Abstracts;
using BaseLib.Extensions;
using EW.EWCode.Extensions;
using Godot;
using System.Collections.Generic;

namespace EW.EWCode.Powers
{
    public abstract class EWPower : CustomPowerModel
    {
        private static readonly Dictionary<string, string> CardPowerIconFiles = new()
        {
            [nameof(EWTimedBombPower)] = "timed_bomb.png",
            [nameof(EWCamouflagePower)] = "micai.png",
            [nameof(EWEndCombatHealPower)] = "PIL1 02 gei_wo_wan_mrfz.png",
            [nameof(EWExplosionGeniusPower)] = "PIL2 01 bao_zha_tian_cai.png",
            [nameof(EWThreeTwoOnePower)] = "PIL2 02 3_2_1.png",
            [nameof(EWBombDemonCourtPower)] = "PIL2 03 zha_dan_mo_wang_ting.png",
            [nameof(EWBombCounterArmorPower)] = "PIL2 04 bao_fan_zhuang_jia.png",
            [nameof(EWKazdelSpeakerPower)] = "PIL3 01 ka_zi_dai_er_yi_zhang.png",
            [nameof(EWKazdelHopePower)] = "PIL3 02 ka_zi_dai_er_de_xi_wang.png",
            [nameof(EWKazdelStrengthPower)] = "PIL3 03 yong_bing_tuan_α.png",
            [nameof(EWKazdelDexterityPower)] = "PIL3 04 yong_bing_tuan_β.png",
            [nameof(EWNotebookPower)] = "PIL3 05 bi_ji_ben.png",
            [nameof(EWSoulRemainsPower)] = "PIL4 01 si_hun_ling_de_yu_xi.png",
            [nameof(EWHLZYSplashPower)] = "PIL4 02 tou_zhi_shou.png",
            [nameof(EWSoulBlessingPower)] = "PIL4 03 hun_lin_bi_you.png",
            [nameof(EWHLZYRepeatAttackPower)] = "PIL4 04 hao_li.png",
            [nameof(EWAncestorGuardPower)] = "PIL4 05 xian_zu_de_bi_hu.png",
            [nameof(EWHLZYRoaringHandPower)] = "PIL4 06 hong_ming_zhi_shou.png",
            [nameof(EWRemoveStrengthAtTurnEndPower)] = "remove_strength_at_turn_end.png",
            [nameof(EWAfterimagePower)] = "PIL4 07 can_ying.png",
            [nameof(EWAfterimageMarkPower)] = "canying_debuff.png",
            [nameof(EWRestoreStrengthAtTurnEndPower)] = "restore_strength_at_turn_end.png",
            [nameof(EWNextCardCostUpPower)] = "next_card_cost_up.png",
            [nameof(EWNextTurnEnergyPower)] = "nextTurnEnergy.png",
        };

        private string IconFileName =>
            CardPowerIconFiles.TryGetValue(GetType().Name, out var fileName)
                ? fileName
                : "power.png";

        public override string CustomPackedIconPath => IconFileName.PowerImagePath();
        public override string CustomBigIconPath => IconFileName.BigPowerImagePath();
    }
}
