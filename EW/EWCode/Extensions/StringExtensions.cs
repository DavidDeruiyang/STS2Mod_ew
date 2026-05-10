using Godot;

namespace EW.EWCode.Extensions
{
    //Mostly utilities to get asset paths.
    public static class StringExtensions
    {
        private static string ResPath(params string[] parts)
        {
            return $"{MainFile.ResPath}/{string.Join("/", parts).Replace('\\', '/')}";
        }

        public static string ImagePath(this string path)
        {
            return ResPath("images", path);
        }

        public static string CardImagePath(this string path)
        {
            path = ResPath("images", "card_portraits", path);
            if (ResourceLoader.Exists(path)) return path;

            MainFile.Logger.Info("Could not find card image path: " + path);
            return ResPath("images", "card_portraits", "card.png");
        }

        public static string BigCardImagePath(this string path)
        {
            path = ResPath("images", "card_portraits", "big", path);
            if (ResourceLoader.Exists(path)) return path;

            MainFile.Logger.Info("Could not find big card image path: " + path);
            return ResPath("images", "card_portraits", "big", "card.png");
        }

        public static string PowerImagePath(this string path)
        {
            path = ResPath("images", "powers", path);
            if (ResourceLoader.Exists(path)) return path;

            MainFile.Logger.Info("Could not find power image path: " + path);
            return ResPath("images", "powers", "power.png");
        }

        public static string BigPowerImagePath(this string path)
        {
            path = ResPath("images", "powers", "big", path);
            if (ResourceLoader.Exists(path)) return path;

            MainFile.Logger.Info("Could not find big power image path: " + path);
            return ResPath("images", "powers", "big", "power.png");
        }

        public static string RelicImagePath(this string path)
        {
            path = ResPath("images", "relics", path);
            if (ResourceLoader.Exists(path)) return path;

            MainFile.Logger.Info("Could not find relic image path: " + path);
            return ResPath("images", "relics", "relic.png");
        }

        public static string BigRelicImagePath(this string path)
        {
            path = ResPath("images", "relics", "big", path);
            if (ResourceLoader.Exists(path)) return path;

            MainFile.Logger.Info("Could not find big relic image path: " + path);
            return ResPath("images", "relics", "big", "relic.png");
        }

        public static string CharacterUiPath(this string path)
        {
            return ResPath("images", "character_ui", path);
        }
        public static string AssetsPath(this string path)
        {
            return ResPath("images", "assets", path);
        }
    }
}
