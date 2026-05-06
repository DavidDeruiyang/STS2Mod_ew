using BaseLib.Abstracts;
using BaseLib.Utils;
using EW.EWCode.Character;

namespace EW.EWCode.Potions
{
    [Pool(typeof(EWPotionPool))]
    public abstract class EWPotion : CustomPotionModel;
}