using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectToggle : MonoBehaviour
{
    public int chapter;
    public int stage;
    public Image selectedFrameImage;
    private float selectedTransitionFadeTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Toggle toggleComponent = gameObject.GetComponent<Toggle>();
        toggleComponent.onValueChanged.AddListener(
            (bool isOn) => { OnToggleSelected(isOn); });
        toggleComponent.isOn = false;
        if (Global.isDebug == false)
        {
            Toggle toggle = gameObject.GetComponent<Toggle>();
            if (Global.userProperty.LastReachedChapter > this.chapter ||
            (Global.userProperty.LastReachedChapter == this.chapter && Global.userProperty.LastReachedStage >= this.stage - 1) ||
            (Global.userProperty.LastReachedChapter + 1 == this.chapter && Global.userProperty.LastReachedStage == 5 && this.stage == 1) ||
            (Global.userProperty.LastReachedStage == 0 /*초기 실행*/ && this.chapter == 1 && this.stage == 1))
            {
                toggle.interactable = true;
            }
            else
            {
                toggle.interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnToggleSelected(bool isOn)
    {
        if (isOn)
        {
            StageSelectSceneManager stageSelectManager
                = GameObject.Find("StageSelectManager").GetComponent<StageSelectSceneManager>();
            stageSelectManager.selectedStage = this.stage;
            Global._chapter = this.chapter;
            Global._stage = this.stage;
            ToggleGroup toggleGroup = gameObject.GetComponentInParent<ToggleGroup>();
            toggleGroup.allowSwitchOff = false;
            Debug.Log("chapter:" + chapter + ", stage:" + stage);
            // TODO: 선택된 것 표시
            selectedFrameImage.CrossFadeColor(Color.white, selectedTransitionFadeTime, false, true);
        }
        else
        {
            // TODO: 원래대로 되돌리기
            selectedFrameImage.CrossFadeColor(Color.clear, selectedTransitionFadeTime, false, true);
        }
    }
}
