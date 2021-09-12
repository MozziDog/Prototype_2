using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Earthquake : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Image _cool_Img;
    [SerializeField] GameObject _cool_txt;

    [SerializeField] float coolTime = 3f;
    public bool isClicked = false;
    [SerializeField] float leftTime = 3f;

    [SerializeField] MoneyManager _moneyManager;
    [SerializeField] GameObject _healEffect;
    [SerializeField] GameObject _player;

    [SerializeField] int _moneyAmount;
    [SerializeField] int _moneyUpgradeAmount;
    [SerializeField] float _healAmount;
    //[SerializeField] float _healUpgradeAmount;
    [SerializeField] int _upgraded = 1;

    private void Start()
    {
        _moneyAmount -= (_upgraded - 1) * _moneyUpgradeAmount;
        //_healAmount += (_upgraded - 1) * _healUpgradeAmount;
        _player = GameObject.Find("Player1");
        _moneyManager = GameObject.Find("InGameShopManager").GetComponent<MoneyManager>();
    }

    private void Update()
    {
        if (isClicked)
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                if (leftTime <= 0)
                {
                    _cool_txt.SetActive(false);
                    leftTime = 0f;
                    if (_button)
                        _button.enabled = true;
                    isClicked = true;
                }

                float ratio = leftTime / coolTime;
                if (_cool_Img)
                {
                    _cool_Img.fillAmount = ratio;
                }
                _cool_txt.GetComponent<Text>().text = Mathf.Floor(leftTime).ToString() + " √ ";
            }
    }

    public void StartCoolTime()
    {
        leftTime = coolTime;
        isClicked = true;
        if (_button)
            _button.enabled = false;
        _cool_txt.SetActive(true);
    }

    public void HealSkill()
    {
        if (_moneyManager.GetLeftMoney() < _moneyAmount || _player.GetComponent<Player>().currentHP >= _player.GetComponent<Player>().maxHP)
            return;
        StartCoolTime();
        _moneyManager.SpendMoney(_moneyAmount);
        _player.GetComponent<Player>().getHealed(_healAmount);
        Instantiate(_healEffect, _player.transform);
    }
}
