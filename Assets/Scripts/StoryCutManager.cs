using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StoryCutManager : MonoBehaviour
{
    public Image[] images;
    Coroutine fadeCoroutine = null;
    int index = -1;
    // Start is called before the first frame update
    void Start()
    {
        index = -1;
        fadeCoroutine = null;
        showNextImage();
    }

    public void OnClickSkipButton()
    {
        FinishSlide();
    }

    public void OnClickNextButton()
    {
        showNextImage();
        Debug.Log(fadeCoroutine == null);
    }

    void FinishSlide()
    {
        Global.userProperty.startStoryFinishFlag = true;
        GameObject.Find("SaveLoadManager")?.GetComponent<SaveLoadManager>().Save();
        SceneLoader.LoadScene("StageSelectScene");
    }

    public void showNextImage()
    {
        if (fadeCoroutine == null)
        {
            fadeCoroutine = StartCoroutine(nextImage());
        }
    }

    IEnumerator nextImage()
    {
        index++;
        if (index > 0)
        {
            for (int i = 5; i >= 0; i--)
            {
                images[index - 1].color = new Color(i / 5f, i / 5f, i / 5f);
                yield return new WaitForSeconds(0.1f);
            }
            images[index - 1].gameObject.SetActive(false);
        }
        if (index == images.Length)
        {
            FinishSlide();
            yield break;
        }
        images[index].gameObject.SetActive(true);
        for (int i = 0; i <= 5; i++)
        {
            images[index].color = new Color(i / 5f, i / 5f, i / 5f);
            yield return new WaitForSeconds(0.1f);
        }
        fadeCoroutine = null;
    }
}
