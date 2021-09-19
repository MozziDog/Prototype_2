using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float maxHP;
    public float currentHP;
    [SerializeField]
    private Transform bodyPos;
    [SerializeField]
    private Animator anim;
    public GameManager gameManager;
    public Text hpText;
    [SerializeField]
    private GameObject getHitFx;
    public AudioClip hitSound;



    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.anim = transform.GetComponent<Animator>();
        currentHP = maxHP;
        UpdateHpText(currentHP);


    }

    void Update()
    {
        if (currentHP <= 0) StartCoroutine(OnDie());
    }

    public void UpdateHpText(float hp) // �ǰݽ� ����
    {
        if (hp < 0)
        {
            hp = 0;
        }
        hpText.text = hp.ToString();
    }

    public void StartGetHit(float hitDamage)
    {
        GameObject effect = Instantiate(getHitFx, bodyPos.position, Quaternion.identity);
        effect.transform.SetParent(this.transform);
        Destroy(effect, 1f);
        StartCoroutine(GetHitCoroutine(hitDamage));
    }

    public void getHealed(float heal)
    {
        currentHP += heal;
        if (currentHP > maxHP) currentHP = maxHP;
        UpdateHpText(currentHP);
    }

    public IEnumerator GetHitCoroutine(float damage)
    {
        SFXManager.Play(hitSound);
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
