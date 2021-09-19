using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerUp : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Image _cool_Img;
    [SerializeField] GameObject _cool_txt;
    [SerializeField] GameObject _Cool_Info_txt;

    [SerializeField] float coolTime;
    public bool isClicked = false;
    [SerializeField] float leftTime;

    public bool isSpellMode = false;

    [SerializeField] GameObject _rageSkill;
    [SerializeField] GameObject _skillIndicator;

    [SerializeField] int _upgraded = 1;

    AudioSource _audio;

    GameObject _indicator;

    private void Start()
    {
        _upgraded = PlayerPrefs.GetInt("grade_PowerUp");
        coolTime = 15f - (+_upgraded - 1) * 2f / 19f;
        _rageSkill.GetComponent<RageSkill>().rageAmount = 1f + (_upgraded - 1) * 3f / 19f;
        _Cool_Info_txt.GetComponent<Text>().text = string.Format("{0:0.#}", coolTime) + "초";
        _audio = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (isClicked)
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                if (leftTime <= 0)
                {
                    _cool_txt.SetActive(false);
                    leftTime = 0f;
                    if (_button)
                        _button.enabled = true;
                    isClicked = true;
                }

                float ratio = leftTime / coolTime;
                if (_cool_Img)
                {
                    _cool_Img.fillAmount = ratio;
                }
                _cool_txt.GetComponent<Text>().text = Mathf.Floor(leftTime).ToString() + " 초";
            }

        if (isSpellMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
            {
                Vector3 targetPosition = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.5f, Mathf.Floor(hit.point.z) + 0.5f);
                if (Input.GetMouseButtonDown(0))
                {
                    _indicator = Instantiate(_skillIndicator, targetPosition, Quaternion.identity);
                }
                if (Input.GetMouseButton(0))
                {
                    _indicator.transform.position = targetPosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Destroy(_indicator);
                    Instantiate(_rageSkill, targetPosition, Quaternion.identity);
                    _audio.Play();
                    StartCoolTime();
                }
            }
            else if (Input.GetMouseButton(0))
            {
                isSpellMode = false;
                Destroy(_indicator);
            }
        }
    }

    public void StartCoolTime()
    {
        isSpellMode = false;
        leftTime = coolTime;
        isClicked = true;
        if (_button)
            _button.enabled = false;
        _cool_txt.SetActive(true);
    }

    public void EnterSpell()
    {
        isSpellMode = true;
    }
}
