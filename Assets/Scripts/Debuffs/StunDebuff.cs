using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunDebuff : MonoBehaviour
{
    public float LV;
    public float stunDuration;
    public float mujeockTime;


    public float EnemyoriginSpeed;
    private Transform headPos;
    private Transform bodyPos;
    private StunDebuff thisDebuff;

    public GameObject StunFx;
    public GameObject effect;
    public GameObject thisEnemy;

    public void SetUp(float LV, float stunDuration, float mujeockTime, GameObject StunFx)
    {

        this.LV = LV;
        this.stunDuration = stunDuration;
        this.mujeockTime=mujeockTime;
        this.StunFx = StunFx;

        
    }

    public void ExecuteDebuff()
    {
        if(!thisEnemy.GetComponent<EnemyInterFace>().CheckDead())
             StartCoroutine(StunCoroutine());


    }

    IEnumerator StunCoroutine()
    {

            EnemyoriginSpeed= this.gameObject.GetComponent<EnemyInterFace>().GetSpeed();
            this.gameObject.GetComponent<EnemyInterFace>().SetSpeed(0);
            effect = Instantiate(StunFx, new Vector3(headPos.position.x, headPos.position.y+0.5f, headPos.position.z), Quaternion.identity);
            effect.transform.SetParent(this.gameObject.transform);
            Destroy(effect, stunDuration);
            yield return new WaitForSeconds(stunDuration);
            this.gameObject.GetComponent<EnemyInterFace>().SetSpeed(EnemyoriginSpeed);
            yield return new WaitForSeconds(mujeockTime);
            EraseDebuff();
        }
        


    

    public void EraseDebuff()
    {
        Destroy(effect);
        Destroy(thisDebuff);
    }

    
   
    void OnEnable()
    {
        thisEnemy = this.gameObject;
        thisDebuff = this.gameObject.GetComponent<StunDebuff>();
        headPos = thisEnemy.GetComponent<EnemyInterFace>().GetHeadPos();
        bodyPos = thisEnemy.GetComponent<EnemyInterFace>().GetBodyPos();
        //SetUp(poisonDamage, poisonDuration, poisonRate, PoisonFx);
    }

    // Update is called once per frame
    void Update()
    {

        if (thisEnemy.GetComponent<EnemyInterFace>().CheckDead())
            EraseDebuff();
    }
}

