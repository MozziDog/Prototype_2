using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChainBullet : Bullet_base, BulletInterFace
{
    public string BulletName;
    public float bulletSpeed;
    private float bulletDamage;
    public Transform currentTarget;
    private Transform currentBodyPos;
    public GameObject impactParticle;
    public Vector3 aimPosition;
    public int maxChainCount = 0;
    public int currentChainCount = 0;
    public float chainRadius = 1.5f;
    public WeaponState weaponState = WeaponState.AttackToTarget;
    private List<GameObject> hitTargets;
    RaycastHit hit;
    private AudioSource musicPlayer;
    public AudioClip shootSound;
    private bool isShooting = false;
    public void SetUp(BulletInfo bulletinfo)
    {
        isShooting = true;
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.currentTarget = bulletinfo.attackTarget;
        this.currentBodyPos = currentTarget.GetComponent<EnemyInterFace>().GetBodyPos();
        this.bulletDamage = bulletinfo.bulletDamage;
        this.maxChainCount = bulletinfo.maxChainCount;
        this.chainRadius = bulletinfo.chainRadius;
    }

    public void ChangeState(WeaponState newState) //���� ����  Ž��, ����  ����� �ڷ�ƾ ��ȯ
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    private IEnumerator SearchTarget() //�� Ž��
    {
        while (true)
        {
            float closestDistSqr = Mathf.Infinity;
            Collider[] colliders = Physics.OverlapSphere(transform.position, chainRadius);

            if (colliders.Length == 0) //destroy if there's no enemy to chase
                Disable(gameObject);

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
                Disable(gameObject);

            if (currentTarget) //if found new target
            {
                currentBodyPos = currentTarget.GetComponent<EnemyInterFace>().GetBodyPos();
                hitTargets.Add(currentTarget.gameObject);
                ChangeState(WeaponState.AttackToTarget);
            }


            yield return null;
        }
    }

    private IEnumerator AttackToTarget() //�� ����
    {
        while (true)
        {
            if (currentTarget == null)
            {

                ChangeState(WeaponState.SearchTarget);
                break;
            }


            Shoot();
            yield return null;
        }
    }


    private void OnTriggerEnter(Collider other) //���� �浹�� ��ȣ�ۿ�
    {

        if (other.gameObject.layer != 12) return;
        if (other.transform != currentTarget) return;
        StopCoroutine(AttackToTarget());


        if (other.CompareTag("GroundEnemy"))
            other.GetComponent<GroundEnemy>().GetDamage(bulletDamage);
        else if (other.CompareTag("FlyingEnemy"))
            other.GetComponent<FlyingEnemy>().GetDamage(bulletDamage);

        //hit particle spawn
        GameObject clone = Instantiate(impactParticle, currentBodyPos.position,
            Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
        clone.transform.parent = currentTarget.transform;
        Destroy(clone, 2f);


        currentChainCount++; //count the chain successed

        //find the next target
        StartCoroutine(SearchStateAfterTime(0.12f));
    }

    IEnumerator SearchStateAfterTime(float Time)
    {
        Debug.LogWarning("search after time");
        yield return new WaitForSeconds(Time);
        currentTarget = null;
        ChangeState(WeaponState.SearchTarget);
    }

    void Shoot()
    {
        //���ع������� �߻�..������� on 
        aimPosition = currentBodyPos.position;//new Vector3(currentTarget.position.x, currentTarget.position.y + 0.5f, currentTarget.position.z); 
        Physics.Raycast(transform.position, aimPosition, out hit);
        transform.LookAt(aimPosition);

        this.transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, bulletSpeed * Time.deltaTime);
    }

    void MusicPlay()
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = shootSound;
        musicPlayer.time = 0;
        musicPlayer.volume = Global.soundVolume;
        musicPlayer.Play();
    }


    // Start is called before the first frame update
    void Start()
    {
      
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            isShooting = false;
            currentChainCount = 0;
            MusicPlay();
            hitTargets = new List<GameObject>();
            hitTargets.Add(currentTarget.gameObject);
            StartCoroutine(AttackToTarget());    
        }

        Disable(gameObject, 2.5f);
        if (currentChainCount == maxChainCount)
            Disable(gameObject);
    }


}
