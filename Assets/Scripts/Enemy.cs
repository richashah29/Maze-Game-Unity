using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public TimerManager timerManager;  // reference to central timer manager
    private SpriteRenderer sr;

    private bool isTouchingPlayer = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false; // invisible by default
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.enabled = true;       // show enemy
            isTouchingPlayer = true;

            if (timerManager != null)
                timerManager.StartTouching();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.enabled = false;      // hide enemy
            isTouchingPlayer = false;

            if (timerManager != null)
                timerManager.StopTouching();
        }
    }
}