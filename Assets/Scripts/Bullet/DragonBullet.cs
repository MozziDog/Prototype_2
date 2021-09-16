using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBullet : MonoBehaviour
{
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUp(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    // Update is called once per frame
    void Update()
    {
      this.transform.LookAt(targetPos);
      this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, 8f * Time.deltaTime);   
    }
}
