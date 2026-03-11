using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;


public class TimerManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text finishText;

    public GameObject warningText;
    public Image flashImage;

    private float totalStartTime;
    private float enemyTime = 0f;

    private bool runStarted = false;
    private bool runFinished = false;

    private int touchingCount = 0;

    void Update(){
        if (!runStarted || runFinished) {return;}

        if (touchingCount > 0){
            enemyTime += Time.deltaTime;
        }

        float totalTime = Time.time - totalStartTime;
        float accuracy = 1 - enemyTime / totalTime;

        if (timerText != null)
            timerText.text = 
                $"Accuracy: {(accuracy * 100f):F0}%\n" +
                $"Time: {totalTime:F1}s";
    }

    public void StartRun(){
        if (runStarted) return;

        runStarted = true;
        totalStartTime = Time.time;
    }

    public void FinishRun(){
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

    public void StartTouching(){
        touchingCount++;

        if (MenuLogic.FeedbackType == "Text")
        {
            // 1. Show the Text Box
            if (warningText != null) warningText.SetActive(true);

            // 2. Turn on the Overlay and set the "tint"
            if (flashImage != null)
            {
                flashImage.gameObject.SetActive(true);
                // Red at 40% transparency
                flashImage.color = new Color(1f, 0f, 0f, 0.4f); 
            }
        }
    }

    public void StopTouching()
    {
        touchingCount = Mathf.Max(0, touchingCount - 1);
        Debug.Log("Stopped touching. Current count: " + touchingCount); // Check this!

        if (touchingCount == 0)
        {
            if (warningText != null) warningText.SetActive(false);

            if (flashImage != null)
            {
                flashImage.color = new Color(1f, 0f, 0f, 0f);
                flashImage.gameObject.SetActive(false);
            }
        }
    }

    void SaveResults(float accuracy, float enemyTime, float totalTime)
    {
    string path = "results.csv";
    bool fileExists = File.Exists(path);
    StringBuilder sb = new StringBuilder();

    // Updated Header to include 2 new categories
    if (!fileExists){
        sb.AppendLine("PlayerType,FeedbackType,Accuracy,EnemyTime,TotalTime");
    }

    // Grabbing the data from your Welcome Page script
    string playerType = MenuLogic.PlayerType;
    string feedbackType = MenuLogic.FeedbackType;

    // 3. Adding them to the start of the row
    sb.AppendLine($"{playerType},{feedbackType},{accuracy},{enemyTime},{totalTime}");

    File.AppendAllText(path, sb.ToString());
    Debug.Log("Saved results to: " + path);
    }
}