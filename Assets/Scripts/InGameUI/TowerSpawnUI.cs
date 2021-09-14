using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnUI : MonoBehaviour
{
    Coroutine UIAnimationCoroutine = null;
    GameObject targetTower = null;
    [SerializeField] TowerInfoUI towerInfoUI;
    public TowerManager towerManager;
    bool isActive = false;

    public void SetUI(GameObject tower, bool isOn)
    {
        if (isOn && !isActive)
        {
            targetTower = tower;
            gameObject.SetActive(isOn);
            UIOnAnimation();
            isActive = true;
        }
        if (!isOn && isActive)
        {
            targetTower = null;
            UIOffAnimation();
            isActive = false;
        }
    }
    void UIOnAnimation()
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

    void UIOffAnimation()
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


    public void OnClickConfirmButton()
    {
        towerManager.TrySpawnTower();
        SetUI(targetTower, false);
    }

    public void OnClickCancelButton()
    {
        towerManager.destroyTempTower();
        SetUI(targetTower, false);
    }

    public void OnClickRotateButton()
    {
        towerManager.RotateTempTower();
    }

    public void OnClickInvertButton()
    {
        towerManager.invertTempTower();
    }

    public void OnClickTowerInfoButton()
    {
        towerInfoUI.SetTowerInfoUIContents(targetTower);
        towerInfoUI.OpenTowerInfoUI();
    }

}
