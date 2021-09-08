using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfoUI : MonoBehaviour
{
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
