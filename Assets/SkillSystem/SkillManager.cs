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
    public List<GameObject> info_skill;

    [SerializeField] GameObject infoGrid;
    [SerializeField] GameObject SelectGrid;

    [SerializeField] GameObject[] skills;

    ToggleGroup _toggleGroup;

    //UI
    [SerializeField] Text _levelTxt;
    [SerializeField] Text _InfoTxt;
    [SerializeField] Text _costTxt;
    [SerializeField] GameObject _isSkill;
    [SerializeField] GameObject _emptySkill;

    GameObject skill;
    int cost;

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
        _toggleGroup = GetComponent<ToggleGroup>();

        if (!_toggleGroup.AnyTogglesOn())
        {
            _isSkill.SetActive(false);
            _emptySkill.SetActive(true);
        }
            

        //Equip 세팅
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

        //PlayerPref 세팅
        for(int i = 0; i < info_skill.Count; i++)
        {
            if(PlayerPrefs.GetInt(info_skill[i].GetComponent<SkillInven>().skillGrade) == 0)
            {
                PlayerPrefs.SetInt(info_skill[i].GetComponent<SkillInven>().skillGrade, 1);
                PlayerPrefs.Save();
            }
            else
            {
                info_skill[i].GetComponent<SkillInven>()._upgraded = PlayerPrefs.GetInt(info_skill[i].GetComponent<SkillInven>().skillGrade);
            }
        }
    }

    public void InfoSetting()
    {
        _isSkill.SetActive(true);
        _emptySkill.SetActive(false);
        for (int i = 0; i < my_skill.Count; i++)
        {
            if (my_skill[i].GetComponent<Toggle>().isOn)
            {
                info_skill[i].SetActive(true);
                _levelTxt.text = "LV"+info_skill[i].GetComponent<SkillInven>()._upgraded.ToString();
                _InfoTxt.text = info_skill[i].GetComponent<SkillInven>().info;
                _costTxt.text = info_skill[i].GetComponent<SkillInven>()._UpgradeCost[info_skill[i].GetComponent<SkillInven>()._upgraded - 1].ToString();
                cost = info_skill[i].GetComponent<SkillInven>()._UpgradeCost[info_skill[i].GetComponent<SkillInven>()._upgraded - 1];
            }
            else
            {
                info_skill[i].SetActive(false);
            }
        }
    }

    public void UpgradeSpell()
    {
        if (Global.userProperty.gold < cost)
            return;
        Global.userProperty.gold -= cost;
        GameObject.Find("StageSelectManager").GetComponent<StageSelectSceneManager>().RefreshUserPropertyData();
        GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().Save();
        for (int i = 0; i < info_skill.Count; i++)
            if (info_skill[i].activeInHierarchy)
                skill = info_skill[i];
        if (skill.GetComponent<SkillInven>()._upgraded > 19)
            return;
        skill.GetComponent<SkillInven>()._upgraded += 1;
        PlayerPrefs.SetInt(skill.GetComponent<SkillInven>().skillGrade, skill.GetComponent<SkillInven>()._upgraded);
        skill.GetComponent<SkillInven>().CalCool();
        PlayerPrefs.Save();
    }


}
