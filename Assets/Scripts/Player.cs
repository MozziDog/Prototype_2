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

    [SerializeField]
    Text _lifeText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        this.anim = transform.GetComponent<Animator>();
    }    

    public IEnumerator GetHitCoroutine(float damage)
    {
        currentHP -= damage;
        anim.SetBool("isHit", true);
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("isHit", false);


    }

  
    void OnDie()
    {
        this.transform.GetComponent<BoxCollider>().enabled = false;
        
         Time.timeScale = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0) OnDie();
        _lifeText.text = "Life : " + currentHP.ToString();
    }
}
