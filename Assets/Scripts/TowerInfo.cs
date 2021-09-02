using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo : MonoBehaviour
{
    public Vector3[] _myposition;
    [SerializeField] private int price;
    [SerializeField]
    private float numberOfBlocks;
    public TowerHead[] TowerHeads;
    public bool isTemp = true;
    public GameObject BulletPrefab;

    public TowerData towerData;
    public float bulletSpeed;
    public float bulletDamage;
    public float attackRate;
    public float attackRange;
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
    public void Setup() //���ݼ�Ÿ�� �Ӽ� ����
    {
        price = towerData.Price;
        for (int i = 0; i < TowerHeads.Length; i++)
        {
            TowerHeads[i].BulletPrefab = this.BulletPrefab;
            TowerHeads[i].bulletSpeed = this.bulletSpeed;
            TowerHeads[i].bulletDamage = this.bulletDamage;
            TowerHeads[i].attackRate = this.attackRate;
            TowerHeads[i].attackRange = this.attackRange;
        }
    }

    void Start()
    {
        bulletSpeed = towerData.BulletSpeed;
        bulletDamage = towerData.BulletDamage;
        attackRate = towerData.AttackRate;
        attackRange = towerData.AttackRange;
        if (TowerHeads.Length > 0) //���ݼ� Ÿ�� �����Ҷ� setup ����
            Setup();
    }

    public int GetPrice()
    {
        return towerData.Price;
    }
}
