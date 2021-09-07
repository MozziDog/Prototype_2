using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopToggle : MonoBehaviour
{
    public GameObject towerPrefab;
    TowerShop shopManager;
    // Start is called before the first frame update
    void Start()
    {
        shopManager = GameObject.Find("GameManager").GetComponentInChildren<TowerShop>();
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) => OnToggleChanged(isOn));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        if (towerPrefab == null)
        {
            return;
        }
        TowerPreviewImage previewManager = GameObject.Find("TowerPreviewManager").GetComponent<TowerPreviewImage>();
        RawImage previewImage = GetComponentInChildren<RawImage>();
        Debug.Log(previewImage.gameObject.name);
        previewImage.texture = previewManager.GetTowerImage(towerPrefab);
    }

    void OnDisable()
    {
        TowerPreviewImage previewManager = GameObject.Find("TowerPreviewManager").GetComponent<TowerPreviewImage>();
        previewManager.CloseImage(towerPrefab);
    }

    public void OnToggleChanged(bool isOn)
    {
        shopManager._shopItems[gameObject.transform.GetSiblingIndex()].isChecked = isOn;
        shopManager.CheckShoppingCart();
        gameObject.GetComponentInChildren<Outline>().enabled = isOn;
    }
    
}
