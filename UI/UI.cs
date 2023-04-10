using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DifficultySaver.UI
{
    class DSFlowCoordinator : FlowCoordinator
    {
        DSViewController view = null;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("Difficulty Saver");
                showBackButton = true;

                if (view == null)
                    view = BeatSaberUI.CreateViewController<DSViewController>();

                ProvideInitialViewControllers(view);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal);
        }

        public void ShowFlow()
        {
            var _parentFlow = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();

            BeatSaberUI.PresentFlowCoordinator(_parentFlow, this);
        }

        static DSFlowCoordinator flow = null;

        static MenuButton theButton;

        public static void Initialize()
        {

            MenuButtons.instance.RegisterButton(theButton ??= new MenuButton("Difficulty Saver", "Just saves your difficulty lmao", () => {
                if (flow == null)
                    flow = BeatSaberUI.CreateFlowCoordinator<DSFlowCoordinator>();

                flow.ShowFlow();
            }, true));
        }

        public static void Deinit()
        {
            if (theButton != null)
                MenuButtons.instance.UnregisterButton(theButton);
        }
    }

    [HotReload(RelativePathToLayout = @"./settings.bsml")]
    [ViewDefinition("DifficultySaver.UI.settings.bsml")]
    class DSViewController : BSMLAutomaticViewController
    {
        Config config = Config.Instance;
        bool ModToggle
        {
            get => config.ModToggle;
            set => config.ModToggle = value;
        }

        [UIValue("DifficultyOptions")]
        private List<object> options = new object[] { "Easy", "Normal", "Hard", "Expert", "Expert+" }.ToList();

        string DifficultySelected
        {
            get => config.DifficultySelected;
            set
            {
                config.DifficultySelected = value;

                if (!config.ModToggle) return;

                PlayerDataModel playerDataModel = Resources.FindObjectsOfTypeAll<PlayerDataModel>().First();
                PlayerData playerData = playerDataModel.playerData;

                BeatmapDifficulty beatmapDifficulty = BeatmapDifficulty.ExpertPlus;

                switch (config.DifficultySelected)
                {
                    case "Easy":
                        beatmapDifficulty = BeatmapDifficulty.Easy;
                        break;
                    case "Normal":
                        beatmapDifficulty = BeatmapDifficulty.Normal;
                        break;
                    case "Hard":
                        beatmapDifficulty = BeatmapDifficulty.Hard;
                        break;
                    case "Expert":
                        beatmapDifficulty = BeatmapDifficulty.Expert;
                        break;
                    case "Expert+":
                        beatmapDifficulty = BeatmapDifficulty.ExpertPlus;
                        break;
                };
                Plugin.Log.Info("Forced Difficulty to: " + config.DifficultySelected);


                playerData.SetLastSelectedBeatmapDifficulty(beatmapDifficulty);
            }
        }
    }

    public static class BsmlWrapper
    {
        static readonly bool hasBsml = IPA.Loader.PluginManager.GetPluginFromId("BeatSaberMarkupLanguage") != null;

        public static void EnableUI()
        {
            void wrap() => DSFlowCoordinator.Initialize();

            if (hasBsml)
                wrap();
        }
        public static void DisableUI()
        {
            void wrap() => DSFlowCoordinator.Deinit();

            if (hasBsml)
                wrap();
        }
    }
}