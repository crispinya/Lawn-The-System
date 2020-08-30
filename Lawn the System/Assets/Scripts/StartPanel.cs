using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public Button firstButton;
    public GameObject startPanel;

    public void ResumeFirstGame()
    {
        startPanel.GetComponent<CanvasGroup>().alpha = -1;
        startPanel.GetComponent<CanvasGroup>().interactable = false;
        startPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Time.timeScale = 1f;
    }
}
