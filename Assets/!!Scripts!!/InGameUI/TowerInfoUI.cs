using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoUI : MonoBehaviour
{
    [SerializeField] Image towerImage;
    [SerializeField] Text attackDamageText;
    [SerializeField] Text attackRangeText;
    [SerializeField] Text attackSpeedText;
    [SerializeField] Text attackAreaText;
    [SerializeField] Text specialEffectText;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenTowerInfoUI()
    {
        SetWindow(true);
    }

    public void CloseTowerInfoUI()
    {
        SetWindow(false);
    }

    void SetWindow(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    public void SetTowerInfoUIContents(GameObject tower)
    {
        tower.GetComponent<TowerBase>();
        SetTowerInfoData();
        SetTowerInfoText();
        SetTowerPreview();
    }

    void SetTowerInfoData()
    {

    }

    void SetTowerInfoText()
    {

    }

    void SetTowerPreview()
    {

    }
}
