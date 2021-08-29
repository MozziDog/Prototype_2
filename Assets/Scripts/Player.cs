using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    private float maxHP=5;
    public float currentHP;
    
    
    // Start is called before the first frame update
    void Start()
    {
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
    }
}
