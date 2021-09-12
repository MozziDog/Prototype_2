using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class SkillManager : MonoBehaviour
{
    public List<Toggle> _toggle;
    public List<GameObject> _skill;

    [SerializeField] GameObject _skillToggle;
    [SerializeField] GameObject _toggleGrid;


    void MySkill()
    {

    }

    void InitToggle()
    {
        GameObject toggle = Instantiate(_skillToggle);
        toggle.transform.SetParent(_toggleGrid.transform, false);
        _toggle.Add(toggle.GetComponent<Toggle>());
    }
}
