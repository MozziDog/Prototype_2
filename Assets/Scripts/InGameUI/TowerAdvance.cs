
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct TowerUpgradeList
{
    public string TYPE;
    public GameObject[] UpgradeList;
}
public enum AlphabetTypes
{
    F,I,L,N,P,T,U,V,W,X,Y,Z
}


public class TowerAdvance : MonoBehaviour
{
    //for AdvanceList SetUp
    public TowerSelectedUI _selectUI;
    public Inventory _inven;
    public Button _advanceButton;
    public float _numberOfTypes=12;
    public int _numberOfLevels=3;
    public List<TowerUpgradeList> _towerUpgradeLists = new List<TowerUpgradeList>();
    private GameObject[] tempContainer;

    //for compare
    private List<int> ingredientindex = new List<int>();
    private string compareType;
    private float compareLV;
    private bool canAdvance;

    private GameObject targetTower;
   

    private void SetUp()
    {
        for (int i = 0; i < _numberOfTypes; i++)
        {
            TowerUpgradeList container = new TowerUpgradeList();
            AlphabetTypes tempType = (AlphabetTypes)i;
            container.TYPE = tempType.ToString();
            tempContainer = new GameObject[_numberOfLevels];
            container.UpgradeList = tempContainer;
            _towerUpgradeLists.Add(container);

        }
    }

    private void Reset()
    {
        ingredientindex.Clear();
        
    }

    public void CheckAdvance(GameObject targetTower)
    {
        compareLV = targetTower.GetComponent<TowerBase>().LV;
        if (compareLV == _numberOfLevels) return;
        compareType = targetTower.GetComponent<TowerBase>().type;
        this.targetTower = targetTower;
        for (int i=0;i<_inven._toggle.Count;i++) {
            if (compareLV != _inven._toggle[i].GetComponent<TowerBase>().LV)
                continue;
            if (compareType != _inven._toggle[i].GetComponent<TowerBase>().type)
                continue;
            ingredientindex.Add(i);
        }

        _advanceButton.interactable = ingredientindex.Count > 1 ? true : false;


    }

    public void DoAdvance()
    {
        _inven.DestoryToggle(ingredientindex[0]);
        _inven.DeleteSelectedTower(ingredientindex[0]);
        _inven.DestoryToggle(ingredientindex[1]);
        _inven.DeleteSelectedTower(ingredientindex[1]);
        Vector3 replacePos = targetTower.transform.position;
        Destroy(targetTower);
        AlphabetTypes tempType = (AlphabetTypes)System.Enum.Parse(typeof(AlphabetTypes), compareType);
        GameObject AdvancedTower = Instantiate(_towerUpgradeLists[(int)tempType].UpgradeList[(int)compareLV-1],replacePos,Quaternion.identity);


        AdvancedTower.GetComponent<TowerBase>().ConfirmTowerPosition();
        SendMessage("OnTowerSelected", AdvancedTower);
        Reset();
        

    }

    private void Awake()
    {
      //  SetUp();

    }

    
    // Update is called once per frame
    void Update()
    {
        if (_selectUI.gameObject.activeSelf)
        {
            Reset();
            _selectUI.CheckCanAdvance();
        }
    }
}
