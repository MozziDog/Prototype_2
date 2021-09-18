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
        if (!EventSystem.current.IsPointerOverGameObject(0) && Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                OnTouchStart();
            }
            if (touch.phase == TouchPhase.Moved)
            {
                OnTouchMove();
            }
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                OnTouchEnd();
            }
        }
    }

    void OnTouchStart()
    {
        lastTouchPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
        {

            if (hit.transform.CompareTag("Tower") || hit.transform.CompareTag("TowerBase"))// 타워 터치된 경우
            {
                GameObject tower = hit.collider.gameObject;
                tower = GetTowerFromTowerParts(tower);
                if (tower.GetComponent<TowerBase>() != null)
                    selectedTower = tower;
                else
                    selectedTower = null;
            }
            else // 바닥 터치된 경우
            {
                //if (selectedTower != null)
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
        else // 아무 빈 공간을 터치한 경우
        {
            if (selectedTower != null)
            {
                SendMessage("OnTowerUnselected", selectedTower);
                selectedTower = null;
            }
        }
    }

    void OnTouchMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
        {
            selectedTilePosition = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.5f, Mathf.Floor(hit.point.z) + 0.5f);
            if (lastSelectedTilePosition != selectedTilePosition)
            {
                SendMessage("OnSelectedTileChanged", selectedTilePosition);
                lastSelectedTilePosition = selectedTilePosition;
            }
        }
    }

    void OnTouchEnd()
    {
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Ray ray = Camera.main.ScreenPointToRay(lastTouchPosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
            {
                GameObject tower = hit.transform.gameObject;
                tower = GetTowerFromTowerParts(tower);
                if (tower == selectedTower)
                {
                    Debug.Log("new tower touched!");
                    SendMessage("OnTowerSelected", selectedTower);
                }
            }
        }
        selectedTower = null;
    }

    GameObject GetTowerFromTowerParts(GameObject towerPart)
    {
        GameObject tower = towerPart;
        while (tower.transform.parent != null)
        {
            if (tower.transform.parent.CompareTag("TowerBase") == true || tower.transform.parent.CompareTag("Tower") == true)
            {
                tower = tower.transform.parent.gameObject;
            }
            else
            {
                break;
            }
        }
        return tower;
    }

    public Vector3 GetPointedTile()
    {
        return lastSelectedTilePosition;
    }

    public GameObject GetPointedTower()
    {
        return selectedTower;
    }

    public void SetSelectedTower(GameObject tower)
    {
        selectedTower = tower;
    }
}
