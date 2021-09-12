using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    GameObject _MeteorRain;

    [SerializeField]
    GameObject HealEffect;

    [SerializeField]
    GameObject SacrificeEffect;

    [SerializeField]
    GameObject _RageSkill;

    [SerializeField]
    GameObject _Wall;

    [SerializeField]
    GameObject _player;

    [SerializeField]
    MoneyManager _moneyManager;

    [SerializeField]
    EnemyManager _enemyManager;

    private float _meteorCool;
    private float nextSkill_1;
    private float nextSkill_2;
    private float nextSkill_3;
    private float nextSkill_4;
    private float nextSkill_5;
    private float nextSkill_6;

    private float maxCharge = 3f;
    private float leftCharge = 3f;
    public bool isEarthQuake = false;

    private void Start()
    {
        _meteorCool = _MeteorRain.GetComponent<ParticleCollisionInstance>().coolDown;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextSkill_1)
        {
            MeteorRain();
        }

        if (Input.GetKeyDown(KeyCode.W) && Time.time > nextSkill_2)
        {
            Heal();
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > nextSkill_3 && (_player.GetComponent<Player>().currentHP > 5))
        {
            Sacrifice();
        }

        if(Input.GetKey(KeyCode.R) && leftCharge > 0 && Time.time > nextSkill_4)
        {
            //Debug.Log(leftCharge);
            EarthQuake();
        }
        else if (Input.GetKeyUp(KeyCode.R) && leftCharge != maxCharge)
        {
            isEarthQuake = false;
            nextSkill_4 = Time.time + 10f * (1f - leftCharge / maxCharge);
            Debug.Log(10f * (1f - leftCharge / maxCharge));
            leftCharge = maxCharge;
            for (int i = 0; i < _enemyManager.CurrentEnemyList.Count; i++)
            {
                _enemyManager.CurrentEnemyList[i].GetComponent<GroundEnemy>().agent.speed = 1f;
            }
        }

        if (Input.GetKeyUp(KeyCode.T) && Time.time > nextSkill_5)
        {
            TowerRage();
        }
        

        if (Input.GetKeyUp(KeyCode.Y) && Time.time > nextSkill_6)
        {
            UpTerrain();
        }
    }

    void MeteorRain()
    {
        nextSkill_1 = Time.time + _meteorCool;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
        {
            Vector3 targetPosition = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.5f, Mathf.Floor(hit.point.z) + 0.5f);
            Instantiate(_MeteorRain, targetPosition, Quaternion.identity);
        }
    }

    void Heal()
    {
        nextSkill_2 = Time.time + 3f;
        _moneyManager.SpendMoney(100);
        _player.GetComponent<Player>().getHealed(3);
        Instantiate(HealEffect, _player.transform);
    }

    void Sacrifice()
    {
        nextSkill_3 = Time.time + 2f;
        _player.GetComponent<Player>().StartGetHit(5);
        _moneyManager.AddMoney(50);
        Instantiate(SacrificeEffect, _player.transform);
    }

    void EarthQuake()
    {
        isEarthQuake = true;
        leftCharge = leftCharge - Time.deltaTime;
        for(int i = 0; i <  _enemyManager.CurrentEnemyList.Count; i++)
        {
            _enemyManager.CurrentEnemyList[i].GetComponent<GroundEnemy>().agent.speed = 0f;
        }
    }

    void TowerRage()
    {
        nextSkill_5 = Time.time + 3f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
        {
            Vector3 targetPosition = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.5f, Mathf.Floor(hit.point.z) + 0.5f);
            Instantiate(_RageSkill, targetPosition, Quaternion.identity);
        }
    }

    void UpTerrain()
    {
        nextSkill_6 = Time.time + 2f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
        {
            GameObject Wall;
            Vector3 targetPosition = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, -0.5f, Mathf.Floor(hit.point.z) + 0.5f);
            Wall = Instantiate(_Wall, targetPosition, Quaternion.identity);
            //_enemyManager.BakeNav();
        }
    }
}
