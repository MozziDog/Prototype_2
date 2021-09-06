using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : MonoBehaviour
{
    // Start is called before the first frame update
    public float LV;
    public float originTimeScale;
    public float debuffedTimeScale;
    public float slowIntensity;
    public GameObject WhoCastDebuff;
    public GameObject SlowFx;
    private SlowDebuff thisDebuff;


    GameObject effect;
    public void SetUp(float LV,float slowIntensity, GameObject WhoCastDebuff, GameObject SlowFx)
    {
        this.LV = LV;
        thisDebuff = this.gameObject.GetComponent<SlowDebuff>();
        this.originTimeScale = this.gameObject.GetComponent<EnemyInterFace>().GetSpeed();
        this.slowIntensity = slowIntensity;
        this.WhoCastDebuff = WhoCastDebuff;
        this.SlowFx = SlowFx;
    }

    public void ExecuteDebuff()
    {
        debuffedTimeScale = originTimeScale * slowIntensity;
        this.gameObject.GetComponent<EnemyInterFace>().SetSpeed(debuffedTimeScale);
        effect = Instantiate(SlowFx, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity);
        effect.transform.SetParent(this.gameObject.transform);
    }

    public void EraseDebuff()
    {
        this.gameObject.GetComponent<EnemyInterFace>().SetSpeed(originTimeScale);
        Destroy(effect);
        Destroy(thisDebuff);
    }

    public void RefreshSlow(float LV,float slowIntensity)
    {
        this.LV = LV;
        this.slowIntensity = slowIntensity;
        Destroy(effect);
        ExecuteDebuff();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
