using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuffTower : MonoBehaviour , TowerInterFace
{
    public float LV;
    public float slowIntensity;
    public float slowRange;
    public SphereCollider sphereCollider;
    public GameObject SlowFx;
    public string type;
    public void SetUp(TowerInfo towerinfo)
    {
        this.LV = towerinfo.LV;
        this.type = towerinfo.type;
        this.slowIntensity = 1f-towerinfo.slowIntensity;
        this.slowRange = towerinfo.slowRange;
        this.sphereCollider.radius = slowRange+0.772f; //+0.78F
    }
    
    private void SlowAreaActivate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, slowRange);
        foreach(Collider other in colliders)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !other.gameObject.GetComponent<SlowDebuff>())
            {
                other.gameObject.AddComponent<SlowDebuff>();
                other.gameObject.GetComponent<SlowDebuff>().SetUp(LV, slowIntensity, this.gameObject, SlowFx);
                other.gameObject.GetComponent<SlowDebuff>().ExecuteDebuff();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.gameObject.GetComponent<SlowDebuff>()
                && other.gameObject.GetComponent<SlowDebuff>().LV < LV)
            {
                other.gameObject.GetComponent<SlowDebuff>().RefreshSlow(LV, slowIntensity,SlowFx,this.gameObject); 
            }

        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
      
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("SLOW ERASED OFF!");
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.gameObject.GetComponent<SlowDebuff>()
            && other.gameObject.GetComponent<SlowDebuff>().LV == this.LV)
            other.gameObject.GetComponent<SlowDebuff>().EraseDebuff();
    }

    void OnDrawGizmos() //ÆøÅº ¹üÀ§ sphere Ç¥½Ã
    {
        Gizmos.DrawWireSphere(transform.position, slowRange);
    }


    void Start()
    {
        
    }

   
    void Update()
    {
        SlowAreaActivate();
    }
}
