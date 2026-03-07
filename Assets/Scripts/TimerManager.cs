using UnityEngine;
using TMPro;
using System.IO;
using System.Text;

public class TimerManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text finishText;

    private float totalStartTime;
    private float enemyTime = 0f;

    private bool runStarted = false;
    private bool runFinished = false;

    private int touchingCount = 0;

    void Update()
    {
        if (!runStarted || runFinished)
            return;

        if (touchingCount > 0)
        {
            enemyTime += Time.deltaTime;
        }

        float totalTime = Time.time - totalStartTime;
        float accuracy = 1 - enemyTime / totalTime;

        if (timerText != null)
            timerText.text = 
                $"Accuracy: {(accuracy * 100f):F0}%\n" +
                $"Time: {totalTime:F1}s";
    }

    public void StartRun()
    {
        if (runStarted) return;

        runStarted = true;
        totalStartTime = Time.time;
    }

    public void FinishRun()
    {
        if (runFinished) return;

        runFinished = true;

        float totalTime = Time.time - totalStartTime;
        float accuracy = 1 - enemyTime / totalTime;

        if (finishText != null)
        {
            finishText.text =
                $"Accuracy: {(accuracy * 100f):F0}%\n" +
                $"Enemy Time: {enemyTime:F1}s\n" +
                $"Total Time: {totalTime:F1}s";
        }

        SaveResults(accuracy, enemyTime, totalTime);
    }

    public void StartTouching()
    {
        touchingCount++;
    }

    public void StopTouching()
    {
        touchingCount = Mathf.Max(0, touchingCount - 1);
    }

    void SaveResults(float accuracy, float enemyTime, float totalTime)
    {
        string path ="results.csv";

        bool fileExists = File.Exists(path);

        StringBuilder sb = new StringBuilder();

        // write header if file doesn't exist
        if (!fileExists)
        {
            sb.AppendLine("Accuracy,EnemyTime,TotalTime");
        }

        sb.AppendLine($"{accuracy},{enemyTime},{totalTime}");

        File.AppendAllText(path, sb.ToString());

        Debug.Log("Saved results to: " + path);
    }
}