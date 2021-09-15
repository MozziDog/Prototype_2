using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeButtonEvent : MonoBehaviour
{
    public Button _advanceButton;
    public Text UpgradeMessagePopUp;
    private Coroutine actingCoroutine;
    [SerializeField]
    private TowerAdvance _towerAdvance;
    public void OnClickNotInteractiveUpgrade()
    {

        if (_advanceButton.interactable)
        {
            UpgradeMessagePopUp.text = string.Format("You've Upgraded it to Lv.{0}!", (int)(_towerAdvance.compareLV + 1));
            actingCoroutine = StartCoroutine(AdvanceMsgPopUp());
            Debug.LogWarning("ADVANCE COMPLETE!");
        }
        else if (!_advanceButton.interactable && _towerAdvance.compareLV != _towerAdvance._numberOfLevels)
        {
            UpgradeMessagePopUp.text = "There's no Enough Ingredients to Upgrade!";
            actingCoroutine = StartCoroutine(AdvanceMsgPopUp());
            Debug.LogWarning("NO Ingredients");
        }
        else if (!_advanceButton.interactable && _towerAdvance.compareLV == _towerAdvance._numberOfLevels)
        {
            UpgradeMessagePopUp.text = "This Tower is At Max Level!";
            actingCoroutine = StartCoroutine(AdvanceMsgPopUp());
        }


    }


    IEnumerator AdvanceMsgPopUp()
    {
        UpgradeMessagePopUp.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        Debug.Log("Message Pop!!!!!!!!!!!!!!!!!!!!!!!!!!");
        UpgradeMessagePopUp.transform.parent.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
