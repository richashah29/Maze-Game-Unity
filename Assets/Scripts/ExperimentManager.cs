using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine; // Added for Debug.Log

public static class ExperimentManager
{
    public static float accuracyVisual;
    public static float enemyTimeVisual;
    public static float totalTimeVisual;

    public static float accuracyText;
    public static float enemyTimeText;
    public static float totalTimeText;

    // Track if we have received data for both rounds
    private static bool hasVisualData = false;
    private static bool hasTextData = false;

    public static void Reset()
    {
        accuracyVisual = enemyTimeVisual = totalTimeVisual = 0f;
        accuracyText = enemyTimeText = totalTimeText = 0f;
        hasVisualData = false;
        hasTextData = false;
    }

    public static void RegisterRun(string feedbackType, float accuracy01, float enemyTime, float totalTime)
    {
        if (feedbackType == "Visual")
        {
            accuracyVisual = accuracy01;
            enemyTimeVisual = enemyTime;
            totalTimeVisual = totalTime;
            hasVisualData = true;
            Debug.Log("Visual Data Registered");
        }
        else if (feedbackType == "Text")
        {
            accuracyText = accuracy01;
            enemyTimeText = enemyTime;
            totalTimeText = totalTime;
            hasTextData = true;
            Debug.Log("Text Data Registered");
        }

        // ONLY save if BOTH halves are complete
        if (hasVisualData && hasTextData)
        {
            SaveCombinedRow();
            Reset(); // Clear everything for the next participant
        }
    }

    private static void SaveCombinedRow()
    {
        string path = "results.csv";
        string header = "PlayerType,accuracy_visual,enemy_time_visual,total_time_visual,accuracy_text,enemy_time_text,total_time_text";

        bool writeHeader = true;

        if (File.Exists(path))
        {
            string firstLine = File.ReadLines(path).FirstOrDefault();
            if (firstLine != null && firstLine.Trim() == header)
            {
                writeHeader = false;
            }
        }

        StringBuilder sb = new StringBuilder();
        if (writeHeader) sb.AppendLine(header);

        string playerType = MenuLogic.PlayerType; 

        sb.AppendLine($"{playerType},{accuracyVisual},{enemyTimeVisual},{totalTimeVisual},{accuracyText},{enemyTimeText},{totalTimeText}");

        File.AppendAllText(path, sb.ToString());
        Debug.Log("CSV Row Saved Successfully!");
    }
}