using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBombRotation : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {

        this.target = this.transform.parent.GetComponent<NormalBombBullet>().target;
        this.bulletSpeed = this.transform.parent.GetComponent<NormalBombBullet>().bulletSpeed;
        transform.LookAt(target);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(150f * Time.deltaTime * bulletSpeed, 0, 0);
        
    }
}