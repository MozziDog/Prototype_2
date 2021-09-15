using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;


// TODO : InvenToggle 클래스 활용하여 리팩토링
public class Inventory : MonoBehaviour
{
    public int MAX_INVENTORY_CAPACITY = 5; // 임의로 5로 지정해두겠습니다.
    public TowerManager _towerManager;
    public List<Toggle> _toggle;
    public List<GameObject> _tower;

    [SerializeField] GameObject _toggleGrid;
    [SerializeField] GameObject _togglePrefab;


    private void Start()
    {
        OffToggle();
    }

    public void OffToggle()
    {
        for (int i = 0; i < _toggle.Count; i++)
        {
            _toggle[i].isOn = false;
        }
    }

    public GameObject GetSelectedTower()
    {
        return _tower[GetActiveToggleIndex()];
    }

    private int GetActiveToggleIndex()
    {
        for (int i = 0; i < _toggle.Count; i++)
        {
            if (_toggle[i].isOn)
                return i;
        }
        return -1;
    }

    public void AddItem(GameObject tower)
    {
        GameObject toggle = Instantiate(_togglePrefab);
        toggle.transform.SetParent(_toggleGrid.transform, false);
        _toggle.Add(toggle.GetComponent<Toggle>());
        SetToggleContents(toggle, tower);

        _tower.Add(tower);
    }

    private void SetToggleContents(GameObject toggle, GameObject tower)
    {
        Toggle toggleComponent = toggle.GetComponent<Toggle>();
        toggleComponent.group = _toggleGrid.GetComponent<ToggleGroup>();
        UnityAction<bool> sendMessage = delegate (bool isSelected)
        {
            if (isSelected == true)
                _towerManager.SendMessage("OnInventoryItemSelected");
        };
        toggleComponent.onValueChanged.AddListener(sendMessage);
        // inventory 내 TowerInfo window 구현
        InvenToggle invenToggle = toggle.GetComponent<InvenToggle>();
        invenToggle.towerPrefab = tower;

        TowerBase towerComponent = tower.GetComponent<TowerBase>();
        Text label = toggle.GetComponentInChildren<Text>();
        label.text = towerComponent.type + "타워";
        Image towerImage = toggle.transform.Find("Preview").GetComponent<Image>();
        towerImage.sprite = towerComponent.towerImage;
        Image pentominoImage = toggle.transform.Find("Pentomino").GetComponent<Image>();
        pentominoImage.sprite = towerComponent.pentominoImage;
    }

    public void DeleteSelectedItem()
    {
        int selectedIndex = GetActiveToggleIndex();
        DestoryToggle(selectedIndex);
        DeleteSelectedTower(selectedIndex);
    }

    public void DestoryToggle(int selectedIndex)
    {
        if (selectedIndex != -1)
        {
            _toggle[selectedIndex].isOn = false;
            Destroy(_toggle[selectedIndex].gameObject);
            _toggle.RemoveAt(selectedIndex);
            _towerManager.towerToSpawn = null;
            return;
        }
    }

    public void DeleteSelectedTower(int selectedIndex)
    {
        _tower.RemoveAt(selectedIndex);
    }

    public void SetToggleInteractable(bool interactable)
    {
        for (int i = 0; i < _toggle.Count; i++)
        {
            _toggle[i].interactable = interactable;
        }
    }

    public bool isFull()
    {
        return _toggle.Count >= MAX_INVENTORY_CAPACITY;
    }

    public int GetItemCount()
    {
        return _toggle.Count;
    }

    public void OnItemUnselected()
    {
        _towerManager.SendMessage("OnInventoryItemUnselected");
    }
}
