using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;

    private float cumulativeTime = 0f;
    private int touchingCount = 0; // how many enemies player is currently touching

    void Update()
    {
        if (touchingCount > 0)
        {
            cumulativeTime += Time.deltaTime;
        }

        if (timerText != null)
            timerText.text = $"Time touching enemies: {cumulativeTime:F2}s";
    }

    // Called by Enemy when player touches
    public void StartTouching()
    {
        touchingCount++;
    }

    // Called by Enemy when player stops touching
    public void StopTouching()
    {
        touchingCount = Mathf.Max(0, touchingCount - 1);
    }
}