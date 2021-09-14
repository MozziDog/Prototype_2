using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text _UIText;
    public int _baseMoney;
    public int _LeftMoney;
    // Start is called before the first frame update

    private void Start()
    {
        _LeftMoney = _baseMoney;
        RefreshUI();
    }

    public int GetLeftMoney()
    {
        return _LeftMoney;
    }

    public void SetLeftMoney(int value)
    {
        _LeftMoney = value;
        RefreshUI();
    }

    public void SpendMoney(int value)
    {
        if (_LeftMoney < value)
        {
            // 오류 처리
            Debug.LogWarning("남은 돈이 부족합니다");
        }
        else
        {
            _LeftMoney -= value;
            RefreshUI();
        }
    }

    public void AddMoney(int value)
    {
        _LeftMoney += value;
        RefreshUI();
    }

    private void RefreshUI()
    {
        _UIText.text = _LeftMoney.ToString();
    }

}
