using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillInven : MonoBehaviour, IPointerClickHandler
{
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
            isEquipped = false;
            gameObject.transform.SetParent(_SelectGrid.transform);
            tap = 0;
            readyForDoubleTap = false;
        } 
        else if (!isEquipped && CheckEquip())
        {
            isEquipped = true;
            gameObject.transform.SetParent(_EquipGrid.transform);
            tap = 0;
            readyForDoubleTap = false;
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
}
