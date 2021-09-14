using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectSceneManager : MonoBehaviour
{
    public GameObject fader;
    public Text goldText;
    public Text RubyText;
    public Text StaminaText;

    public int MAX_STAGE;
    public int MIN_STAGE;

    public int selectedChapter = 0;
    public int selectedStage = 0;
    public GameObject ChapterToggleGroup;
    public float tweenTime;
    public GameObject ShopUI;
    public GameObject ShopUI_movingPart;
    public GameObject OptionUI;


    // Start is called before the first frame update
    void Start()
    {
        LoadSaveData();
        FadeIn();
        ShopUI_movingPart.transform.position = new Vector3(Screen.width, ShopUI_movingPart.transform.position.y, ShopUI_movingPart.transform.position.z);
        ShopUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadSaveData()
    {
        // TODO: Save & Load 구현
        GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().Load();
        RefreshUserPropertyData();
    }

    public void RefreshUserPropertyData()
    {
        goldText.text = Global.userProperty.gold.ToString();
        RubyText.text = Global.userProperty.ruby.ToString();
        StaminaText.text = Global.userProperty.stamina.ToString();
    }

    Coroutine fadeInCoroutine = null;
    public void FadeIn()
    {
        fader.SetActive(true);
        IEnumerator _Fade()
        {
            for (int i = 5; i >= 0; i--)
            {
                fader.GetComponent<Image>().color = new Color(0, 0, 0, i / 5f);
                yield return new WaitForSeconds(0.05f);
            }
            fadeInCoroutine = null;
            fader.SetActive(false);
        }
        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        fadeInCoroutine = StartCoroutine(_Fade());
    }

    Coroutine fadeOutCoroutine = null;
    public void FadeOut()
    {
        fader.SetActive(true);
        IEnumerator _Fade()
        {
            for (int i = 0; i < 6; i++)
            {
                fader.GetComponent<Image>().color = new Color(0, 0, 0, i / 5f);
                yield return new WaitForSeconds(0.05f);
            }
            fadeOutCoroutine = null;
        }
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        fadeOutCoroutine = StartCoroutine(_Fade());
    }

    public void StartStage()
    {
        string SceneString = GetSelectedStage();
        //SceneManager.LoadScene("Stage" + stageCode);
        SceneLoader.LoadScene(SceneString);
    }

    string GetSelectedStage()
    {
        SetStageData();
        // TODO: GetSelectedStage 구현
        // return "MainScene_" + selectedChapter.toString();
        return "SampleScene_TH";
    }

    void SetStageData()
    {
        // TODO : Global 클래스를 통해 인게임 씬에 스테이지 정보 전달 구현
    }

    public void OnClickNextChapter()
    {
        // TODO: ChangeSelectedStage_Next 구현
        if (selectedChapter < MAX_STAGE)
        {
            selectedChapter++;
            DoTween(ChapterToggleGroup.transform, ChapterToggleGroup.transform.position - new Vector3(Screen.width, 0, 0), tweenTime);
            Debug.Log("다음 스테이지 선택");
        }
        else
        {
            Debug.Log("마지막 스테이지입니다!");
        }
    }

    public void OnClickBeforeChapter()
    {
        // TODO: ChangeSelectedStage_Before 구현
        if (selectedChapter > MIN_STAGE)
        {
            selectedChapter--;
            DoTween(ChapterToggleGroup.transform, ChapterToggleGroup.transform.position + new Vector3(Screen.width, 0, 0), tweenTime);
            Debug.Log("이전 스테이지 선택");
        }
        else
        {
            Debug.Log("첫 스테이지입니다!");
        }
    }

    void DoTween(Transform originTransform, Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(Tween(targetPosition, tweenTime));
        IEnumerator Tween(Vector3 targetPosition, float time)
        {
            Vector3 originPosition = originTransform.position;
            float startTime = Time.time;
            float nowTime = startTime;
            while (startTime + time > Time.time)
            {
                nowTime = Time.time;
                originTransform.position = new Vector3(
                    Mathf.Lerp(originPosition.x, targetPosition.x, (nowTime - startTime) / time),
                    Mathf.Lerp(originPosition.y, targetPosition.y, (nowTime - startTime) / time),
                    Mathf.Lerp(originPosition.z, targetPosition.z, (nowTime - startTime) / time));
                yield return 0;
            }
        }

    }

    public void OpenShopUI()
    {
        ShopUI.SetActive(true);
        DoTween(ShopUI_movingPart.transform, new Vector3(Screen.width / 2, ShopUI_movingPart.transform.position.y, ShopUI_movingPart.transform.position.z), tweenTime);
    }

    public void CloseShopUI()
    {
        IEnumerator disableWithDelay()
        {
            yield return new WaitForSeconds(tweenTime);
            ShopUI.SetActive(false);
        }

        DoTween(ShopUI_movingPart.transform, new Vector3(Screen.width * 3 / 2, ShopUI_movingPart.transform.position.y, ShopUI_movingPart.transform.position.z), tweenTime);
        StartCoroutine(disableWithDelay());
    }

    public void OpenOptionUI()
    {
        OptionUI.SetActive(true);
    }

    public void CloseOptionUI()
    {
        OptionUI.SetActive(false);
    }
}
