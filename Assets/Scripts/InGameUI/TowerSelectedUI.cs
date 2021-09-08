using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectedUI : MonoBehaviour
{
    Coroutine UIAnimationCoroutine = null;
    GameObject targetTower = null;
    [SerializeField] TowerInfoUI towerInfoUI;
    public TowerManager towerManager;

    public void SetUI(GameObject tower, bool isOn)
    {
        if (isOn)
        {
            targetTower = tower;
            gameObject.SetActive(isOn);
            SetUIPosition(tower);
            UIOnAnimation();
        }
        if (!isOn)
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
        IEnumerator UIAnimation()
        {
            // TODO: UI 시각효과 구현
            yield return 0;
            gameObject.SetActive(false);
        }
        if (UIAnimationCoroutine != null)
        {
            StopCoroutine(UIAnimationCoroutine);
        }
        UIAnimationCoroutine = StartCoroutine(UIAnimation());
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
}
