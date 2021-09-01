using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Tower;

    [SerializeField] GameObject _floor;

    [SerializeField] EnemyManager enemyManager;
    [SerializeField] TowerManager towerManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] TowerShop towerShop;
    [SerializeField] MoneyManager wallet;

   
    public GameObject[] _selected;
    public GameObject _rangeGizmo;
    public GameObject GameOverPanel;
    public GameObject StageClearPanel;
    public float _scaleFactor = 1f;
    public bool isGameOver = false;

    private void Awake()
    {
        SetResolution();
    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        Time.timeScale = 1;
    }

    private void Update()
    {
        CheckTileUnderCursor();
        _floor.GetComponent<Renderer>().material.SetFloat("_GridScaleFactor", _scaleFactor);
        CheckGameOver();
        CheckStageClear();
    }



    void CheckGameOver()
    {
        if (isGameOver)
        {
            isGameOver = false;
            StartCoroutine(GameOver());
            
        }
      
    }

    void CheckStageClear()
    {
        if (waveManager.allWaveClear)
            Reward();
    }

    void Reward()
    {
        StageClearPanel.SetActive(true);
    }

    IEnumerator GameOver()
    {
        enemyManager.KillAllEnemy();
        yield return new WaitForSeconds(3f);
        //Time.timeScale = 0;
        GameOverPanel.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
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
                if (hit.transform.tag == "Tower")
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

    public void OnEnemyDie(int rewardMoney)
    {
        wallet.AddMoney(rewardMoney);
    }

}
