using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoUI : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] Image towerImage;
    [SerializeField] Text attackDamageText;
    [SerializeField] Text attackRangeText;
    [SerializeField] Text attackSpeedText;
    [SerializeField] Text attackAreaText;
    [SerializeField] Text specialEffectText;
    [SerializeField] Text towerDescription;



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
        UI.SetActive(isOn);
    }

    public void SetTowerInfoUIContents(GameObject tower)
    {
        TowerBase towerComponent = tower.GetComponent<TowerBase>();
        SetTowerInfoData(towerComponent);
        SetTowerInfoText(towerComponent);
        SetTowerPreview(towerComponent);
    }

    void SetTowerInfoData(TowerBase tower)
    {
        attackDamageText.text = tower.GetAttackDamage();
        attackRangeText.text = tower.GetAttackRange();
        attackSpeedText.text = tower.GetAttackRate();
        attackAreaText.text = tower.GetAttackArea();
        specialEffectText.text = tower.GetSpecialValue();
    }

    void SetTowerInfoText(TowerBase tower)
    {
        string newText = tower.towerDescription.Replace("\\n", "\n");
        towerDescription.text = newText;
    }

    public void SetTowerPreview(TowerBase tower)
    {
        towerImage.sprite = tower.towerImage;
    }
}
