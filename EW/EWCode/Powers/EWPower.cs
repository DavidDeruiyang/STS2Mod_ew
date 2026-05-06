using BaseLib.Abstracts;
using BaseLib.Extensions;
using EW.EWCode.Extensions;
using Godot;

namespace EW.EWCode.Powers
{
    public abstract class EWPower : CustomPowerModel
    {
        //Loads from EW/images/powers/your_power.png
        public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
        public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
    }
}