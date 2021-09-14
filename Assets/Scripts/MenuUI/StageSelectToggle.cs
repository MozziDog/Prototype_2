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
            stageSelectManager.selectedChapter = chapter;
            stageSelectManager.selectedStage = stage;
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
