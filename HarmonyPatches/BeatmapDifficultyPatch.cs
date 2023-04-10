using HarmonyLib;

namespace DifficultySaver.HarmonyPatches
{
    [HarmonyPatch]
    internal class BeatmapDifficultyPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(LevelStatsView), nameof(LevelStatsView.ShowStats))]
        static void DifficultyPatch(PlayerData playerData)
        {
            Config config = Config.Instance;

            if (config.ModToggle)
            {
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
                }
                playerData.SetLastSelectedBeatmapDifficulty(beatmapDifficulty);
            }        
        }
    }
}
