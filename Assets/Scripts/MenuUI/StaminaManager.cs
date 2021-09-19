using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StaminaManager : MonoBehaviour
{
    #region Stamina
    // 화면에 표시하기 위한 UI변수. NGUI가 있다면 사용가능
    // public UILabel LastModifiedTimeLabel = null;
    public Text StaminaRechargeTimer = null;
    public Text StaminaAmountLabel = null;

    private int m_StaminaAmount = 0; //보유 하트 개수
    private DateTime m_LastModifiedTime = new DateTime(1970, 1, 1).ToLocalTime();
    private const int MAX_STAMINA = 100; //하트 최대값
    public int StaminaRechargeInterval = 600;// 하트 충전 간격(단위:초)
    private Coroutine m_RechargeTimerCoroutine = null;
    private int m_RechargeRemainTime = 0;
    #endregion

    private void Awake()
    {
        Init();
        //DontDestroyOnLoad(gameObject);
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
            LoadLastModifiedTime();
            SetRechargeScheduler();
        }
        else
        {
            SaveStaminaInfo();
            SaveLastModifiedTime();
        }
    }
    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        Debug.Log("GoodsRechargeTester: OnApplicationQuit()");
        SaveStaminaInfo();
        SaveLastModifiedTime();
    }
    /*
    //버튼 이벤트에 이 함수를 연동한다.
    public void OnClickUseStamina()
    {
        Debug.Log("OnClickUseStamina");
        UseStamina();
    }
    */

    void SetLastModifiedTime()
    {
        m_LastModifiedTime = DateTime.Now.ToLocalTime();
    }

    public void Init()
    {
        m_StaminaAmount = 0;
        m_RechargeRemainTime = 0;
        m_LastModifiedTime = new DateTime(1970, 1, 1).ToLocalTime();
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
                    SetLastModifiedTime();
                }
            }
            else
            {
                m_StaminaAmount = MAX_STAMINA;
                SetLastModifiedTime();
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
    public bool LoadLastModifiedTime()
    {
        Debug.Log("LoadLastModifiedTime");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("LastModifiedTime"))
            {
                Debug.Log("PlayerPrefs has key : LastModifiedTime");
                var LastModifiedTime = string.Empty;
                LastModifiedTime = PlayerPrefs.GetString("LastModifiedTime");
                m_LastModifiedTime = DateTime.FromBinary(Convert.ToInt64(LastModifiedTime));
            }
            Debug.Log(string.Format("Loaded LastModifiedTime : {0}", m_LastModifiedTime.ToString()));
            //LastModifiedTimeLabel.text = string.Format("LastModifiedTime : {0}", m_LastModifiedTime.ToString());
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
        Debug.Log("SaveLastModifiedTime");
        bool result = false;
        try
        {
            var LastModifiedTime = m_LastModifiedTime.ToLocalTime().ToBinary().ToString();
            PlayerPrefs.SetString("LastModifiedTime", LastModifiedTime);
            PlayerPrefs.Save();
            Debug.Log("Saved LastModifiedTime : " + m_LastModifiedTime.ToLocalTime().ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveLastModifiedTime Failed (" + e.Message + ")");
        }
        return result;
    }
    public void SetRechargeScheduler(Action onFinish = null)
    {
        if (m_RechargeTimerCoroutine != null)
        {
            StopCoroutine(m_RechargeTimerCoroutine);
        }
        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_LastModifiedTime).TotalSeconds);
        Debug.Log("TimeDifference In Sec :" + timeDifferenceInSec + "s");
        var StaminaToAdd = timeDifferenceInSec / StaminaRechargeInterval;
        Debug.Log("Stamina to add : " + StaminaToAdd);
        var remainTime = timeDifferenceInSec % StaminaRechargeInterval;
        Debug.Log("RemainTime : " + remainTime);
        if (m_StaminaAmount < MAX_STAMINA)
        {
            m_StaminaAmount += StaminaToAdd;
            if (m_StaminaAmount >= MAX_STAMINA)
                m_StaminaAmount = MAX_STAMINA;
        }
        SetLastModifiedTime();
        if (m_StaminaAmount >= MAX_STAMINA)
        {
            SetLastModifiedTime();
            SetStaminaRechargeTimer();
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(StaminaRechargeInterval - remainTime, onFinish));
        }
        SetStaminaAmountLabel();
        Debug.Log("StaminaAmount : " + m_StaminaAmount);
    }
    public void UseStamina(int usedStamina, Action onFinish = null)
    {
        if (m_StaminaAmount < usedStamina)
        {
            return;
        }

        m_StaminaAmount -= usedStamina;
        SetStaminaAmountLabel();
        SaveStaminaInfo();
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
        SetLastModifiedTime();
        if (m_StaminaAmount >= MAX_STAMINA)
        {
            SetLastModifiedTime();
            m_RechargeRemainTime = 0;
            SetStaminaRechargeTimer();
            Debug.Log("StaminaAmount reached max amount");
            m_RechargeTimerCoroutine = null;
        }
        else
        {
            m_StaminaAmount += 1;
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

    public void AddStamina(int value)
    {
        m_StaminaAmount += value;
        SetStaminaRechargeTimer();
        StaminaAmountLabel.text = m_StaminaAmount.ToString();
        PlayerPrefs.SetInt("StaminaAmount", m_StaminaAmount);
        PlayerPrefs.Save();
    }
}

// 코드 출처 : https://tenlie10.tistory.com/177
// (코드는 일부 수정됨)
