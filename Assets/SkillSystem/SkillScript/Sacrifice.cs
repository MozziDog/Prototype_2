using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sacrifice : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Image _cool_Img;
    [SerializeField] GameObject _cool_txt;
    [SerializeField] GameObject _Cool_Info_txt;

    [SerializeField] float coolTime;
    public bool isClicked = false;
    [SerializeField] float leftTime;

    [SerializeField] MoneyManager _moneyManager;
    [SerializeField] GameObject _sacrificeEffect;
    [SerializeField] GameObject _player;

    AudioSource _audio;

    [SerializeField] float _hpAmount;
    [SerializeField] float _hpUpgradeAmount;
    [SerializeField] int _moneyAmount;
    [SerializeField] int _moneyUpgradeAmount;
    [SerializeField] int _upgraded = 1;


    private void Start()
    {
        _upgraded = PlayerPrefs.GetInt("grade_Sacrifice");
        _audio = GetComponent<AudioSource>();
        _moneyAmount = 10 + (_upgraded - 1) * 5;
        if (_upgraded >= 1 && _upgraded < 6)
            _hpAmount = 5;
        else if (_upgraded < 13)
            _hpAmount = 6;
        else if (_upgraded <= 20)
            _hpAmount = 7;

        _Cool_Info_txt.GetComponent<Text>().text = string.Format("{0:0.#}", coolTime) + "초";
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
                _cool_txt.GetComponent<Text>().text = Mathf.Floor(leftTime).ToString() + " 초";
            }
    }

    public void StartCoolTime()
    {
        _audio.Play();
        leftTime = coolTime;
        isClicked = true;
        if (_button)
            _button.enabled = false;
        _cool_txt.SetActive(true);
    }

    public void SacrificeSkill()
    {
        if (_player.GetComponent<Player>().currentHP <= _hpAmount)
            return;
        StartCoolTime();
        _player.GetComponent<Player>().StartGetHit(_hpAmount);
        _moneyManager.AddMoney(_moneyAmount);
        Instantiate(_sacrificeEffect, _player.transform);
    }
}
