using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class SkillManager : MonoBehaviour
{
    public List<GameObject> my_skill;
    public List<GameObject> equipped_skill;

    [SerializeField] GameObject inGameGrid;
    [SerializeField] GameObject SelectGrid;

    [SerializeField] GameObject[] skills;

    public void AddSkill(GameObject skill)
    {
        if (my_skill.Contains(skill))
            return;
        my_skill.Add(skill);
        initInven();
    }

    public void initInven()
    {
        for (int i = 0; i < SelectGrid.transform.childCount; i++)
        {
            Destroy(SelectGrid.transform.GetChild(i).gameObject);
        }

        for (int index = 0; index < my_skill.Count; index++)
        {
            GameObject skill = Instantiate(my_skill[index]);
            skill.transform.SetParent(SelectGrid.transform, false);
        }
    }

    /*private void OnEnable()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(PlayerPrefs.GetString("selectedSkill" + (i + 1).ToString()));
        }
    }*/

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.GetString("selectedSkill" + (i + 1).ToString()) != "")
            {
                for (int index = 0; index < my_skill.Count; index++)
                {
                    if (my_skill[index].GetComponent<SkillInven>().skillName == PlayerPrefs.GetString("selectedSkill" + (i + 1).ToString()))
                    {
                        my_skill[index].GetComponent<SkillInven>().Setup();
                    }
                }
            }
        }
    }

}
