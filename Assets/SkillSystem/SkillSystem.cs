using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    GameObject _MeteorRain;

    [SerializeField]
    GameObject _player;

    [SerializeField]
    MoneyManager _moneyManager;

    private float _meteorCool;
    private float nextSkill_1;
    private float nextSkill_2;
    private float nextSkill_3;

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

        if (Input.GetKeyDown(KeyCode.E) && Time.time > nextSkill_3)
        {
            Sacrifice();
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
        _player.GetComponent<Player>().getHealed();
    }

    void Sacrifice()
    {
        nextSkill_3 = Time.time + 10f;
        _player.GetComponent<Player>().StartGetHit(Mathf.Round(_player.GetComponent<Player>().currentHP / 2f));
        _moneyManager.AddMoney(1000);
    }
}
