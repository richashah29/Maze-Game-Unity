using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    public static string PlayerType = "Unknown"; 
    public static string FeedbackType = "Unknown";

    [Header("UI References")]
    public Button startButton;    // the Start button
    public Button gamerButton;    // optional: to disable after selection
    public Button nonGamerButton; // optional: to disable after selection

    void Start()
    {
        ResetMenu();
    }

    // Called when player selects Gamer
    public void PickGamer() 
    { 
        PlayerType = "Gamer"; 
        Debug.Log("Gamer Selected"); 
        ShowStartButton();
        DisableTypeButtons();
    }

    // Called when player selects Non-Gamer
    public void PickNonGamer() 
    { 
        PlayerType = "Non-Gamer"; 
        Debug.Log("Non-Gamer Selected"); 
        ShowStartButton();
        DisableTypeButtons();
    }

    private void ShowStartButton()
    {
        if (startButton != null)
            startButton.gameObject.SetActive(true);
    }

    private void DisableTypeButtons()
    {
        if (gamerButton != null)
            gamerButton.interactable = false;

        if (nonGamerButton != null)
            nonGamerButton.interactable = false;
    }

    private void EnableTypeButtons()
    {
        if (gamerButton != null)
            gamerButton.interactable = true;

        if (nonGamerButton != null)
            nonGamerButton.interactable = true;
    }

    // Start experiment: Feedback Type 1
    public void LoadFeedback1()
    {
        FeedbackType = "Visual";
        SceneManager.LoadScene("Visual Feedback");
    }

    // Continue to Feedback Type 2
    public void LoadFeedback2()
    {
        FeedbackType = "Text";
        SceneManager.LoadScene("Text Feedback");
    }

    // Called to end the game and go back to welcome menu
    public void EndGame()
    {
        LoadWelcome();
    }

    public void LoadWelcome()
    {
        ExperimentManager.Reset();

        // Reset static variables
        PlayerType = "Unknown";
        FeedbackType = "Unknown";

        // Reset UI
        ResetMenu();

        // Reload the welcome scene (or your main menu scene)
        SceneManager.LoadScene("Welcome Page");
    }

    private void ResetMenu()
    {
        // Hide Start button
        if (startButton != null)
            startButton.gameObject.SetActive(false);

        // Re-enable player type selection buttons
        EnableTypeButtons();
    }
}