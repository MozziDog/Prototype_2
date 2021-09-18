using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wall : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyManager;

    public float duration;

    private void Start()
    {
        _enemyManager = GameObject.Find("SpawnPointGroup");
        StartCoroutine(SmoothMoveStart(new Vector3(transform.position.x, 0.5f, transform.position.z), 2f));
        //StartCoroutine(SmoothMoveEnd(new Vector3(transform.position.x, -0.5f, transform.position.z), 2f));
        //Invoke("_enemyManager.BakeNav", 4f);
        StartCoroutine(DestroyWall(duration));
        
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

    IEnumerator DestroyWall(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
        updatePath();
    }

    public void updatePath()
    {
        for(int i = 0; i < _enemyManager.GetComponent<EnemyManager>().CurrentEnemyList.Count; i++)
        {
            _enemyManager.GetComponent<EnemyManager>().CurrentEnemyList[i].GetComponent<GroundEnemy>().setDest();
        }
    }
}
