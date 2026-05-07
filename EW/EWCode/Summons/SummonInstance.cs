using System.Collections.Generic;

namespace EW.EWCode.Summons
{
    public sealed class SummonInstance
    {
        public const string HLZYId = "HLZY";

        public string Id { get; init; } = HLZYId;
        public int SlotIndex { get; init; }
        public int Blood { get; set; } = 1;
        public int MaxBlood { get; init; } = 1;
        public List<SummonEffect> ProvidedEffects { get; } = [];

        public bool IsAlive => Blood > 0;
    }
}
