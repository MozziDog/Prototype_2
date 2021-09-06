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
    public float damage = 10f;

    void Start()
    {
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
        if(other.GetComponent<GroundEnemy>()) 
        {
            //Debug.Log("Attacked!");
            other.GetComponent<GroundEnemy>().GetDamage(other.GetComponent<GroundEnemy>().maxHP * 0.20f);
        } 
        else if (other.GetComponent<FlyingEnemy>())
        {
            other.GetComponent<FlyingEnemy>().GetDamage(damage);
        }
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
}
