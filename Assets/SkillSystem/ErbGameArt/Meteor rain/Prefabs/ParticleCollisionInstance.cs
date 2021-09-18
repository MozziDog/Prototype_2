/*This script created by using docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleCollisionInstance : MonoBehaviour
{
    public GameObject[] EffectsOnCollision;
    public float Offset = 0;
    public float DestroyTimeDelay = 5;
    public bool UseWorldSpacePosition;
    public bool UseFirePointRotation;
    public bool DestroyMainEffect = true;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem ps;


    //Editable
    [Header("¼öÁ¤¿ë")]
    public float duration = 3f;
    public float radius = 0.5f;
    public float coolDown = 3f;
    public float damage;

    void Start()
    {
        StartCoroutine(Damaging());
        part = GetComponent<ParticleSystem>();
        SetParticle();
    }
    void SetParticle()
    {
        part.Stop();
        //main
        var main = part.main;
        main.duration = duration;

        //shape
        var shape = part.shape;
        shape.radius = radius;

        part.Play();
    }
    void OnParticleCollision(GameObject other)
    {      
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);     
        for (int i = 0; i < numCollisionEvents; i++)
        {
            foreach (var effect in EffectsOnCollision)
            {
                var instance = Instantiate(effect, collisionEvents[i].intersection + collisionEvents[i].normal * Offset, new Quaternion()) as GameObject;
                if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
                else { instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal); }
                if (!UseWorldSpacePosition) instance.transform.parent = transform;
                Destroy(instance, DestroyTimeDelay);
            }
        }
        if (DestroyMainEffect == true)
        {
            Destroy(gameObject, DestroyTimeDelay + 0.5f);
        }
    }

    IEnumerator Damaging()
    {
        while (true)
        {
            var hits = Physics.SphereCastAll(transform.position, 2f, Vector3.up, 1f);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.tag == "GroundEnemy")
                {
                    hits[i].transform.GetComponent<GroundEnemy>().GetDamage(hits[i].transform.GetComponent<GroundEnemy>().maxHP * damage / 100);
                } 
                else if (hits[i].transform.tag == "FlyingEnemy")
                {
                    hits[i].transform.GetComponent<FlyingEnemy>().GetDamage(hits[i].transform.GetComponent<FlyingEnemy>().maxHP * damage / 100);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
