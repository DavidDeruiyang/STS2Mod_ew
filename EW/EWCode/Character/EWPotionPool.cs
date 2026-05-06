using BaseLib.Abstracts;
using EW.EWCode.Extensions;
using Godot;

namespace EW.EWCode.Character
{
    public class EWPotionPool : CustomPotionPoolModel
    {
        public override Color LabOutlineColor => EW.Color;


        public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
        public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    }
}