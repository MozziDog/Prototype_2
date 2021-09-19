using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectedUI : MonoBehaviour
{
    Coroutine UIAnimationCoroutine = null;
    GameObject targetTower = null;
    [SerializeField] TowerInfoUI towerInfoUI;
    [SerializeField] TowerAdvance towerAdvance;
    [SerializeField] Text sellPriceText;
    public TowerManager towerManager;


    public void SetUI(GameObject tower, bool isOn)
    {
        // Debug.Log("setui : " + isOn);
        if (isOn == true)
        {
            Debug.LogWarning("tower selected!");
            targetTower = tower;
            towerAdvance.SetAdvanceTarget(targetTower);
            sellPriceText.text = (targetTower.GetComponent<TowerBase>()?.GetPrice() / 2).ToString();
            gameObject.SetActive(isOn);
            SetUIPosition(tower);
            UIOnAnimation();
        }
        else
        {
            targetTower = null;
            UIOffAnimation();
        }
    }
    public void UIOnAnimation()
    {
        IEnumerator UIAnimation()
        {
            // TODO: UI 시각효과 구현
            yield break;
        }
        if (UIAnimationCoroutine != null)
        {
            StopCoroutine(UIAnimationCoroutine);
        }
        UIAnimationCoroutine = StartCoroutine(UIAnimation());
    }

    public void UIOffAnimation()
    {
        Debug.Log("uioffanim");
        //gameObject.SetActive(false);
        IEnumerator UIAnimation()
        {
            // TODO: UI 시각효과 구현
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
        }
        if (UIAnimationCoroutine != null)
        {
            StopCoroutine(UIAnimationCoroutine);
        }
        //UIAnimationCoroutine = StartCoroutine(UIAnimation());
        gameObject.SetActive(false);
    }

    public void SetUIPosition(Vector2 position)
    {
        gameObject.GetComponent<RectTransform>().position = position;
    }

    public void SetUIPosition(GameObject targetObject)
    {
        Camera cam = Camera.main;
        SetUIPosition(cam.WorldToScreenPoint(targetObject.transform.position));
    }

    public void OnClickTowerInfoButton()
    {
        towerInfoUI.SetTowerInfoUIContents(targetTower);
        towerInfoUI.OpenTowerInfoUI();
    }
    public void CheckCanAdvance()
    {
        if (gameObject.activeSelf)
        {

            towerAdvance.CheckAdvance();
        }
    }
    public void OnClickTowerAdvanceButton()
    {
        towerAdvance.TryAdvance();

    }
}
