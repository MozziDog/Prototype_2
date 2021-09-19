using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text _UIText;
    public int leftMoney;
    [SerializeField] MoneyArray[] stageBaseMoney;
    [SerializeField] MoneyArray[] waveClearRewardMoney;
    // Start is called before the first frame update

    private void Start()
    {
        leftMoney = stageBaseMoney[Global._chapter - 1].value[Global._stage - 1];
        RefreshUI();
    }

    public int GetLeftMoney()
    {
        return leftMoney;
    }

    public void SetLeftMoney(int value)
    {
        leftMoney = value;
        RefreshUI();
    }

    public void SpendMoney(int value)
    {
        if (leftMoney < value)
        {
            // 오류 처리
            Debug.LogWarning("남은 돈이 부족합니다");
        }
        else
        {
            leftMoney -= value;
            RefreshUI();
        }
    }

    public void AddMoney(int value)
    {
        leftMoney += value;
        RefreshUI();
    }

    private void RefreshUI()
    {
        _UIText.text = leftMoney.ToString();
    }

    public void GiveWaveClearReward()
    {
        Debug.LogWarning("WaveClearReward");
        leftMoney += waveClearRewardMoney[Global._chapter - 1].value[Global._stage - 1];
        RefreshUI();
    }
}

[System.Serializable]
public class MoneyArray
{
    public int[] value;
}
