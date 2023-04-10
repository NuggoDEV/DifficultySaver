using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace DifficultySaver
{
    internal class Config
    {
        public static Config Instance;
        public bool ModToggle { get; set; } = true;

        public string DifficultySelected { get; set; } = "Expert";


    }
}
