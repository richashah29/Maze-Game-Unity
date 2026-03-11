using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    [Header("References")]
    public TimerManager timerManager;  // reference to central timer manager
    public Image flash;
    private SpriteRenderer sr;

    private bool isTouchingPlayer = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false; // invisible by default
        flash.color = new Color(1f, 0f, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            flash.color = new Color(1f, 0f, 0f, 0.5f);       // flash screen
            isTouchingPlayer = true;

            if (timerManager != null)
                timerManager.StartTouching();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            flash.color = new Color(1f, 0f, 0f, 0f);      // return to screen
            isTouchingPlayer = false;

            if (timerManager != null)
                timerManager.StopTouching();
        }
    }

}
