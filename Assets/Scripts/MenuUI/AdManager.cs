using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public void OnClickCloseButton()
    {
        SceneManager.UnloadSceneAsync("Ad");
    }

    public void OnClickAdButton0()
    {
        TryShowAd(() =>
        {
            Global.userProperty.ruby += 50;
            GameObject.Find("StageSelectManager").GetComponent<StageSelectSceneManager>().RefreshUserPropertyData();
            GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().Save();
        });
    }

    public void OnClickAdButton1()
    {
        TryShowAd(() =>
        {
            Global.userProperty.gold += 2000;
            GameObject.Find("StageSelectManager").GetComponent<StageSelectSceneManager>().RefreshUserPropertyData();
            GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().Save();
        });
    }

    public void OnClickAdButton2()
    {
        TryShowAd(() =>
        {
            StaminaManager sm = GameObject.Find("StaminaManager")?.GetComponent<StaminaManager>();
            sm.AddStamina(5);
            GameObject.Find("StageSelectManager").GetComponent<StageSelectSceneManager>().RefreshUserPropertyData();
            GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().Save();
        });
    }

    public void ShowAd()
    {
        SceneManager.LoadScene("Ad", LoadSceneMode.Additive);
    }

    #region Ad
    // 화면에 표시하기 위한 UI변수. NGUI가 있다면 사용가능
    // public UILabel appQuitTimeLabel = null;
    public Text rechargeTimer = null;
    public Image readyImage = null;
    public int AdContentID;
    private DateTime m_LastUpdateTime = new DateTime(1970, 1, 1).ToLocalTime();
    public int rechargeInterval = 600;// 하트 충전 간격(단위:초)
    private Coroutine m_RechargeTimerCoroutine = null;
    private int m_RechargeRemainTime = 0;
    private int isReady;

    #endregion

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        //SetStaminaAmountLabel();
        OnApplicationFocus(true);
    }
    //게임 초기화, 중간 이탈, 중간 복귀 시 실행되는 함수
    public void OnApplicationFocus(bool value)
    {
        // Debug.LogWarning("OnApplicationFocus() : " + value);
        if (value)
        {
            LoadAdInfo();
            LoadLastModifiedTime();
            SetRechargeScheduler();
        }
        else
        {
            SaveAdInfo();
            SaveLastModifiedTime();
        }
    }
    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        //Debug.LogWarning("GoodsRechargeTester: OnApplicationQuit()");
        SaveAdInfo();
        SaveLastModifiedTime();
    }
    //버튼 이벤트에 이 함수를 연동한다.
    public void OnClickViewAd()
    {
        //Debug.LogWarning("OnClickViewAd");
        TryShowAd();
    }

    public void Init()
    {
        isReady = 0;
        m_RechargeRemainTime = 0;
        m_LastUpdateTime = new DateTime(1970, 1, 1).ToLocalTime();
        SetRechargeTimer();
    }
    public bool LoadAdInfo()
    {
        Debug.Log("LoadAdInfo");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("AdReady" + AdContentID))
            {
                //Debug.LogWarning("PlayerPrefs has key : AdReady" + AdContentID);
                isReady = PlayerPrefs.GetInt("AdReady" + AdContentID);
                if (isReady < 0)
                {
                    isReady = 0;
                }
            }
            else
            {
                isReady = 1;
            }
            SetAdReadyImage();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadAdInfo Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool SaveAdInfo()
    {
        Debug.LogWarning("SaveAdInfo");
        bool result = false;
        try
        {
            PlayerPrefs.SetInt("AdReady" + AdContentID, isReady);
            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveStaminaInfo Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool LoadLastModifiedTime()
    {
        //Debug.LogWarning("LoadLastModifiedTime");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("LastAdUpdate" + AdContentID))
            {
                //Debug.LogWarning("PlayerPrefs has key : LastAdUpdate" + AdContentID);

                var lastUpdateTime = PlayerPrefs.GetString("LastAdUpdate" + AdContentID);
                m_LastUpdateTime = DateTime.FromBinary(Convert.ToInt64(lastUpdateTime));
            }
            Debug.Log(string.Format("Loaded LastUpdate : {0}", m_LastUpdateTime.ToString()));
            //appQuitTimeLabel.text = string.Format("AppQuitTime : {0}", m_AppQuitTime.ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadLastModifiedTime Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool SaveLastModifiedTime()
    {
        //Debug.LogWarning("SaveLastModifiedTime");
        bool result = false;
        try
        {
            var lastUpdateTime = m_LastUpdateTime.ToBinary().ToString();
            PlayerPrefs.SetString("LastAdUpdate" + AdContentID, lastUpdateTime);
            PlayerPrefs.Save();
            Debug.Log("Saved LastAdUpdate : " + m_LastUpdateTime.ToLocalTime().ToString());

            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveLastModifiedTime Failed (" + e.Message + ")");
        }
        return result;
    }
    // 디버깅용
    DateTime nowTime;
    public void SetRechargeScheduler(Action onFinish = null)
    {
        //Debug.LogWarning("SetRechargeScheduler");
        if (m_RechargeTimerCoroutine != null)
        {
            StopCoroutine(m_RechargeTimerCoroutine);
        }
        nowTime = DateTime.Now.ToLocalTime();
        //Debug.LogWarning("nowTime Sec :" + nowTime.Second + "s");
        //Debug.LogWarning("lastAdUpdate Sec :" + m_LastUpdateTime.Second + "s");
        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_LastUpdateTime).TotalSeconds);
        //Debug.LogWarning("TimeDifference In Sec :" + timeDifferenceInSec + "s");
        // var StaminaToAdd = timeDifferenceInSec / rechargeInterval;
        // var remainTime = timeDifferenceInSec % rechargeInterval;
        isReady = timeDifferenceInSec > rechargeInterval ? 1 : 0;
        var remainTime = timeDifferenceInSec > rechargeInterval ? 0 : timeDifferenceInSec;
        //Debug.LogWarning("RemainTime : " + remainTime);
        if (isReady >= 1)
        {
            isReady = 1;
            m_LastUpdateTime = DateTime.Now.ToLocalTime();
            SaveLastModifiedTime();
            SetRechargeTimer();
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(rechargeInterval - remainTime, onFinish));
        }
        SetAdReadyImage();
        //Debug.LogWarning("StaminaAmount : " + isReady);
    }
    public void TryShowAd(Action onFinish = null)
    {
        if (isReady <= 0)
        {
            return;
        }

        isReady--;
        SaveAdInfo();
        m_LastUpdateTime = DateTime.Now.ToLocalTime();
        SaveLastModifiedTime();
        ShowAd();
        SetAdReadyImage();
        if (m_RechargeTimerCoroutine == null)
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(rechargeInterval));
        }
        if (onFinish != null)
        {
            onFinish();
        }
    }
    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        //Debug.LogWarning("DoRechargeTimer");
        if (remainTime <= 0)
        {
            m_RechargeRemainTime = rechargeInterval;
        }
        else
        {
            m_RechargeRemainTime = remainTime;
        }
        //SetRechargeTimer();

        while (m_RechargeRemainTime > 0)
        {
            SetRechargeTimer();
            m_RechargeRemainTime -= 1;
            yield return new WaitForSeconds(1f);
            //Debug.LogWarning("recharge tick");
        }
        isReady = 1;
        SaveAdInfo();

        m_LastUpdateTime = DateTime.Now.ToLocalTime();
        SaveLastModifiedTime();

        m_RechargeRemainTime = 0;
        SetRechargeTimer();
        m_RechargeTimerCoroutine = null;
        SetAdReadyImage();
    }

    void SetAdReadyImage()
    {
        if (readyImage != null)
            readyImage.enabled = isReady > 0 ? true : false;
    }

    void SetRechargeTimer()
    {
        //Debug.LogWarning("SetRechargeTimer : " + m_RechargeRemainTime);
        if (rechargeTimer != null)
        {
            if (isReady >= 1)
            {
                rechargeTimer.text = "받기!";
            }
            else
            {
                int min = m_RechargeRemainTime / 60;
                int second = m_RechargeRemainTime % 60;
                rechargeTimer.text = string.Format("{0:00}:{1:00}", min, second);
            }
        }
    }
}
