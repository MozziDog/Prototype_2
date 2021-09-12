using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sacrifice : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Image _cool_Img;
    [SerializeField] GameObject _cool_txt;

    [SerializeField] float coolTime = 3f;
    public bool isClicked = false;
    [SerializeField] float leftTime = 3f;

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
        _audio = GetComponent<AudioSource>();
        _moneyAmount += (_upgraded - 1) * _moneyUpgradeAmount;
        _hpAmount += (_upgraded - 1) * _hpUpgradeAmount;
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
