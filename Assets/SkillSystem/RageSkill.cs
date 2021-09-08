using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageSkill : MonoBehaviour
{

    private void Start()
    {
        var hits = Physics.SphereCastAll(transform.position, 2f, Vector3.up, 0f);
        for(int i = 0; i < hits.Length; i++)
        {
            if(hits[i].transform.tag == "TowerBase" && !hits[i].transform.gameObject.GetComponent<TowerBase>().isBuffed)
            {
                Debug.Log(hits[i].transform.gameObject.name);
                float originAttackRate = hits[i].transform.gameObject.GetComponent<TowerBase>().attackRate;
                float originBulletDamage = hits[i].transform.gameObject.GetComponent<TowerBase>().bulletDamage;
                hits[i].transform.gameObject.GetComponent<TowerBase>().isBuffed = true;
                hits[i].transform.gameObject.GetComponent<TowerBase>().attackRate *= 0.9f;
                hits[i].transform.gameObject.GetComponent<TowerBase>().bulletDamage *= 1.1f;
                hits[i].transform.gameObject.GetComponent<TowerBase>().SetUp();
                StartCoroutine(BuffOff(hits[i].transform.gameObject, originAttackRate, originBulletDamage));
            }
        }
    }

    IEnumerator BuffOff(GameObject _tower, float AttackRate, float BulletDamage)
    {
        yield return new WaitForSeconds(2f);
        _tower.transform.gameObject.GetComponent<TowerBase>().isBuffed = false;
        _tower.transform.gameObject.GetComponent<TowerBase>().attackRate = AttackRate;
        _tower.transform.gameObject.GetComponent<TowerBase>().bulletDamage = BulletDamage;
        _tower.transform.gameObject.GetComponent<TowerBase>().SetUp();
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 2f);
    }
}
