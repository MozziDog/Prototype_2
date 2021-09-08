using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TowerBase : MonoBehaviour
{
    TowerInfo towerinfo = new TowerInfo();
    public Vector3[] _myposition;

    [Header("Info about this Tower")]


    [SerializeField]
    public float LV;
    public string type;
    private int price = 30;
    public float bulletSpeed;
    public float bulletDamage;
    public float attackRate;
    public float attackRange;


    [Header("for ShootTower that has Bomb-Type bullet, set BombRange")]
    public float bombRange;

    [Header("for MultipleShootTower , SHOULD ALSO manipulate  property below")]
    public float bulletAmmoCount;
    public float burstRate;

    [Header("for ChainShootTower, SHOULD ALSO manipulate  property below")]
    public int maxChainCount;
    public float chainRadius;

    [Header("for PoisonShootTower, SHOULD ALSO manipulate  property below")]
    public float poisonDamage;
    public float poisonDuration;
    public float poisonRate;

    [Header("for StunShootTower, SHOULD ALSO manipulate  property below")]
    public float stunDuration;
    public float mujeockTime;

    [Header("for DebuffAreaTower, manipulate  property intensity(0~1) and range only")]
    public float slowIntensity;
    public float slowRange;

   

    [Header("Inspections that should be managed per Tower 유형")]
    [SerializeField]
    private float numberOfBlocks;
    [SerializeField]
    public GameObject[] Towers;

    [Header("is this Tower Built-Temporary Mode ?")]
    public bool isTemp = true;

    
    // Start is called before the first frame update
    public void ConfirmTowerPosition()
    {
        isTemp = false;
        gameObject.layer = LayerMask.NameToLayer("Floor");
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Floor");
        }
    }
    public void SetUp()
    {
        towerinfo.LV = this.LV;
        towerinfo.type = this.type;
    towerinfo.bulletSpeed = this.bulletSpeed;
        towerinfo.bulletDamage = this.bulletDamage;
        towerinfo.attackRate = this.attackRate;
        towerinfo.attackRange = this.attackRange;

        towerinfo.bombRange = this.bombRange;

        towerinfo.bulletAmmoCount = this.bulletAmmoCount;
        towerinfo.burstRate = this.burstRate;

        towerinfo.maxChainCount = this.maxChainCount;
        towerinfo.chainRadius = this.chainRadius;

        towerinfo.slowIntensity = this.slowIntensity;
        towerinfo.slowRange = this.slowRange;

        towerinfo.poisonDamage= this.poisonDamage; 
        towerinfo.poisonDuration = this.poisonDuration;
        towerinfo.poisonRate = this.poisonRate;


        towerinfo.stunDuration = this.stunDuration;
        towerinfo.mujeockTime = this.mujeockTime;


        foreach (GameObject Tower in Towers)
        {
            TowerInterFace tower = Tower.GetComponent<TowerInterFace>();
            tower.SetUp(towerinfo);

        }


    }
    

    void Start()
    {
        if (Towers.Length > 0) //���ݼ� Ÿ�� �����Ҷ� setup ����
            SetUp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetPrice()
    {
        return price;
    }
}
