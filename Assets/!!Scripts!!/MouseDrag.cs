using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool isSelected = false;
    [SerializeField] GameObject _tower;
    GameObject tower;
    Vector3 originPos;


    private void Start()
    {
        originPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSelected = true;
        gameObject.SetActive(false);
        Debug.Log("Start");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Debug.Log("Draging");
        //throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Destroy(_tower);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag != "Tower")
            {
                for (int i = 0; i < _tower.GetComponent<TowerBase>()._myposition.Length; i++)
                {
                    Collider[] hitColliders;
                    hitColliders = Physics.OverlapSphere(_tower.GetComponent<TowerBase>()._myposition[i], 0.4f);
                }

                tower = Instantiate(_tower, new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.5f, Mathf.Floor(hit.point.z) + 0.5f), Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                gameObject.transform.position = originPos;
                gameObject.SetActive(true);
            }
        }
        else
        {
            gameObject.transform.position = originPos;
            gameObject.SetActive(true);
        }
        Debug.Log("Drop");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isSelected = false;
        Debug.Log("EndDrag");
    }
}
