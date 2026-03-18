using UnityEngine;
using TMPro;

public class FinishButtonLabel : MonoBehaviour
{
    private TMP_Text buttonText;

    void Awake()
    {
        // Grab the text component on this object or its children
        buttonText = GetComponent<TMP_Text>();
        if (buttonText == null)
            buttonText = GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        if (buttonText == null) return;

        // First round: show "Continue", second round: show "Finish"
        buttonText.text = MenuLogic.IsSecondRound ? "Finish" : "Continue";
    }
}

