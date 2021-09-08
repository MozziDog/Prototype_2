using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChainBullet : MonoBehaviour ,BulletInterFace
{
    public string BulletName;
    public float bulletSpeed;
    private float bulletDamage;
    public Transform currentTarget;
    public GameObject impactParticle;
    public Vector3 aimPosition;
    public int maxChainCount=0;
    public int currentChainCount = 0;
    public float chainRadius = 1.5f;
    public WeaponState weaponState = WeaponState.AttackToTarget;
    private List<GameObject> hitTargets;
    RaycastHit hit;

    public void SetUp(BulletInfo bulletinfo)
    {
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.currentTarget = bulletinfo.attackTarget;
        this.bulletDamage = bulletinfo.bulletDamage;
        this.maxChainCount = bulletinfo.maxChainCount;
        this.chainRadius = bulletinfo.chainRadius;
    }

    public void ChangeState(WeaponState newState) //적에 대한  탐색, 공격  모드의 코루틴 전환
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    private IEnumerator SearchTarget() //적 탐색
    {
        while (true)
        {
            float closestDistSqr = Mathf.Infinity;
            Collider[] colliders = Physics.OverlapSphere(transform.position, chainRadius); 
            
            if (colliders.Length == 0) //destroy if there's no enemy to chase
                Destroy(gameObject);
           
            foreach (Collider searchedObject in colliders)
            {
                if (searchedObject.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                    continue;
                if (hitTargets.Contains(searchedObject.gameObject))
                    continue;


                float distance = Vector3.Distance(searchedObject.gameObject.transform.position, transform.position);
                if (distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    currentTarget = searchedObject.transform;
                    
                }
            }
            if (!currentTarget) //if all searched colliders are already been chased..destroy
                Destroy(gameObject);

            if (currentTarget) //if found new target
            {
                hitTargets.Add(currentTarget.gameObject);
                ChangeState(WeaponState.AttackToTarget);
            }

            
            yield return null;
        }
    }

    private IEnumerator AttackToTarget() //적 공격
    {
        while (true)
        {
            if (currentTarget == null)
            {
                
                ChangeState(WeaponState.SearchTarget);
                break;
            }


            Shoot();
            /*
            if (this.transform.position == currentTarget.transform.position)
            {
                currentTarget = null;
                ChangeState(WeaponState.SearchTarget);
            }
            */

            yield return null;
        }
    }

    
    private void OnTriggerEnter(Collider other) //적과 충돌시 상호작용
    {
        
        if (other.gameObject.layer != 12) return;
        if(other.transform != currentTarget) return;

        

        if (other.CompareTag("GroundEnemy"))
            other.GetComponent<GroundEnemy>().GetDamage(bulletDamage);
        else if (other.CompareTag("FlyingEnemy"))
            other.GetComponent<FlyingEnemy>().GetDamage(bulletDamage);

        //hit particle spawn
        GameObject clone = Instantiate(impactParticle, currentTarget.transform.position + Vector3.up * 0.5f,
            Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
        clone.transform.parent = currentTarget.transform;
        Destroy(clone, 3);


        currentChainCount++; //count the chain successed

        //find the next target
        currentTarget = null;
       ChangeState(WeaponState.SearchTarget);
    }

    void Shoot()
    {
        //조준방향으로 발사..추적기능 on 
        aimPosition = new Vector3(currentTarget.position.x, currentTarget.position.y + 0.5f, currentTarget.position.z); 
        Physics.Raycast(transform.position, aimPosition, out hit);
        transform.LookAt(aimPosition);
        
        this.transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, bulletSpeed * Time.deltaTime);
    }
   
    // Start is called before the first frame update
    void Start()
    {
        currentChainCount = 0;
        hitTargets = new List<GameObject>();
        hitTargets.Add(currentTarget.gameObject);
        StartCoroutine(AttackToTarget());
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (currentChainCount == maxChainCount)
            Destroy(gameObject);
        
    }
}
