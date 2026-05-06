using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using EW.EWCode.Character;
using EW.EWCode.Extensions;
using Godot;

namespace EW.EWCode.Relics
{
    [Pool(typeof(EWRelicPool))]
    public abstract class EWRelic : CustomRelicModel
    {
        public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
        protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
        protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
    }
}