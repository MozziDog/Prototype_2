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

    public int MAX_STAGE;
    public int MIN_STAGE;

    public int selectedChapter = 0;
    public int selectedStage = 0;
    public GameObject ChapterToggleGroup;
    public float tweenTime;
    public GameObject ShopUI;
    public GameObject ShopUI_movingPart;
    public GameObject InvenUI;
    public GameObject OptionUI;
    public GameObject CreditUI;
    [SerializeField] StaminaManager staminaManager;
    readonly int STAMINA_PER_STAGE = 10;
    public GameObject noStaminaWindow;
    bool tweenAnimationFinished = true;
    int screenWidth;

    public AudioClip clickSound;


    // Start is called before the first frame update
    void Start()
    {
        LoadSaveData();
        if (Global.userProperty.startStoryFinishFlag == false)
        {
            WatchStoryCutIn();
        }
        FadeIn();
        ShopUI_movingPart.transform.position = new Vector3(Screen.width, ShopUI_movingPart.transform.position.y, ShopUI_movingPart.transform.position.z);
        ShopUI.SetActive(false);
        screenWidth = Screen.width;
        if (Global.userProperty.LastReachedChapter != 1)
            for (int i = 0; i < Global.userProperty.LastReachedChapter - 1; i++)
            {
                Debug.Log("next");
                OnClickNextChapter();
            }
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

    public void TryStartStage()
    {
        Debug.LogWarning("TryStart...");
        if (selectedStage == 0)
        {
            Debug.Log("Stage not selected");
            return;
        }
        if (staminaManager.GetStaminaAmount() > STAMINA_PER_STAGE)
        {
            staminaManager.UseStamina(STAMINA_PER_STAGE);
            StartStage();
        }
        else
        {
            Debug.LogWarning("Stamina not enough");
            noStaminaWindow.SetActive(true);
            IEnumerator closeWindow()
            {
                yield return new WaitForSeconds(1f);
                noStaminaWindow.SetActive(false);
            }
            StartCoroutine(closeWindow());
        }
    }

    void StartStage()
    {
        Debug.Log(Global._chapter);
        Global.userProperty.LastReachedChapter = Global._chapter;
        GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().Save();
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
        if (tweenAnimationFinished)
        {
            // TODO: ChangeSelectedStage_Next 구현
            if (selectedChapter < MAX_STAGE)
            {
                selectedChapter++;
                resetToggleGroups();
                tweenAnimationFinished = false;
                DoTween(ChapterToggleGroup.transform, ChapterToggleGroup.transform.position, ChapterToggleGroup.transform.position - new Vector3(screenWidth, 0, 0), tweenTime);
                Debug.Log("다음 스테이지 선택");
            }
            else
            {
                //ChapterToggleGroup.transform.position = new Vector3(Screen.width, 0, 0);
                Debug.Log("마지막 스테이지입니다!");
            }
        }
    }

    public void OnClickBeforeChapter()
    {
        if (tweenAnimationFinished)
        {
            // TODO: ChangeSelectedStage_Before 구현
            if (selectedChapter > MIN_STAGE)
            {
                selectedChapter--;
                resetToggleGroups();
                tweenAnimationFinished = false;
                DoTween(ChapterToggleGroup.transform, ChapterToggleGroup.transform.position, ChapterToggleGroup.transform.position + new Vector3(screenWidth, 0, 0), tweenTime);
                Debug.Log("이전 스테이지 선택");
            }
            else
            {
                Debug.Log("첫 스테이지입니다!");
                //ChapterToggleGroup.transform.position = new Vector3(0, 0, 0);
            }
        }
    }

    void resetToggleGroups()
    {
        ToggleGroup[] toggleGroups = ChapterToggleGroup.GetComponentsInChildren<ToggleGroup>();
        foreach (var group in toggleGroups)
        {
            group.allowSwitchOff = true;
            group.SetAllTogglesOff();
        }
    }

    void DoTween(Transform transform, Vector3 originPosition, Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(Tween(targetPosition, tweenTime));
        IEnumerator Tween(Vector3 targetPosition, float time)
        {
            float startTime = Time.time;
            float nowTime = startTime;
            while (startTime + time > Time.time)
            {
                nowTime = Time.time;
                transform.position = new Vector3(
                    Mathf.Lerp(originPosition.x, targetPosition.x, (nowTime - startTime) / time),
                    Mathf.Lerp(originPosition.y, targetPosition.y, (nowTime - startTime) / time),
                    Mathf.Lerp(originPosition.z, targetPosition.z, (nowTime - startTime) / time));
                yield return 0;
            }
            transform.position = targetPosition;
            tweenAnimationFinished = true;
        }

    }

    public void OpenShopUI()
    {
        ShopUI.SetActive(true);
        DoTween(ShopUI_movingPart.transform, ShopUI_movingPart.transform.position, new Vector3(screenWidth / 2, ShopUI_movingPart.transform.position.y, ShopUI_movingPart.transform.position.z), tweenTime);
        CloseInventoryUI();
    }

    public void CloseShopUI()
    {
        IEnumerator disableWithDelay()
        {
            yield return new WaitForSeconds(tweenTime);
            ShopUI.SetActive(false);
        }

        DoTween(ShopUI_movingPart.transform, ShopUI_movingPart.transform.position, new Vector3(screenWidth * 3 / 2, ShopUI_movingPart.transform.position.y, ShopUI_movingPart.transform.position.z), tweenTime);
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

    public void OpenInventoryUI()
    {
        InvenUI.SetActive(true);
        CloseShopUI();
    }

    public void CloseInventoryUI()
    {
        InvenUI.SetActive(false);
    }

    public void OpenCreditWindow()
    {
        CreditUI.SetActive(true);
    }

    public void CloseCreditWindow()
    {
        CreditUI.SetActive(false);
    }

    public void WatchStoryCutIn()
    {
        SceneLoader.LoadScene("StoryCutIn");
    }

    public void PlayClickSound()
    {
        SFXManager.Play(clickSound);
    }
}
