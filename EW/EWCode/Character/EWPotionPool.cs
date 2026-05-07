using BaseLib.Abstracts;
using EW.EWCode.Extensions;
using Godot;

namespace EW.EWCode.Character
{
    public class EWPotionPool : CustomPotionPoolModel
    {
        public override Color LabOutlineColor => EW.Color;


        public override string BigEnergyIconPath => "big_energy.png".CharacterUiPath();
        public override string TextEnergyIconPath => "text_energy.png".CharacterUiPath();
    }
}
