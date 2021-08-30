using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    public float maxHP;
    public float currentHP;
    public GameManager gameManager;

    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHP = maxHP;
        this.anim = transform.GetComponent<Animator>();
       
    }

    void Update()
    {
        if (currentHP <= 0) StartCoroutine(OnDie());
    }



    public IEnumerator GetHitCoroutine(float damage)
    {
        currentHP -= damage;
        anim.SetBool("isHit", true);
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("isHit", false);


    }


    IEnumerator OnDie()
    {
        this.transform.GetComponent<BoxCollider>().enabled = false;
        anim.SetBool("isHit", false);
        anim.SetBool("isDead", true);
        gameManager.isGameOver = true;
        yield return null;
        
    }

    
   
    
}
