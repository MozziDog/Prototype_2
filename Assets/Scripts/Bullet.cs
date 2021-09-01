using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class Bullet : MonoBehaviour
{
   
    public float bulletSpeed;
    private float bulletDamage;
    private Transform target;
    
    
    public void Setup(Transform target, float bulletSpeed, float bulletDamage)
    {
        this.bulletSpeed = bulletSpeed;
        this.target = target;
        this.bulletDamage = bulletDamage;
    }
    private void OnTriggerEnter(Collider other) //적과 충돌시 상호작용
    {
        if (other.gameObject.layer != 12) return;
        if(other.transform != target) return;

        if (other.CompareTag("GroundEnemy"))
            other.GetComponent<GroundEnemy>().GetDamage(bulletDamage);
        else if (other.CompareTag("FlyingEnemy"))
            other.GetComponent<FlyingEnemy>().GetDamage(bulletDamage);
        Destroy(gameObject);
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //조준방향으로 발사
            Vector3 aimPosition = new Vector3(target.position.x, target.position.y+0.5f, target.position.z); //k
            transform.LookAt(aimPosition);
            this.transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, bulletSpeed * Time.deltaTime);
        } 
        else
        {
            Destroy(gameObject); //적 소멸시 자체 파괴
        }
    }
}
