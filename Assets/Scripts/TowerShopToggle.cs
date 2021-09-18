using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopToggle : MonoBehaviour
{
    public GameObject towerPrefab;
    TowerShop shopManager;
    AudioSource audioSource;
    public AudioClip clickSound;
    // Start is called before the first frame update
    void Start()
    {
        shopManager = GameObject.Find("GameManager").GetComponentInChildren<TowerShop>();
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) => OnToggleChanged(isOn));
        audioSource = gameObject.GetComponent<AudioSource>();
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
        //TowerPreviewImage previewManager = GameObject.Find("TowerPreviewManager").GetComponent<TowerPreviewImage>();
        //RawImage previewImage = GetComponentInChildren<RawImage>();
        //Debug.Log(previewImage.gameObject.name);
        //previewImage.texture = previewManager.GetTowerImage(towerPrefab);
    }

    public void OnInstantiated()
    {
        OnEnable();
    }

    void OnDisable()
    {
        //TowerPreviewImage previewManager = GameObject.Find("TowerPreviewManager").GetComponent<TowerPreviewImage>();
        //previewManager.CloseImage(towerPrefab);
    }

    public void OnToggleChanged(bool isOn)
    {
        shopManager._shopItems[gameObject.transform.GetSiblingIndex()].isChecked = isOn;
        shopManager.CheckShoppingCart();
        gameObject.GetComponentInChildren<Outline>().enabled = isOn;
    }

    [SerializeField] Text labelText;
    [SerializeField] Image towerImage;
    [SerializeField] Image pentominoImage;
    [SerializeField] Text priceText;
    public void SetToggleItem(TowerBase tower)
    {
        if (labelText == null)
            labelText = gameObject.GetComponentInChildren<Text>();
        if (labelText != null)
        {
            labelText.text = tower.type + " 타워";
        }
        if (towerImage == null)
            towerImage = gameObject.transform.Find("Preview")?.GetComponent<Image>();
        if (towerImage != null)
        {
            towerImage.sprite = tower.towerImage;
        }
        if (pentominoImage == null)
            pentominoImage = gameObject.transform.Find("Pentomino")?.GetComponent<Image>();
        if (towerImage != null)
        {
            pentominoImage.sprite = tower.pentominoImage;
        }
        priceText.text = tower.GetPrice().ToString();
    }

}
