using System.Reflection;
using System.Runtime.Loader;

var gameDir = @"E:\Steam\steamapps\common\Slay the Spire 2\data_sts2_windows_x86_64";
AssemblyLoadContext.Default.Resolving += (_, name) =>
{
    var path = Path.Combine(gameDir, name.Name + ".dll");
    return File.Exists(path) ? AssemblyLoadContext.Default.LoadFromAssemblyPath(path) : null;
};

var sts2 = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(gameDir, "sts2.dll"));
var baseLib = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
    @".nuget\packages\alchyr.sts2.baselib\3.1.0\lib\net9.0\BaseLib.dll"));
foreach (var typeName in new[]
{
    "MegaCrit.Sts2.Core.Models.AbstractModel",
    "MegaCrit.Sts2.Core.Models.CardModel",
    "MegaCrit.Sts2.Core.Models.Cards.CardModel",
    "MegaCrit.Sts2.Core.Models.Cards.StrikeIronclad",
    "MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardLibrary",
    "MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardLibraryGrid",
    "MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardLibraryStats",
    "MegaCrit.Sts2.Core.Saves.ProgressState",
    "MegaCrit.Sts2.Core.Saves.ProfileScopedData",
    "BaseLib.Abstracts.CustomCardModel",
    "BaseLib.Abstracts.ConstructedCardModel",
    "MegaCrit.Sts2.Core.Unlocks.UnlockState",
    "MegaCrit.Sts2.Core.Unlocks.Discovery",
    "MegaCrit.Sts2.Core.Models.Cards.Discovery",
    "MegaCrit.Sts2.Core.Save.SerializableUnlockState"
})
{
    var type = sts2.GetType(typeName) ?? baseLib.GetType(typeName);
    Console.WriteLine("TYPE " + typeName);
    if (type == null)
    {
        continue;
    }

    foreach (var member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                 .Where(m => m is ConstructorInfo || m.Name.Contains("Card") || m.Name.Contains("Library") || m.Name.Contains("Discover") || m.Name.Contains("Unlock") || m.Name.Contains("ShouldShow") || m.Name.Contains("Progress"))
                 .OrderBy(m => m.Name))
    {
        Console.WriteLine("  " + Format(member));
    }
}

static string Format(MemberInfo member)
{
    return member switch
    {
        MethodInfo m => $"{m.Attributes} virtual={m.IsVirtual} final={m.IsFinal} base={m.GetBaseDefinition().DeclaringType?.FullName} {m.ReturnType.Name} {m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name))})",
        PropertyInfo p => $"{p.PropertyType.Name} {p.Name}; get={(p.GetMethod?.Attributes.ToString() ?? "none")}; set={(p.SetMethod?.Attributes.ToString() ?? "none")}",
        FieldInfo f => $"{f.Attributes} {f.FieldType.Name} {f.Name}",
        ConstructorInfo c => $"{c.Attributes} {c.DeclaringType?.Name}({string.Join(", ", c.GetParameters().Select(p => p.ParameterType.FullName + " " + p.Name + (p.HasDefaultValue ? "=" + (p.DefaultValue ?? "null") : "")))})",
        _ => member.Name
    };
}
