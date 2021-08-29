using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Base : MonoBehaviour
{
    [SerializeField]
    protected float maxHP;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected Animator anim;

    protected float currentHP;
    protected bool isDie = false;
    protected bool isWalking = true;
    public float hitDamage;
    [SerializeField] protected int rewardMoney;
    protected GameObject target;
    protected GameObject Player;



    public GameObject GameManagerObject;
    // Start is called before the first frame update
    protected void Start()
    {
        currentHP = maxHP;
        anim = this.GetComponent<Animator>();
        GameManagerObject = GameObject.Find("SpawnPoint");
        target = GameObject.Find("EndPoint");
        Player = GameObject.Find("Player1");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetDamage(float Damage) //k
    {
        currentHP -= Damage;
        if (currentHP <= 0)
        {
            OnEnemyDie();
        }
    }

    protected void OnEnemyDie()
    {
        isDie = true;
        GameManagerObject.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
        GameObject.Find("GameManager").GetComponent<GameManager>().OnEnemyDie(rewardMoney);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            isWalking = false;
            StartCoroutine(HitPlayer());
        }
    }

    IEnumerator HitPlayer()
    {
        if (anim != null)
            anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Player.GetComponent<Player>().GetHitCoroutine(hitDamage));
        GameManagerObject.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}
