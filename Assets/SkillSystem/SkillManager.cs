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

    public void equipSkill(GameObject skill)
    {
        equipped_skill.Add(skill);
    }

    public void unequipSkill(GameObject skill)
    {
        equipped_skill.Remove(skill);
    }

    public void AddSkill(GameObject skill)
    {
        if (my_skill.Contains(skill))
            return;
        my_skill.Add(skill);
        initInven();
    }
    
    public void initInven()
    {
        for(int i = 0; i < SelectGrid.transform.childCount; i++)
        {
            Destroy(SelectGrid.transform.GetChild(i).gameObject);
        }

        for(int index = 0; index < my_skill.Count; index++)
        {
            GameObject skill = Instantiate(my_skill[index]);
            skill.transform.SetParent(SelectGrid.transform, false);
        }
    }

}
