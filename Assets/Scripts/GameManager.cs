using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] GameObject clearUI;
    [SerializeField] StageClaerReward[] claerRewards;
    [SerializeField] Text clearRewardText;
    public int stageReward;

    private void Awake()
    {
        // Resolution Error!!!!!!
        // SetResolution();
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
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        Time.timeScale = 1;
    }

    private void Update()
    {
        // CheckTileUnderCursor();
        // _floor.GetComponent<Renderer>().material.SetFloat("_GridScaleFactor", _scaleFactor);
        CheckGameOver();
    }



    void CheckGameOver()
    {
        if (isGameOver)
        {
            isGameOver = false;
            StartCoroutine(GameOver());

        }

    }

    public void OnStageClear()
    {
        StartCoroutine(StageClearReward());
    }

    IEnumerator StageClearReward()
    {
        yield return new WaitForSeconds(1.7f);
        GiveStageClearReward();
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
        SceneLoader.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameExit()
    {
        SceneLoader.LoadScene("StageSelectScene");
    }

    /*
    private void CheckTileUnderCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
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
    */


    public void OnEnemyDie(int rewardMoney)
    {
        wallet.AddMoney(rewardMoney);
    }

    public void GiveStageClearReward()
    {
        stageReward = claerRewards[Global._chapter - 1].rewards[Global._stage - 1];
        clearRewardText.text = stageReward.ToString();
        Global.userProperty.gold += stageReward;
        if (Global.userProperty.LastReachedChapter < Global._chapter)
        {
            Global.userProperty.LastReachedChapter = Global._chapter;
            Global.userProperty.LastReachedStage = Global._stage;
        }
        else if (Global.userProperty.LastReachedStage < Global._stage)
        {
            Global.userProperty.LastReachedStage = Global._stage;
        }
        GameObject saveManager = GameObject.Find("SaveLoadManager");
        saveManager?.GetComponent<SaveLoadManager>().Save();
        nextStageButton.interactable = Global._stage switch
        {
            5 => false,
            _ => true
        };
        clearUI.SetActive(true);
    }

    [SerializeField] Button nextStageButton;

    public void GiveAdBonusReward()
    {
        Global.userProperty.gold += stageReward;
        clearRewardText.text = (stageReward * 2).ToString();
        GameObject saveManager = GameObject.Find("SaveLoadManager");
        saveManager?.GetComponent<SaveLoadManager>().Save();
    }

    public void OnClickButton_NextStage()
    {
        Global._stage++;
        SceneLoader.LoadScene("SampleScene_TH");
    }

}

[System.Serializable]
public class StageClaerReward
{
    public int[] rewards;
}
