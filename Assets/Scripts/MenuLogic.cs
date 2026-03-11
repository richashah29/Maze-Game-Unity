using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    public static string PlayerType = "Unknown"; 
    public static string FeedbackType = "Unknown";

    public void PickGamer() {PlayerType = "Gamer"; Debug.Log("Gamer Selected");}
    public void PickNonGamer() {PlayerType = "Non-Gamer"; Debug.Log("Non-Gamer Selected");}

    public void PickVisual() 
    { 
        FeedbackType = "Visual"; 
        SceneManager.LoadScene("Main"); 
    }

    public void PickText() 
    { 
        FeedbackType = "Text"; 
        SceneManager.LoadScene("Text Feedback"); 
    }
}