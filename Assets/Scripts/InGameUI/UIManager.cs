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
        if (spellWindow.activeSelf)
            SetButtonUI_inventory();
        else
            SetButtonUI_spell();
    }

    public void SetButtonUI_spell()
    {
        spellWindow.SetActive(true);
        inventoryWindow.SetActive(false);
    }

    public void SetButtonUI_inventory()
    {
        spellWindow.SetActive(false);
        inventoryWindow.SetActive(true);
    }

    public void PlayClickSound()
    {
        SFXManager.Play(clickSound);
    }
}
