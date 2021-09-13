using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Earthquake : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    [SerializeField] Button _button;
    [SerializeField] Image _cool_Img;
    [SerializeField] GameObject _cool_txt;

    [SerializeField] float coolTime = 30f;
    [SerializeField] float coolMaxTime = 30f;
    public bool isClicked = false;
    [SerializeField] float leftTime = 0f;

    [SerializeField] private float holdTime = 10f;
    [SerializeField] private float _holdTime = 0f;

    [SerializeField] float _coolTime;

    [SerializeField] EnemyManager _enemyManager;


    private void Start()
    {
        _enemyManager = GameObject.Find("SpawnPointGroup").GetComponent<EnemyManager>();
    }
    private void Update()
    {
        if (isClicked)
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                if (leftTime <= 0)
                {
                    _cool_txt.SetActive(false);
                    leftTime = 0f;
                    if (_button)
                        _button.enabled = true;
                    isClicked = true;
                }

                float ratio = leftTime / coolTime;
                if (_cool_Img)
                {
                    _cool_Img.fillAmount = ratio;
                }
                _cool_txt.GetComponent<Text>().text = Mathf.Floor(leftTime).ToString() + " √ ";
            }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(leftTime == 0f)
        {
            isClicked = true;
            StartCoroutine(CountDown());
            StartCoroutine(EarthquakeSkill());
            Invoke("OnLongPress", holdTime);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (leftTime == 0f)
        {
            StopAllCoroutines();
            coolTime = coolMaxTime * _holdTime / holdTime;
            Debug.Log(_holdTime);
            StartCoolTime();
            _holdTime = 0f;
            CancelInvoke("OnLongPress");
            for (int i = 0; i < _enemyManager.CurrentEnemyList.Count; i++)
            {
                _enemyManager.CurrentEnemyList[i].GetComponent<GroundEnemy>().agent.speed = 1f;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (leftTime == 0f)
        {
            StopAllCoroutines();
            CancelInvoke("OnLongPress");
            for (int i = 0; i < _enemyManager.CurrentEnemyList.Count; i++)
            {
                _enemyManager.CurrentEnemyList[i].GetComponent<GroundEnemy>().agent.speed = 1f;
            }
        }
        
    }

    private void OnLongPress()
    {
        StopAllCoroutines();
        coolTime = coolMaxTime * _holdTime / holdTime;
        Debug.Log(_holdTime);
        StartCoolTime();
        _holdTime = 0f;
        CancelInvoke("OnLongPress");
        for (int i = 0; i < _enemyManager.CurrentEnemyList.Count; i++)
        {
            _enemyManager.CurrentEnemyList[i].GetComponent<GroundEnemy>().agent.speed = 1f;
        }
    }

    IEnumerator CountDown()
    {
        while (true)
        {
            _holdTime += Time.deltaTime;
            yield return null;
        }
    }
    
    IEnumerator EarthquakeSkill()
    {
        while (true)
        {
            for (int i = 0; i < _enemyManager.CurrentEnemyList.Count; i++)
            {
                _enemyManager.CurrentEnemyList[i].GetComponent<GroundEnemy>().agent.speed = 0f;
            }
            yield return null;
        }
    }

    public void StartCoolTime()
    {
        leftTime = coolTime;
        isClicked = true;
        if (_button)
            _button.enabled = false;
        _cool_txt.SetActive(true);
    }

}
