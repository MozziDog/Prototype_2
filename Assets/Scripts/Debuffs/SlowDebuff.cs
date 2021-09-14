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
    public GameObject WhoCastDebuff; //info about the tower which cast debuff to this enemy, this variable is remained for debug, but not used for now..
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
        Debug.Log("slow debuff ERASED!");
        this.gameObject.GetComponent<EnemyInterFace>().SetSpeed(originTimeScale);
        Destroy(effect);
        Destroy(thisDebuff);
    }

    public void RefreshSlow(float LV,float slowIntensity,GameObject SlowFx,GameObject WhoCastDebuff)
    {
        Debug.Log("slow debuff refreshed!");
        this.LV = LV;
        this.slowIntensity = slowIntensity;
        this.SlowFx = SlowFx;
        this.WhoCastDebuff = WhoCastDebuff;
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
