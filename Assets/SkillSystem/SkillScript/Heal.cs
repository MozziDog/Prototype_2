using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heal : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Image _cool_Img;
    [SerializeField] GameObject _cool_txt;
    [SerializeField] GameObject _Cool_Info_txt;

    [SerializeField] float coolTime = 2f;
    public bool isClicked = false;
    [SerializeField] float leftTime = 2f;

    [SerializeField] MoneyManager _moneyManager;
    [SerializeField] GameObject _healEffect;
    [SerializeField] GameObject _player;

    AudioSource _audio;

    [SerializeField] int _moneyAmount;
    [SerializeField] int _moneyUpgradeAmount;
    //[SerializeField] float _healUpgradeAmount;
    [SerializeField] int _upgraded = 1;
    public int _usage = 1;

    private void Start()
    {
        _upgraded = PlayerPrefs.GetInt("grade_Heal");
        _audio = GetComponent<AudioSource>();
        if (_upgraded >= 1 && _upgraded < 6)
            _moneyAmount = 150;
        else if (_upgraded < 11)
            _moneyAmount = 130;
        else if (_upgraded < 16)
            _moneyAmount = 120;
        else if (_upgraded < 20)
            _moneyAmount = 110;
        else if (_upgraded == 20)
            _moneyAmount = 100;
        //_healAmount += (_upgraded - 1) * _healUpgradeAmount;
        _Cool_Info_txt.GetComponent<Text>().text = string.Format("{0:0.#}", coolTime) + "초";
        _player = GameObject.Find("Player1");
        _moneyManager = GameObject.Find("InGameShopManager").GetComponent<MoneyManager>();
        _usage = 1;
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
                _cool_txt.GetComponent<Text>().text = Mathf.Floor(leftTime).ToString() + "초";
            }
    }

    public void StartCoolTime()
    {
        leftTime = coolTime;
        isClicked = true;
        _audio.Play();
        if (_button)
            _button.enabled = false;
        _cool_txt.SetActive(true);
    }

    public void HealSkill()
    {
        if (_moneyManager.GetLeftMoney() < _moneyAmount || _player.GetComponent<Player>().currentHP >= _player.GetComponent<Player>().maxHP || _usage <= 0)
            return;
        _usage = 0;
        StartCoolTime();
        _moneyManager.SpendMoney(_moneyAmount);
        _player.GetComponent<Player>().getHealed(_player.GetComponent<Player>().maxHP);
        Instantiate(_healEffect, _player.transform);
    }
}
