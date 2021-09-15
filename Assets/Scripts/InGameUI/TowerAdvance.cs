
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
    F, I, L, N, P, T, U, V, W, X, Y, Z
}


public class TowerAdvance : MonoBehaviour
{
    //for AdvanceList SetUp, that means list making in editor
    public TowerSelectedUI _selectUI;
    public Inventory _inven;
    public Button _advanceButton;
    public TowerManager _towerManager;
    public SelectManager _selectManager;
    public TowerInfoUI _towerInfoUI;
    public Text UpgradeMessagePopUp;
    public float _numberOfTypes = 12;
    public int _numberOfLevels = 5;
    public List<TowerUpgradeList> _towerUpgradeLists = new List<TowerUpgradeList>();
    private GameObject[] tempContainer;

    //for comparing
    private List<int> ingredientindex = new List<int>();
    public string compareType;
    public float compareLV;

    //in-game debug properties
    public GameObject targetTowerAd;
    public GameObject AdvancedTower;
    IEnumerator checkCoroutine;
    Coroutine actingCoroutine;
    private void SetUp()
    {
        for (int i = 0; i < _numberOfTypes; i++)
        {
            TowerUpgradeList container = new TowerUpgradeList();
            AlphabetTypes tempType = (AlphabetTypes)i;
            container.TYPE = tempType.ToString();
            tempContainer = new GameObject[_numberOfLevels - 1];
            container.UpgradeList = tempContainer;
            _towerUpgradeLists.Add(container);


        }
    }

    
    private void Reset()
    {
        ingredientindex.Clear();

    }

    public void SetAdvanceTarget(GameObject tower)
    {
        this.targetTowerAd = tower;
        _selectUI.CheckCanAdvance();
    }

    public void CheckAdvance()
    {
        Reset();
        compareLV = this.targetTowerAd.GetComponent<TowerBase>().LV;
        if (compareLV == _numberOfLevels)
        {
            _advanceButton.interactable = false;
            return;
        }

        compareType = this.targetTowerAd.GetComponent<TowerBase>().type;

        for (int i = 0; i < _inven._toggle.Count; i++)
        {
            _inven._toggle[i].interactable = false;
            if (compareType != _inven._tower[i].GetComponent<TowerBase>().type)
            {
                continue;
            }

            ingredientindex.Add(i);
        }



        if (_advanceButton.interactable = ingredientindex.Count > 0 ? true : false)
        {
            _inven._toggle[ingredientindex[0]].interactable = true;
            _inven._toggle[ingredientindex[0]].Select();
        }



    }

    public void TryAdvance()
    {
        StartCoroutine(DoAdvance());
    }

    IEnumerator DoAdvance()
    {

        _inven.DestoryToggle(ingredientindex[0]);
        _inven.DeleteSelectedTower(ingredientindex[0]);
        Vector3 replacePos = this.targetTowerAd.transform.position;
        Quaternion replaceRot = this.targetTowerAd.transform.rotation;
        Transform local = this.targetTowerAd.transform; 
        _towerManager.towerSpawned.Remove(this.targetTowerAd);
        Destroy(this.targetTowerAd);
        AlphabetTypes tempType = (AlphabetTypes)System.Enum.Parse(typeof(AlphabetTypes), compareType);
        GameObject checkInvert = _towerUpgradeLists[(int)tempType].UpgradeList[(int)compareLV - 1];
        AdvancedTower = Instantiate(checkInvert, replacePos, replaceRot);
        AdvancedTower.transform.localScale = local.localScale;
        for (int i = 0; i < AdvancedTower.transform.childCount; i++)
        {
            Transform childTransform = AdvancedTower.transform.GetChild(i).transform;
            childTransform.localScale = targetTowerAd.transform.GetChild(i).transform.localScale; 
        }

        _towerManager.towerSpawned.Add(AdvancedTower);
        AdvancedTower.GetComponent<TowerBase>().ConfirmTowerPosition();
        _towerManager.OnTowerSelected(AdvancedTower);
        _selectManager.SetSelectedTower(AdvancedTower);
       
        yield return new WaitForSeconds(1f);
        CheckAdvance();
        yield return null;
        

    }


   

    private void Start()
    {
    }

    private void OnEnable()
    {
        SetUp();
        Reset();
        _selectUI.CheckCanAdvance();
   

    }


    private void OnDisable()
    {
        for (int i = 0; i < _inven._toggle.Count; i++)
        {
            _inven._toggle[i].interactable = true;
        }
        //StopCoroutine(AdvanceMsgPopUp());
        UpgradeMessagePopUp.transform.parent.gameObject.SetActive(false);


    }


    // Update is called once per frame
    void Update()
    {

    }
}
