using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickPauseButton()
    {
        pauseUI.SetActive(true);
        PauseGame(true);
    }

    public void OnClickResumeButton()
    {
        pauseUI.SetActive(false);
        PauseGame(false);
    }
    void PauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void OnClickQuitButton()
    {
        Time.timeScale = 1;
        SceneLoader.LoadScene("StageSelectScene");
    }
}
