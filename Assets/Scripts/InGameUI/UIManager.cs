using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject spellWindow;
    [SerializeField] GameObject inventoryWindow;
    public AudioClip clickSound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeBottomUI()
    {
        if (inventoryWindow.activeSelf)
            SetButtonUI_spell();
        else
            SetButtonUI_inventory();
    }

    public void SetButtonUI_spell()
    {
        inventoryWindow.SetActive(false);
    }

    public void SetButtonUI_inventory()
    {
        inventoryWindow.SetActive(true);
    }

    public void PlayClickSound()
    {
        SFXManager.Play(clickSound);
    }
}
