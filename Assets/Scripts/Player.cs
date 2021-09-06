using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    public float maxHP;
    public float currentHP;
    public GameManager gameManager;
    public Text hpText;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHP = maxHP;
        this.anim = transform.GetComponent<Animator>();
        UpdateHpText(currentHP);


    }

    void Update()
    {
        if (currentHP <= 0) StartCoroutine(OnDie());
    }

    public void UpdateHpText(float hp) // 피격시 실행
    {
        if (hp < 0)
        {
            hp = 0;
        }
        hpText.text = string.Format("X {0}", hp);
    }

    public void StartGetHit(float hitDamage)
    {
        StartCoroutine(GetHitCoroutine(hitDamage));
    }

    public void getHealed(float heal)
    {
        currentHP += heal;
        UpdateHpText(currentHP);
    }


    public IEnumerator GetHitCoroutine(float damage)
    {
        currentHP -= damage;
        UpdateHpText(currentHP);
        anim.SetBool("isHit", true);
        yield return new WaitForSeconds(0.65f);
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
