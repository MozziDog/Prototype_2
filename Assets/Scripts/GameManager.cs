using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Tower;

    [SerializeField] GameObject _floor;

    [SerializeField] EnemyManager enemyManager;
    [SerializeField] TowerManager towerManager;

    public float _scaleFactor = 1f;

    GameObject tower;
    public GameObject[] _selected;
    public GameObject _rangeGizmo; 


    private void Awake()
    {
        SetResolution();
    }
    private void Start()
    {

    }

    private void Update()
    {
        CheckTileUnderCursor();
        _floor.GetComponent<Renderer>().material.SetFloat("_GridScaleFactor", _scaleFactor);
    }

    private void CheckTileUnderCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        /*if (CalculateNewPath() == true)
        {
            pathAvailable = true;
            print("Path available");
        }
        else
        {
            pathAvailable = false;
            print("Path not available");
        }*/
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Floor")
            {
                if(hit.transform.tag == "Tower")
                {
                    _selected[0].transform.position = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0f, Mathf.Floor(hit.point.z) + 0.5f);
                    _selected[0].SetActive(true);
                }
                else
                {
                    _selected[1].transform.position = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0f, Mathf.Floor(hit.point.z) + 0.5f);
                    _selected[1].SetActive(true);
                }
                    

                _rangeGizmo.transform.position = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0f, Mathf.Floor(hit.point.z) + 0.5f);
                _rangeGizmo.SetActive(true);
            }
            //else _bg.SetActive(false);
        }
    }


    void SetResolution()
    {
        Camera cam = Camera.main;
        Rect rect = cam.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)18 / 9); // (���� / ����)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        cam.rect = rect;
    }

}
