using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLoader : MonoBehaviour
{
    [SerializeField] GameObject Spell_grid;
    [SerializeField] GameObject[] all_skills;
    [SerializeField] GameObject[] equipped_skills;
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.GetString("selectedSkill" + (i + 1).ToString()) != "")
            {
                for (int index = 0; index < all_skills.Length; index++)
                {
                    if (all_skills[index].transform.gameObject.name == PlayerPrefs.GetString("selectedSkill" + (i + 1).ToString()))
                    {
                        Debug.Log(index);
                        Debug.Log(i);
                        equipped_skills[i] = all_skills[index];
                    }
                }
            }
        }

        for (int j = 0; j < equipped_skills.Length; j++)
        {
            if (equipped_skills[j] != null)
            {
                GameObject skill = Instantiate(equipped_skills[j]);
                skill.transform.SetParent(Spell_grid.transform, false);
            }

        }
    }


}
