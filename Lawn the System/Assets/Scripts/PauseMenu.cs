using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public Button pauseButton;
    public Button resumeButton;
 
    public void Pause()
    {
        pauseUI.SetActive(true);
        pauseUI.GetComponent<CanvasGroup>().alpha = 1;
        pauseUI.GetComponent<CanvasGroup>().interactable = true;
        pauseUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        pauseUI.GetComponent<CanvasGroup>().alpha = -1;
        pauseUI.GetComponent<CanvasGroup>().interactable = false;
        pauseUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Time.timeScale = 1f;
    }
}
