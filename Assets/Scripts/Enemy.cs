using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public TimerManager timerManager; 
    private SpriteRenderer sr;

    private static List<Enemy> allEnemies = new List<Enemy>();
    private static bool isFlashing = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

        allEnemies.Add(this);
    }

    void OnDestroy()
    {
        allEnemies.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // CHOICE A: Visual Feedback (Show all enemy sprites briefly)
            if (MenuLogic.FeedbackType == "Visual")
            {
                TriggerGlobalFlash();
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

    private void TriggerGlobalFlash()
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        foreach (var enemy in allEnemies)
        {
            if (enemy != null && enemy.sr != null)
            {
                enemy.sr.enabled = true;
            }
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var enemy in allEnemies)
        {
            if (enemy != null && enemy.sr != null)
            {
                enemy.sr.enabled = false;
            }
        }

        isFlashing = false;
    }
}