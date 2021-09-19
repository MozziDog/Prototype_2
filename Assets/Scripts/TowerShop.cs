using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    public GameObject _shopButtonPrefab;
    public Inventory _inven;
    private readonly int MAX_SHOP_ITEMS = 5; // 진열될 수 있는 최대 갯수
    [SerializeField] TowerManager _towerManager;
    [SerializeField] MoneyManager _wallet;
    // 상점 창
    [SerializeField] GameObject _shopUI;
    // Button Instantiate할 부모 오브젝트
    [SerializeField] GameObject _shopButtonsGrid;
    // 상점 아이템
    public List<ShopItem> _shopItems;
    [SerializeField] [ReadOnly] List<ShopItem> _shoppingCart;
    // 모든 타워 리스트
    [SerializeField] List<GameObject> _towerAll;
    [SerializeField] Text _totalPriceText;
    [SerializeField] int rerollPrice;

    public class ShopItem
    {
        public GameObject _shopButton;
        public GameObject _towerPrefab;
        public bool isChecked = false;
        public int GetPrice()
        {
            return _towerPrefab.GetComponent<TowerBase>().GetPrice();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // 할당
        _shopItems = new List<ShopItem>();
        _shoppingCart = new List<ShopItem>();

        MakeShoppingList();
        if (_shopUI.activeSelf == false)
        {
            ToggleUI();
        }
    }

    public void ToggleUI()
    {
        _shopUI.SetActive(!_shopUI.activeSelf);
        OffToggles();
        _inven.OnItemUnselected();
        _towerManager.OnTowerUnselected();
    }

    private void OffToggles()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            _shopItems[i]._shopButton.GetComponent<Toggle>().isOn = false;
        }
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
        OffToggles();
    }

    private void AddShoppingItem(int towerCode)
    {
        ShopItem newItem = new ShopItem();
        newItem._towerPrefab = _towerAll[towerCode];
        GameObject newButton = Instantiate(_shopButtonPrefab);
        newItem._shopButton = newButton;
        SetShoppingButton(newButton, towerCode);
        TowerShopToggle itemToggle = newButton.GetComponentInChildren<TowerShopToggle>();
        itemToggle.OnInstantiated(); // 타워 프리팹이 설정되어있지 않은 상태라 이미지 수동으로 설정해줘야함.
        newButton.transform.SetParent(_shopButtonsGrid.transform, false);
        _shopItems.Add(newItem);
    }

    private void SetShoppingButton(GameObject newButton, int towerCode)
    {
        // 초기화
        Toggle toggleComponent = newButton.GetComponent<Toggle>();
        // toggleComponent.group = _shopButtonsGrid.GetComponent<ToggleGroup>();
        newButton.GetComponentInChildren<TowerShopToggle>().towerPrefab = _towerAll[towerCode];
        Debug.Log(_towerAll[towerCode].name);
        // towerIndex가 주어지면 그에 따라 상점 버튼 꾸미기
        TowerBase towerData = _towerAll[towerCode].GetComponent<TowerBase>();
        TowerShopToggle shopToggle = newButton.GetComponent<TowerShopToggle>();
        shopToggle.SetToggleItem(towerData);
    }

    public void CheckShoppingCart()
    {
        _shoppingCart.Clear();
        for (int i = 0; i < _shopItems.Count; i++)
        {
            if (_shopItems[i].isChecked)
            {
                _shoppingCart.Add(_shopItems[i]);
            }
        }
        _totalPriceText.text = GetTotalPriceInCart().ToString();
    }

    private void ClearShoppingList()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            Destroy(_shopItems[i]._shopButton);
        }
        _shopItems.Clear();
    }

    public int GetTotalPriceInCart()
    {
        int total = 0;
        for (int i = 0; i < _shopItems.Count; i++)
        {
            if (_shopItems[i].isChecked)
                total += _shopItems[i].GetPrice();
        }
        return total;
    }

    public void TryPurchase()
    {
        if (_shoppingCart.Count > 0)
        {
            // 충분한 가루 / 인벤토리 공간이 있는 지 확인
            if (CheckEnoughMoney(GetTotalPriceInCart()) == false)
            {
                // TODO: 가루가 모자랍니다 표시
                Debug.LogWarning("요술가루가 모자랍니다!");
                return;
            }
            if (CheckEnoughInventory() == false)
            {
                // TODO: 인벤토리가 가득 찼습니다 표시
                Debug.LogWarning("인벤토리가 모자랍니다!");
                return;
            }
            // 가능하다면 구매진행
            _wallet.SpendMoney(GetTotalPriceInCart());
            for (int i = 0; i < _shoppingCart.Count; i++)
            {
                _inven.AddItem(_shoppingCart[i]._towerPrefab);
            }
            DisableCheckedButton();
            // 쇼핑카트 비우기
            _shoppingCart.Clear();
            return;
        }
    }

    private bool CheckEnoughMoney(int price)
    {
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
        return _inven.MAX_INVENTORY_CAPACITY >= _inven.GetItemCount() + _shoppingCart.Count;
    }

    private void DisableCheckedButton()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            if (_shopItems[i].isChecked == true)
            {
                _shopItems[i].isChecked = false;
                Toggle toggle = _shopItems[i]._shopButton.GetComponent<Toggle>();
                toggle.isOn = false;
                toggle.interactable = false;
            }
        }
    }

    public void OnClickRerollButton()
    {
        Debug.Log("1");
        if (CheckEnoughMoney(rerollPrice) == true)
        {
            Debug.Log("2");
            _wallet.SpendMoney(rerollPrice);
            MakeShoppingList();
        }
        else
        {
            // TODO : 돈이 부족합니다 띄우기
            Debug.Log("돈이 부족합니다!");
        }
    }

}
