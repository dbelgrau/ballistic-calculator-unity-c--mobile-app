using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{
    #region Variables
    [Header("Message text")]
    [SerializeField] private Text errorMessage; // Text component for error messages
    [SerializeField] private Text infoMessage; // Text component for info messages

    [Header("Panels")]
    [SerializeField] private GameObject errorPanel; // Panel for displaying error messages
    [SerializeField] private GameObject infoPanel; // Panel for displaying info messages
    #endregion


    #region Message Handling
    public void ShowErrorMessage(string message)
    {
        // Display error message and activate error panel
        errorPanel.SetActive(true);
        errorMessage.text = message;
    }

    public void ShowInfoMessage(string message, bool autoHide = true, int seconds = 5)
    {
        // Display info message, activate info panel, and optionally auto-hide
        infoPanel.SetActive(true);
        infoMessage.text = message;

        if (autoHide) StartCoroutine(HideInfoPanel(seconds));
    }

    private IEnumerator HideInfoPanel(int seconds)
    {
        // Coroutine to hide info panel after specified time
        yield return new WaitForSeconds(seconds);
        infoPanel.SetActive(false);
    }

    public void HideInfoPanel()
    {
        // Hide info panel
        infoPanel.SetActive(false);
    }
    #endregion
}
