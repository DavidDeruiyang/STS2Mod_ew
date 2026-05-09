using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace EW.EWCode.Keywords
{
    public static class EWKeywords
    {
        [CustomEnum("OriginiumBomb")]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword OriginiumBomb;

        [CustomEnum("SoulShadow")]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword SoulShadow;

        [CustomEnum("Camouflage")]
        [KeywordProperties(AutoKeywordPosition.None)]
        public static CardKeyword Camouflage;
    }
}
