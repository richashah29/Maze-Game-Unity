using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour 
{
    public static string PlayerType = "Unknown"; 
    public static string FeedbackType = "Unknown";
    
    // This stays true even when scenes change during one session
    private static bool isSecondRound = false;
    public static bool IsSecondRound => isSecondRound;

    [Header("UI References")]
    public Button startButton;    
    public Button gamerButton;    
    public Button nonGamerButton; 

    void Start()
    {
        ResetMenu();
    }

    public void PickGamer() { PlayerType = "Gamer"; ShowStartButton(); DisableTypeButtons(); }
    public void PickNonGamer() { PlayerType = "Non-Gamer"; ShowStartButton(); DisableTypeButtons(); }

    private void ShowStartButton() { if (startButton != null) startButton.gameObject.SetActive(true); }

    public void StartExperiment()
    {
        // 0 = Visual, 1 = Text. 
        // We look up what the LAST PLAYER started with.
        int starterType = PlayerPrefs.GetInt("StarterType", 1); 

        if (!isSecondRound)
        {
            // ROUND 1: Play the opposite of what the previous player started with
            if (starterType == 1) { LoadVisual(); }
            else { LoadText(); }
        }
        else
        {
            // ROUND 2: Play the one we haven't done yet
            if (FeedbackType == "Visual") { LoadText(); }
            else { LoadVisual(); }
        }
    }

    private void LoadVisual() { FeedbackType = "Visual"; SceneManager.LoadScene("Visual Feedback"); }
    private void LoadText() { FeedbackType = "Text"; SceneManager.LoadScene("Text Feedback"); }

    // Called by the "Continue"/"Finish" button shown after a run
    public void LoadWelcome()
    {
        // If they just finished their first round, immediately start the second round
        if (!isSecondRound)
        {
            isSecondRound = true;
            StartExperiment();   // go straight to the second game
        }
        else
        {
            // They finished both! Now reset for a completely NEW participant
            isSecondRound = false;
            
            // Flip the starter type for the next participant
            int currentStarter = PlayerPrefs.GetInt("StarterType", 1);
            PlayerPrefs.SetInt("StarterType", currentStarter == 1 ? 0 : 1);
            PlayerPrefs.Save();

            PlayerType = "Unknown";
            FeedbackType = "Unknown";
            ExperimentManager.Reset(); // Clear the floats for the next CSV row
            SceneManager.LoadScene("Welcome Page");
        }
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
    
    private void ResetMenu()
    {
        // Hide Start button
        if (startButton != null)
            startButton.gameObject.SetActive(false);

        // Re-enable player type selection buttons
        EnableTypeButtons();
    }
}