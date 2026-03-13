using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    public Button finishButton;

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
        finishButton.gameObject.SetActive(false);
        if (runStarted) return;

        runStarted = true;
        totalStartTime = Time.time;
    }

    public void FinishRun(){
        if (runFinished) return;
        finishButton.gameObject.SetActive(true);
        
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
        // accuracy here is 0–1
        string feedbackType = MenuLogic.FeedbackType;
        ExperimentManager.RegisterRun(feedbackType, accuracy, enemyTime, totalTime);
    }
}