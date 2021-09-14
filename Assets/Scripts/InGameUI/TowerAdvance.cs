
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
    public Text UpgradeMessagePopUp;
    public float _numberOfTypes = 12;
    public int _numberOfLevels = 5;
    public List<TowerUpgradeList> _towerUpgradeLists = new List<TowerUpgradeList>();
    private GameObject[] tempContainer;

    //for comparing
    private List<int> ingredientindex = new List<int>();
    private string compareType;
    private float compareLV;

    //in-game debug properties
    public GameObject targetTowerAd;
    public GameObject AdvancedTower;
    IEnumerator checkCoroutine;
    Coroutine actingCoroutine;
    TowerTransform towerTransform;

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

    public void OnClickNotInteractiveUpgrade()
    {
        StopCoroutine(AdvanceMsgPopUp());
        if (!_advanceButton.interactable && compareLV != _numberOfLevels)
        {
            UpgradeMessagePopUp.text = "There's no Enough Ingredients to Upgrade!";
            StartCoroutine(AdvanceMsgPopUp());

        }
        else if (!_advanceButton.interactable && compareLV == _numberOfLevels)
        {
            UpgradeMessagePopUp.text = "This Tower is At Max Level!";
            StartCoroutine(AdvanceMsgPopUp());
        }
        else if (_advanceButton.interactable)
        {
            UpgradeMessagePopUp.text = string.Format("You've Upgraded it to Lv.{0}!", (int)(compareLV + 1));
            StartCoroutine(AdvanceMsgPopUp());
        }

    }

    IEnumerator AdvanceMsgPopUp()
    {
        UpgradeMessagePopUp.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        CheckAdvance();
        UpgradeMessagePopUp.transform.parent.gameObject.SetActive(false);
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
        _towerManager.towerSpawned.Remove(this.targetTowerAd);
        towerTransform = targetTowerAd.GetComponent<TowerBase>().towerTransform;
        Destroy(this.targetTowerAd);
        AlphabetTypes tempType = (AlphabetTypes)System.Enum.Parse(typeof(AlphabetTypes), compareType);
        AdvancedTower = Instantiate(_towerUpgradeLists[(int)tempType].UpgradeList[(int)compareLV - 1], replacePos, replaceRot);
        ApplyTowerTransform(AdvancedTower);
        _towerManager.towerSpawned.Add(AdvancedTower);
        AdvancedTower.GetComponent<TowerBase>().ConfirmTowerPosition();
        yield return new WaitForSeconds(0.3f);
        _towerManager.OnTowerSelected(AdvancedTower);
        _selectManager.SetSelectedTower(AdvancedTower);


    }

    void ApplyTowerTransform(GameObject tower)
    {
        if (towerTransform.reverted)
        {
            invertTempTower(tower);
        }
        for (int i = 0; i < towerTransform.rotation; i++)
        {
            RotateTempTower(tower);
        }

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


    public void RotateTempTower(GameObject temporarilyPlacedTower)
    {
        if (temporarilyPlacedTower != null)
        {
            temporarilyPlacedTower.transform.Rotate(new Vector3(0, 90, 0));
            TowerBase towerBase = temporarilyPlacedTower.GetComponent<TowerBase>();
            towerBase.towerTransform.rotation = (towerBase.towerTransform.rotation + 1) % 4;
        }
    }

    public void invertTempTower(GameObject temporarilyPlacedTower)
    {
        if (temporarilyPlacedTower != null)
        {
            if ((temporarilyPlacedTower.transform.rotation.y % 90) == 0)
            {
                TowerBase towerBase = temporarilyPlacedTower.GetComponent<TowerBase>();
                towerBase.towerTransform.reverted = !towerBase.towerTransform.reverted;
                temporarilyPlacedTower.transform.localScale = new Vector3(
                    temporarilyPlacedTower.transform.localScale.x * -1,
                    1,
                    temporarilyPlacedTower.transform.localScale.z);
                // BoxCollider 꼬임 방지를 위해 Children의 globalScale 값을 양수로 해줌.
                for (int i = 0; i < temporarilyPlacedTower.transform.childCount; i++)
                {
                    Transform childTransform = temporarilyPlacedTower.transform.GetChild(i).transform;
                    childTransform.localScale = new Vector3(
                        childTransform.localScale.x * -1,
                        1,
                        childTransform.localScale.z);
                }
            }
            else
            {
                temporarilyPlacedTower.transform.localScale = new Vector3(
                    temporarilyPlacedTower.transform.localScale.x,
                    1,
                    temporarilyPlacedTower.transform.localScale.z * -1);
                // BoxCollider 꼬임 방지를 위해 Children의 globalScale 값을 양수로 해줌.
                for (int i = 0; i < temporarilyPlacedTower.transform.childCount; i++)
                {
                    Transform childTransform = temporarilyPlacedTower.transform.GetChild(i).transform;
                    childTransform.localScale = new Vector3(
                        childTransform.localScale.x,
                        1,
                        childTransform.localScale.z * -1);
                }
            }
        }
    }
}
