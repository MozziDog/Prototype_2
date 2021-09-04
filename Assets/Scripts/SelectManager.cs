using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// TowerManager 등과 같은 컴포넌트와 같은 오브젝트에 넣어주면
// 터치가 될 때마다 해당 컴포넌트로 메세지를 보내 OnTowerSelected 등의 함수를 호출해줍니다.
public class SelectManager : MonoBehaviour
{
    Vector3 selectedTilePosition = Vector3.zero;
    Vector3 lastSelectedTilePosition = Vector3.zero;
    GameObject selectedTower;
    Vector2 lastTouchPosition;

    void Update()
    {
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
        {
            OnTouch();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnTouchUp();
        }
    }

    void OnTouch()
    {
        lastTouchPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
        {
            if (Input.GetMouseButtonDown(0)) // 터치 시작될 때
            {
                if (hit.transform.CompareTag("Tower"))// 타워 터치된 경우
                {
                    GameObject tower = hit.transform.gameObject;
                    while (tower.transform.parent.CompareTag("Tower"))
                    {
                        tower = tower.transform.parent.gameObject;
                    }
                    selectedTower = tower;
                }
                else    // 바닥 터치된 경우
                {
                    if (selectedTower != null)
                    {
                        SendMessage("OnTowerUnselected", selectedTower);
                        selectedTower = null;
                    }
                    selectedTilePosition = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.5f, Mathf.Floor(hit.point.z) + 0.5f);
                    if (lastSelectedTilePosition != selectedTilePosition)
                    {
                        SendMessage("OnSelectedTileChanged", selectedTilePosition);
                        lastSelectedTilePosition = selectedTilePosition;
                    }
                }
            }
            else // 터치 유지되는 경우
            {
                selectedTilePosition = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.5f, Mathf.Floor(hit.point.z) + 0.5f);
                if (lastSelectedTilePosition != selectedTilePosition)
                {
                    SendMessage("OnSelectedTileChanged", selectedTilePosition);
                    lastSelectedTilePosition = selectedTilePosition;
                }
            }

        }
    }

    void OnTouchUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(lastTouchPosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
        {
            GameObject tower = hit.transform.gameObject;
            while (tower.transform.parent.CompareTag("Tower"))
            {
                tower = tower.transform.parent.gameObject;
            }
            if (tower == selectedTower)
            {
                SendMessage("OnTowerSelected", selectedTower);
            }
        }
    }

    public Vector3 GetPointedTile()
    {
        return lastSelectedTilePosition;
    }

    public GameObject GetPointedTower()
    {
        return selectedTower;
    }
}
