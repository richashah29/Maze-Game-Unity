using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public TimerManager timerManager; 
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // CHOICE A: Visual Feedback (Show the Sprite)
            if (MenuLogic.FeedbackType == "Visual")
            {
                sr.enabled = true; 
            }
            
            // CHOICE B: Text Feedback (Handled inside TimerManager)
            if (timerManager != null){
                timerManager.StartTouching();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.enabled = false; // Always hide sprite when leaving

            if (timerManager != null) {
                timerManager.StopTouching();
            }
        }
    }
}