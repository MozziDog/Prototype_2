using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    EnemyManager _enemyManager;

    public float duration;

    private void Start()
    {
        StartCoroutine(SmoothMoveStart(new Vector3(transform.position.x, 0.5f, transform.position.z), 2f));
        //StartCoroutine(SmoothMoveEnd(new Vector3(transform.position.x, -0.5f, transform.position.z), 2f));
        //Invoke("_enemyManager.BakeNav", 4f);
        Destroy(gameObject, duration);
        
    }

    IEnumerator SmoothMoveStart(Vector3 target, float speed)
    {
        while(transform.position != target)
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        yield return null;
    }
    IEnumerator SmoothMoveEnd(Vector3 target, float speed)
    {
        yield return new WaitForSeconds(2f);
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        yield return null;
    }
}
