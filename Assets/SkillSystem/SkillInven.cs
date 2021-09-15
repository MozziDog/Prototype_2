using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillInven : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public string skillName;
    [SerializeField] int tap;
    [SerializeField] float interval = 1f;
    [SerializeField] bool readyForDoubleTap;

    [SerializeField] bool isEquipped;

    [SerializeField] GameObject _EquipGrid;
    [SerializeField] GameObject _SelectGrid;

    [SerializeField] GameObject _Info;

    [SerializeField] static int _upgraded = 1;

    public int[] _UpgradeCost = { 300, 400, 500, 600, 800, 1000, 1200, 1400, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 8000 };
    public void OnPointerClick(PointerEventData eventData)
    {
        tap++;
        if(tap == 1)
        {
            readyForDoubleTap = true;
            StartCoroutine(DoubleTapInterval());
        }
        else if(tap > 1 && readyForDoubleTap && isEquipped)
        {
            UnEquip();
        } 
        else if (!isEquipped && CheckEquip())
        {
            Equip();
        }
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

    public void Equip()
    {
        isEquipped = true;
        gameObject.transform.SetParent(_EquipGrid.transform);
        UpdateEquip();
        tap = 0;
        readyForDoubleTap = false;
    }

    public void Setup()
    {
        isEquipped = true;
        gameObject.transform.SetParent(_EquipGrid.transform);
        tap = 0;
        readyForDoubleTap = false;
    }

}
