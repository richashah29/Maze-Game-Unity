using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Linq;

public static class ExperimentManager
{
    // Visual round results
    public static float accuracyVisual;
    public static float enemyTimeVisual;
    public static float totalTimeVisual;

    // Text round results
    public static float accuracyText;
    public static float enemyTimeText;
    public static float totalTimeText;

    public static void Reset()
    {
        accuracyVisual = enemyTimeVisual = totalTimeVisual = 0f;
        accuracyText = enemyTimeText = totalTimeText = 0f;
    }

    public static void RegisterRun(string feedbackType, float accuracy01, float enemyTime, float totalTime)
    {
        if (feedbackType == "Visual")
        {
            // Save visual-feedback round; wait for user to continue to type 2
            accuracyVisual = accuracy01;
            enemyTimeVisual = enemyTime;
            totalTimeVisual = totalTime;
        }
        else if (feedbackType == "Text")
        {
            // Save text-feedback round and write combined row
            accuracyText = accuracy01;
            enemyTimeText = enemyTime;
            totalTimeText = totalTime;

            SaveCombinedRow();
            Reset();
        }
    }

    private static void SaveCombinedRow()
    {
        string path = "results.csv";
        string header = "PlayerType,accuracy_visual,enemy_time_visual,total_time_visual,accuracy_text,enemy_time_text,total_time_text";

        bool writeHeader = true;

        if (File.Exists(path))
        {
            // check first line
            string firstLine = File.ReadLines(path).FirstOrDefault();
            if (firstLine != null && firstLine.Trim() == header)
            {
                writeHeader = false; // header already exists
            }
        }

        StringBuilder sb = new StringBuilder();

        if (writeHeader)
        {
            sb.AppendLine(header);
        }

        string playerType = MenuLogic.PlayerType;

        sb.AppendLine($"{playerType},{accuracyVisual},{enemyTimeVisual},{totalTimeVisual},{accuracyText},{enemyTimeText},{totalTimeText}");

        File.AppendAllText(path, sb.ToString());
    }
}
