using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : MonoBehaviour, EnemyInterFace
{
    [Header("Enemy Info")]
    public float maxHP;
    public float currentHP;
    public float moveSpeed;
    public float hitDamage;
    public Transform headPos;
    public Transform bodyPos;

    [Header("Enemy State")]
    public bool isDie = false;
    public bool isHitting = false;
    private bool isWalking = true;


    [Header("Animator and EnemyManager")]
    public Animator anim;
    public GameObject enemyManager;

    GameObject target;
    GameObject Player;
    public NavMeshAgent agent;


    public void SetUp() { }

    public Transform GetHeadPos()
    {
        return headPos;
    }

    public Transform GetBodyPos()
    {
        return bodyPos;
    }

    public bool CheckDead()
    {
        if (isDie)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public void SetSpeed(float ApplySpeed)
    {
        moveSpeed = ApplySpeed;
        agent.speed = moveSpeed;
    }




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
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);



    }

    public void setDest()
    {
        agent.SetDestination(target.transform.position);
    }

    private void Update()
    {
       
    }

   





    public void GetDamage(float Damage) //k
    {
        currentHP -= Damage;
        if (currentHP <= 0)
        {
      
            this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            ReadyToDie();
        }
    }

    public void RemoveObject()
    {
        enemyManager.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
        enemyManager.GetComponent<EnemyManager>().enemyKilledCount++;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BombBullet")
        {
            Physics.IgnoreCollision(other, this.gameObject.GetComponent<Collider>());
        }
        if (isDie)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            agent.speed = 0;
            isWalking = false;
            isDie = true;
            Vector3 lookPos = new Vector3(other.transform.position.x, this.transform.position.y, other.transform.position.z);
            transform.LookAt(lookPos);
            StartCoroutine(HitPlayer());


        }

    }

    IEnumerator HitPlayer()
    {

        yield return null;
        isHitting = true;
        anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.5f);
        Player.GetComponent<Player>().StartGetHit(hitDamage);
        yield return new WaitForSeconds(0.8f);
        RemoveObject();
    }

    public void ReadyToDie()
    {
        if (isHitting)
            return;
        isDie = true;
        this.gameObject.layer = LayerMask.NameToLayer("Dead");
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        anim.SetBool("isDead", true);
        agent.speed = 0;
        yield return new WaitForSeconds(1.35f);
        RemoveObject();
    }






}
