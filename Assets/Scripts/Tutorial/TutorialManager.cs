using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialHighlight;
    public GameObject[] tutorialWindow;
    public GameObject _ShopUI;
    public Text tutorialText;
    int currentTutorialPage = 0;
    public bool alwaysDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // 인게임 화면 진입하기 전 메인 화면에서 이미 저장 데이터 로드되었다고 가정.
        if (Global.userProperty.TutorialFinishFlag == true && alwaysDisplay == false)
        {
            Destroy(gameObject);
        }
        else
        {
            DisplayTutorial();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DisplayTutorial()
    {
        tutorialWindow[0].SetActive(true);
    }

    void SetTutorialHighlightPosition(Vector3 position)
    {
        GameObject cam = GameObject.Find("Main Camera");
        tutorialHighlight.transform.position = cam.GetComponent<Camera>().WorldToScreenPoint(position);
    }

    public void OnClickNextButton()
    {
        tutorialWindow[currentTutorialPage].SetActive(false);
        currentTutorialPage++;
        if (currentTutorialPage >= tutorialWindow.Length)
        {
            // Tutorial finish
            Global.userProperty.TutorialFinishFlag = true;
            SaveLoadManager saveManager = GameObject.Find("SaveLoadManager")?.GetComponent<SaveLoadManager>();
            if (saveManager != null)
            {
                saveManager.Save();
                Debug.Log("Tutorial finish Saved");
            }
            else
            {
                Debug.LogWarning("Tutorial file save error");
            }
            _ShopUI.SetActive(true);
            Destroy(gameObject);
        }
        else
        {
            tutorialWindow[currentTutorialPage].SetActive(true);
        }
    }

    public void OnClickSkipButton()
    {
        // Tutorial finish
        Global.userProperty.TutorialFinishFlag = true;
        SaveLoadManager saveManager = GameObject.Find("SaveLoadManager")?.GetComponent<SaveLoadManager>();
        if (saveManager != null)
        {
            saveManager.Save();
            Debug.Log("Tutorial finish Saved");
        }
        else
        {
            Debug.LogWarning("Tutorial file save error");
        }
        _ShopUI.SetActive(true);
        Destroy(gameObject);
    }
}
