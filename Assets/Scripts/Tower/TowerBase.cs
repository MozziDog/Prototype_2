using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TowerBase : MonoBehaviour
{
    public Sprite towerImage;
    public Sprite pentominoImage;
    public string towerDescription;
    TowerInfo towerinfo = new TowerInfo();
    public Vector3[] _myposition;

    [Header("Info about this Tower")]


    [SerializeField]
    public float LV;
    public string type;
    public int price = 30;
    public float bulletSpeed;
    public float bulletDamage;
    public float attackRate;
    public float attackRange;


    [Header("for ShootTower that has Bomb-Type bullet, SHOULD ALSO manipulate BombRange")]
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

    [Header("for SlowAreaTower, manipulate  property intensity(0~1) and range ONLY")]
    public float slowIntensity;
    public float slowRange;

    [Header("-------BELOW ARE INSPECTIONS THAT ARE RELATED IN-GAME MODE-------")]

    [Header("Inspections that should be managed per Tower 유형")]
    [SerializeField]
    private float numberOfBlocks;
    [SerializeField]
    public GameObject[] Towers;

    [Header("is this Tower Built-Temporary Mode ?")]
    public bool isTemp = true;


    [Header("is Buff(Skill)")]
    public bool isBuffed = false;

    public TowerTransform towerTransform;


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

        towerinfo.poisonDamage = this.poisonDamage;
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

    public int GetPrice()
    {
        return price;
    }

    public string GetAttackDamage()
    {
        return bulletDamage.ToString();
    }

    public string GetAttackRange()
    {
        return attackRange != 0 ? attackRange.ToString() : slowRange.ToString();
    }

    public string GetAttackRate()
    {
        return attackRate != 0 ? string.Format("{0:0.00}", (1 / attackRate)) : "-";
    }

    public string GetAttackArea()
    {
        if (bombRange != 0)
            return bombRange.ToString();
        else if (maxChainCount != 0)
            return maxChainCount.ToString() + "개";
        else if (bulletDamage != 0)
            return "단일";
        else
            return "-";
    }

    public string GetSpecialValue()
    {
        if (slowIntensity != 0)
            return (slowIntensity * 100).ToString() + "%";
        else if (stunDuration != 0)
            return stunDuration.ToString() + "초";
        else if (poisonDamage != 0)
            return "틱 당" + poisonDamage.ToString();
        else
            return "-";
    }
}

[System.Serializable]
public class TowerTransform
{
    public bool reverted = false;
    public int rotation = 0; // 시계방향으로 회전한 횟수
}