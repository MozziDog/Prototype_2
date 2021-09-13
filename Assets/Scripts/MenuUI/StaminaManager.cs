using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StaminaManager : MonoBehaviour
{
    #region Stamina
    // 화면에 표시하기 위한 UI변수. NGUI가 있다면 사용가능
    // public UILabel appQuitTimeLabel = null;
    public Text StaminaRechargeTimer = null;
    public Text StaminaAmountLabel = null;

    private int m_StaminaAmount = 0; //보유 하트 개수
    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
    private const int MAX_STAMINA = 10; //하트 최대값
    public int StaminaRechargeInterval = 600;// 하트 충전 간격(단위:초)
    private Coroutine m_RechargeTimerCoroutine = null;
    private int m_RechargeRemainTime = 0;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
        Debug.Log("OnApplicationFocus() : " + value);
        if (value)
        {
            LoadStaminaInfo();
            LoadAppQuitTime();
            SetRechargeScheduler();
        }
        else
        {
            SaveStaminaInfo();
            SaveAppQuitTime();
        }
    }
    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        Debug.Log("GoodsRechargeTester: OnApplicationQuit()");
        SaveStaminaInfo();
        SaveAppQuitTime();
    }
    //버튼 이벤트에 이 함수를 연동한다.
    public void OnClickUseStamina()
    {
        Debug.Log("OnClickUseStamina");
        UseStamina();
    }

    public void Init()
    {
        m_StaminaAmount = 0;
        m_RechargeRemainTime = 0;
        m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
        Debug.Log("StaminaRechargeTimer : " + m_RechargeRemainTime + "s");
        SetStaminaRechargeTimer();
    }
    public bool LoadStaminaInfo()
    {
        Debug.Log("LoadStaminaInfo");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("StaminaAmount"))
            {
                Debug.Log("PlayerPrefs has key : StaminaAmount");
                m_StaminaAmount = PlayerPrefs.GetInt("StaminaAmount");
                if (m_StaminaAmount < 0)
                {
                    m_StaminaAmount = 0;
                }
            }
            else
            {
                m_StaminaAmount = MAX_STAMINA;
            }
            SetStaminaAmountLabel();
            Debug.Log("Loaded StaminaAmount : " + m_StaminaAmount);
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadStaminaInfo Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool SaveStaminaInfo()
    {
        Debug.Log("SaveStaminaInfo");
        bool result = false;
        try
        {
            PlayerPrefs.SetInt("StaminaAmount", m_StaminaAmount);
            PlayerPrefs.Save();
            Debug.Log("Saved StaminaAmount : " + m_StaminaAmount);
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveStaminaInfo Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool LoadAppQuitTime()
    {
        Debug.Log("LoadAppQuitTime");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("AppQuitTime"))
            {
                Debug.Log("PlayerPrefs has key : AppQuitTime");
                var appQuitTime = string.Empty;
                appQuitTime = PlayerPrefs.GetString("AppQuitTime");
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime));
            }
            Debug.Log(string.Format("Loaded AppQuitTime : {0}", m_AppQuitTime.ToString()));
            //appQuitTimeLabel.text = string.Format("AppQuitTime : {0}", m_AppQuitTime.ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool SaveAppQuitTime()
    {
        Debug.Log("SaveAppQuitTime");
        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();
            PlayerPrefs.SetString("AppQuitTime", appQuitTime);
            PlayerPrefs.Save();
            Debug.Log("Saved AppQuitTime : " + DateTime.Now.ToLocalTime().ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }
    public void SetRechargeScheduler(Action onFinish = null)
    {
        if (m_RechargeTimerCoroutine != null)
        {
            StopCoroutine(m_RechargeTimerCoroutine);
        }
        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds);
        Debug.Log("TimeDifference In Sec :" + timeDifferenceInSec + "s");
        var StaminaToAdd = timeDifferenceInSec / StaminaRechargeInterval;
        Debug.Log("Stamina to add : " + StaminaToAdd);
        var remainTime = timeDifferenceInSec % StaminaRechargeInterval;
        Debug.Log("RemainTime : " + remainTime);
        m_StaminaAmount += StaminaToAdd;
        if (m_StaminaAmount >= MAX_STAMINA)
        {
            m_StaminaAmount = MAX_STAMINA;
            SetStaminaRechargeTimer();
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
        }
        SetStaminaAmountLabel();
        Debug.Log("StaminaAmount : " + m_StaminaAmount);
    }
    public void UseStamina(Action onFinish = null)
    {
        if (m_StaminaAmount <= 0)
        {
            return;
        }

        m_StaminaAmount--;
        SetStaminaAmountLabel();
        if (m_RechargeTimerCoroutine == null)
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(StaminaRechargeInterval));
        }
        if (onFinish != null)
        {
            onFinish();
        }
    }
    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        Debug.Log("DoRechargeTimer");
        if (remainTime <= 0)
        {
            m_RechargeRemainTime = StaminaRechargeInterval;
        }
        else
        {
            m_RechargeRemainTime = remainTime;
        }
        Debug.Log("StaminaRechargeTimer : " + m_RechargeRemainTime + "s");
        SetStaminaRechargeTimer();

        while (m_RechargeRemainTime > 0)
        {
            Debug.Log("StaminaRechargeTimer : " + m_RechargeRemainTime + "s");
            SetStaminaRechargeTimer();
            m_RechargeRemainTime -= 1;
            yield return new WaitForSeconds(1f);
        }
        m_StaminaAmount++;
        if (m_StaminaAmount >= MAX_STAMINA)
        {
            m_StaminaAmount = MAX_STAMINA;
            m_RechargeRemainTime = 0;
            SetStaminaRechargeTimer();
            Debug.Log("StaminaAmount reached max amount");
            m_RechargeTimerCoroutine = null;
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(StaminaRechargeInterval, onFinish));
        }
        SetStaminaAmountLabel();
        Debug.Log("StaminaAmount : " + m_StaminaAmount);
    }

    void SetStaminaAmountLabel()
    {
        StaminaAmountLabel = (GameObject.Find("StaminaAmountLabel"))?.GetComponent<Text>();
        if (StaminaAmountLabel != null)
            StaminaAmountLabel.text = m_StaminaAmount.ToString();
    }

    void SetStaminaRechargeTimer()
    {
        StaminaRechargeTimer = (GameObject.Find("StaminaRechargeTimer"))?.GetComponent<Text>();
        if (StaminaRechargeTimer != null)
        {
            if (m_StaminaAmount >= MAX_STAMINA)
            {
                StaminaRechargeTimer.text = " ";
            }
            else
            {
                int min = m_RechargeRemainTime / 60;
                int second = m_RechargeRemainTime % 60;
                StaminaRechargeTimer.text = string.Format("{0:00}:{1:00}", min, second);
            }
        }
    }

    public int GetStaminaAmount()
    {
        return m_StaminaAmount;
    }
}
