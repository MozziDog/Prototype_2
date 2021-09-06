using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopToggle : MonoBehaviour
{
    public GameObject towerPrefab;

    // Start is called before the first frame update
    void Start()
    {

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
}
