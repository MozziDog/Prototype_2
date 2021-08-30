using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float moveSpeed;
    public Animator anim;
    public GameObject enemyManager;
    public float hitDamage;
    private float currentHP;
    private bool isDie = false;
    private bool isWalking = true;
    GameObject target;
    GameObject Player;
    NavMeshAgent agent;
    




    void Start()
    {
        currentHP = maxHP;
        anim = this.GetComponent<Animator>();
        enemyManager = GameObject.Find("SpawnPointGroup");
        target = GameObject.Find("EndPoint");
        Player = GameObject.Find("Player1");
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
        agent.speed = moveSpeed;



    }


    private void Update()
    {

    }
   

    public void GetDamage(float Damage) //k
    {
        currentHP -= Damage;
        if (currentHP <= 0)
        {
            ReadyToDie();
        }
    }

    public void RemoveObject()
    {
        isDie = true;
        enemyManager.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
        enemyManager.GetComponent<EnemyManager>().enemyKilledCount++;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            agent.speed = 0;
            isWalking = false;
            StartCoroutine(HitPlayer());


        }

    }

    IEnumerator HitPlayer()
    {
        anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Player.GetComponent<Player>().GetHitCoroutine(hitDamage));
        yield return new WaitForSeconds(0.75f);
        RemoveObject();
    }

    public void ReadyToDie()
    {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        anim.SetBool("isDead", true);
        agent.speed = 0;
        yield return new WaitForSeconds(2f);
        RemoveObject();
    }






}
