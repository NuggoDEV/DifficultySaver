using IPA;
using HarmonyLib;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using System.Reflection;
using DifficultySaver.UI;

namespace DifficultySaver
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        internal static bool enabled { get; private set; } = true;

        public static Harmony harmony;

        [Init]
        public void Init(IPA.Config.Config conf, IPALogger logger)
        {
            Instance = this;
            Log = logger;
            Log.Info("DifficultySaver initialized.");
            Config.Instance = conf.Generated<Config>();
            harmony = new Harmony("NuggoDEV.BeatSaber.DifficultySaver");
        }

        [OnEnable]
        public void OnEnable()
        {
            enabled = true;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            BsmlWrapper.EnableUI();
        }

        [OnDisable]
        public void OnDisable()
        {
            enabled = false;
            harmony.UnpatchSelf();
            BsmlWrapper.DisableUI();
        }
    }
}
