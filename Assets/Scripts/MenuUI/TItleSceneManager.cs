using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TItleSceneManager : MonoBehaviour
{
    public GameObject touchToStartText;
    public GameObject fader;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("ManagerScene", LoadSceneMode.Additive);
        touchToStartBaseAnimCoroutine = StartCoroutine(TouchToStartBaseAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount != 0)
        {
            if (touchToStartBaseAnimCoroutine != null)
                StopCoroutine(touchToStartBaseAnimCoroutine);
            touchToStartBaseAnimCoroutine = null;
            GoToStageSelectSceneCoroutine = StartCoroutine(GoToStageSelectScene());
        }
    }

    Coroutine touchToStartBaseAnimCoroutine = null;
    IEnumerator TouchToStartBaseAnim()
    {
        Text textComponent = touchToStartText.GetComponent<Text>();
        float blinkTime = 0.6f;
        while (true)
        {
            textComponent.color = new Color(1, 1, 1, 255);
            yield return new WaitForSeconds(blinkTime);
            textComponent.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(blinkTime);
        }
    }

    Coroutine GoToStageSelectSceneCoroutine = null;
    IEnumerator GoToStageSelectScene()
    {
        Text textComponent = touchToStartText.GetComponent<Text>();
        float blinkTime = 0.1f;
        for (int i = 0; i < 4; i++)
        {
            textComponent.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(blinkTime);
            textComponent.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(blinkTime);
        }
        for (int i = 0; i < 6; i++)
        {
            fader.GetComponent<Image>().color = new Color(0, 0, 0, i / 5f);
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadScene("StageSelectScene");
        GoToStageSelectSceneCoroutine = null;
        yield break;
    }
}
