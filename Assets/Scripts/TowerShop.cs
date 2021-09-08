using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    public GameObject _shopButtonPrefab;
    public Inventory _inven;
    private readonly int MAX_SHOP_ITEMS = 5; // 진열될 수 있는 최대 갯수
    [SerializeField] MoneyManager _wallet;
    [SerializeField] GameObject _shopUI;
    // Button Instantiate할 부모 오브젝트
    [SerializeField] GameObject _shopButtonsGrid;
    // _towerOnList와 1대1 대응하는 토글 리스트
    [SerializeField] List<GameObject> _shopButtons;
    // 현재 상점에 올라와 있는 타워 리스트 (프리팹)
    [SerializeField] List<GameObject> _towerOnList;
    // 모든 타워 리스트
    [SerializeField] List<GameObject> _towerAll;


    // Start is called before the first frame update
    void Start()
    {
        MakeShoppingList();

    }

    public void ToggleUI()
    {
        _shopUI.SetActive(!_shopUI.activeSelf);
        OffToggle();
    }

    public void MakeShoppingList()
    {
        // 기존 쇼핑 리스트 클리어
        ClearShoppingList();
        // 랜덤 아이템 생성하여 상점에 진열
        for (int i = 0; i < MAX_SHOP_ITEMS; i++)
        {
            int selectedIndex = Random.Range(0, _towerAll.Count);
            AddShoppingItem(selectedIndex);
        }
    }

    private void OffToggle()
    {
        for (int i = 0; i < _shopButtons.Count; i++)
        {
            _shopButtons[i].GetComponent<Toggle>().isOn = false;
        }
    }

    private void AddShoppingItem(int towerCode)
    {
        _towerOnList.Add(_towerAll[towerCode]);
        GameObject newButton = Instantiate(_shopButtonPrefab);
        SetShoppingButton(newButton, towerCode);
        newButton.transform.SetParent(_shopButtonsGrid.transform, false);
        _shopButtons.Add(newButton);
    }

    private void SetShoppingButton(GameObject newButton, int towerCode)
    {
        // 초기화
        Toggle toggleComponent = newButton.GetComponent<Toggle>();
        toggleComponent.group = _shopButtonsGrid.GetComponent<ToggleGroup>();

        // towerIndex가 주어지면 그에 따라 상점 버튼 꾸미기
        // 임시로 label만 바꿔두겠습니다.
        Text label = newButton.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = _towerAll[towerCode].name;
        }
    }

    private void ClearShoppingList()
    {
        for (int i = 0; i < _shopButtons.Count; i++)
        {
            Destroy(_shopButtons[i]);
            _shopButtons.Clear();
        }
    }

    public void TryPurchase()
    {
        // 충분한 가루 / 인벤토리 공간이 있는 지 확인
        if (CheckEnoughMoney() == false)
        {
            // TODO: 가루가 모자랍니다 표시
            Debug.LogWarning("요술가루가 모자랍니다!");
            return;
        }
        if (CheckEnoughInventory() == false)
        {
            // TODO: 인벤토리가 가득 찼습니다 표시
            Debug.LogWarning("인벤토리가 가득 찼습니다!");
            return;
        }
        // 가능하다면 구매진행
        _wallet.SpendMoney(_towerOnList[GetSelectedItemIndex()].GetComponent<TowerBase>().GetPrice());
        _inven.AddItem(_towerOnList[GetSelectedItemIndex()]);
        DisableUsedButton();
        return;
    }

    private bool CheckEnoughMoney()
    {
        int price = _towerOnList[GetSelectedItemIndex()].GetComponent<TowerBase>().GetPrice();
        Debug.Log("price : " + price);
        if (price <= _wallet.GetLeftMoney())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckEnoughInventory()
    {
        return !_inven.isFull();
    }

    private void DisableUsedButton()
    {
        int selectedIndex = GetSelectedItemIndex();
        Toggle toggle = _shopButtons[selectedIndex].GetComponent<Toggle>();
        toggle.isOn = false;
        toggle.interactable = false;
    }

    private int GetSelectedItemIndex()
    {
        for (int i = 0; i < _shopButtons.Count; i++)
        {
            if (_shopButtons[i].GetComponent<Toggle>().isOn)
                return i;
        }
        return -1;
    }


}
