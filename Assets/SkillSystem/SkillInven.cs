using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillInven : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] public string skillName;
    [SerializeField] public string skillGrade;
    [SerializeField] int tap;
    [SerializeField] float interval = 1f;
    [SerializeField] bool readyForDoubleTap;

    [SerializeField] bool isEquipped;

    [SerializeField] GameObject _EquipGrid;
    [SerializeField] GameObject _SelectGrid;

    [SerializeField] GameObject _Info;

    [SerializeField] GameObject _equipBtn;
    [SerializeField] Text _equipTxt;

    [SerializeField] Text _coolTxt;
    float coolTime;

    public string info = "";


    public int _upgraded = 1;

    public int[] _UpgradeCost = { 300, 400, 500, 600, 800, 1000, 1200, 1400, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 8000, 0 };
    public void OnPointerClick(PointerEventData eventData)
    {
        tap++;
        if(tap == 1)
        {
            _equipBtn.SetActive(true);
            readyForDoubleTap = true;
            StartCoroutine(DoubleTapInterval());
        }
        else if(tap > 1 && readyForDoubleTap)
        {
            EquipChange();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(Input.GetMouseButtonDown(0))
            _equipBtn.SetActive(false);
    }

    IEnumerator DoubleTapInterval()
    {
        yield return new WaitForSeconds(interval);
        tap = 0;
        readyForDoubleTap = false;
    }

    public bool CheckEquip()
    {
        return _EquipGrid.transform.childCount < 4;
    }

    public void Upgrade()
    {
        _upgraded += 1;
        Debug.Log(_upgraded);
    }

    public void UpdateEquip()
    {
        for(int i = 0; i < _EquipGrid.transform.childCount; i++)
        {
            PlayerPrefs.SetString("selectedSkill" + (i + 1).ToString(), _EquipGrid.transform.GetChild(i).GetComponent<SkillInven>().skillName);
        }
        for(int j = _EquipGrid.transform.childCount; j < 4; j++)
        {
            PlayerPrefs.SetString("selectedSkill" + (j + 1).ToString(), "");
        }
    }

    public void UnEquip()
    {
        isEquipped = false;
        gameObject.transform.SetParent(_SelectGrid.transform);
        UpdateEquip();
        tap = 0;
        readyForDoubleTap = false;
    }

    public void EquipChange()
    {
        if(!isEquipped && CheckEquip())
        {
            isEquipped = true;
            gameObject.transform.SetParent(_EquipGrid.transform);
            UpdateEquip();
            tap = 0;
            readyForDoubleTap = false;
            _equipTxt.text = "해 제";
            _equipBtn.SetActive(false);

        }
        else if (isEquipped)
        {
            isEquipped = false;
            gameObject.transform.SetParent(_SelectGrid.transform);
            UpdateEquip();
            tap = 0;
            readyForDoubleTap = false;
            _equipTxt.text = "장 착";
            _equipBtn.SetActive(false);

        }
    }

    public void Setup()
    {
        isEquipped = true;
        gameObject.transform.SetParent(_EquipGrid.transform);
        tap = 0;
        readyForDoubleTap = false;
    }

    private void Start()
    {
        CalCool();
        _coolTxt.text = string.Format("{0:0.#}", coolTime) + "초";
    }

    public void CalCool()
    {
        if (skillName == "Meteor")
        {
            _upgraded = PlayerPrefs.GetInt("grade_Meteor");
            coolTime = 20f - (_upgraded - 1) * 5f / 19f;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "초";
            info = "스킬 시전시 하늘에서 불덩이들을 소환하여\n3초간 초당 " + string.Format("{0:0.#}", (2f + (_upgraded - 1) * 3f / 19f)) + "%의 데미지를 입힌다.";
        }
        else if (skillName == "Sacrifice")
        {
            _upgraded = PlayerPrefs.GetInt("grade_Sacrifice");
            coolTime = 2f;
            int _hpAmount = 0 ;
            if (_upgraded >= 1 && _upgraded < 6)
                _hpAmount = 5;
            else if (_upgraded < 13)
                _hpAmount = 6;
            else if (_upgraded <= 20)
                _hpAmount = 7;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "초";
            info = "체력을 " + _hpAmount.ToString() + "만큼 소모하여 " + string.Format("{0:0.#}", (10 + (_upgraded - 1) * 5)) + "만큼의 마나수정을 획득한다.";
        }
        else if (skillName == "Heal")
        {
            _upgraded = PlayerPrefs.GetInt("grade_Heal");
            coolTime = 2f;
            int _moneyAmount = 0;
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
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "초";
            info = "마나수정을 " + _moneyAmount + "만큼 소모하여 체력을 완전히 회복한다. \n단, 이 스킬은 스테이지 당 한번만 사용이 가능하다.";
        }
        else if (skillName == "PowerUp")
        {
            _upgraded = PlayerPrefs.GetInt("grade_PowerUp");
            coolTime = 15f - (+_upgraded - 1) * 2f / 19f;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "초";
            info = "스킬 범위 안의 타워를 4초 동안 공격력을 " + string.Format("{0:0.#}", (1f + (_upgraded - 1) * 3f / 19f)) + "%만큼 증가시킨다.";
        }
        else if (skillName == "Earthquake")
        {
            _upgraded = PlayerPrefs.GetInt("grade_Earthquake");
            coolTime = 35f - (_upgraded - 1) * 10f / 19f;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "초";
            info = "스킬을 꾹 누르는 동안 최대 " + string.Format("{0:0.#}", (1f + (_upgraded - 1) * 2f / 19f)) + "초만큼 지진을 일으킨다. \n오래 누를수록 쿨타임이 길어진다.";
        }
        else if (skillName == "LandSmash")
        {
            _upgraded = PlayerPrefs.GetInt("grade_LandSmash");
            coolTime = 30f - (_upgraded - 1) * 5f / 19f;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "초";
            info = "지면을 강하게 내리찍어 선택한 타일이 " + string.Format("{0:0.#}", (2f + (_upgraded - 1) * 2f / 19f)) +"초 동안 솟아 오른다.";
        }

    }

    private void Update()
    {
        if (this.GetComponent<Toggle>())
            if(this.GetComponent<Toggle>().isOn)
                _Info.GetComponent<SkillManager>().InfoSetting();

        if (this.GetComponent<Toggle>())
            if (!this.GetComponent<Toggle>().isOn)
                _equipBtn.SetActive(false);
    }

}
