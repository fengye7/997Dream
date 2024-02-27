using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsFeedBack : MonoBehaviour
{
    private static SkillsFeedBack instance;

    public TextMeshProUGUI feedbackText; // Unity编辑器中指定的UI Text对象

    public static SkillsFeedBack Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillsFeedBack>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("SkillsFeedBack");
                    instance = obj.AddComponent<SkillsFeedBack>();
                }
            }
            return instance;
        }
    }

    // 更新文本提示对象的位置
    private void UpdateFeedbackTextPosition()
    {
        if (feedbackText != null)
        {
            // 将文本提示对象的位置设置为角色位置上方一定距离
            feedbackText.transform.position = transform.position + Vector3.up * 2f;
        }
    }

    public void ShowFeedbackText(string feedbackMessage)
    {
        if (feedbackText != null)
        {
            UpdateFeedbackTextPosition();
            feedbackText.text = feedbackMessage;
            feedbackText.gameObject.SetActive(true);

            // 在一定时间后关闭文本提示
            StartCoroutine(HideFeedbackText());
        }
    }

    private IEnumerator HideFeedbackText()
    {
        yield return new WaitForSeconds(2f); // 设置文本显示时间
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }

    #region 豆汁技能
    [Header("DouzhiSkill")]
    public GameObject speedReductionParticles;
    public GameObject damageIncreaseParticles;

    private GameObject _speedDown;
    private GameObject _damageUp;

    public void ShowSpeedReductionFeedback()
    {
        if(_speedDown == null)
        {
            Debug.Log("初始化移动速度下降动画");
            _speedDown = Instantiate(speedReductionParticles,transform.position,Quaternion.identity);
            _speedDown.transform.position = transform.position;
            _speedDown.transform.parent = transform;
        }
        Debug.Log("播放移动速度下降动画");
        _speedDown.GetComponent<ParticleSystem>().Play();
    }

    public void ShowDamageIncreaseFeedback()
    {
        if (_damageUp == null)
        {
            Debug.Log("初始化攻击力上升动画");
            _damageUp = Instantiate(damageIncreaseParticles, transform.position, Quaternion.identity);
            _damageUp.transform.position = transform.position;
            _damageUp.transform.parent = transform;
        }
        Debug.Log("播放攻击力上升动画");
        _damageUp.GetComponent<ParticleSystem>().Play();
    }

    public void StopDouzhiFeedback()
    {
        if (_speedDown != null)
        {
            _speedDown.GetComponent<ParticleSystem>().Stop();
        }
        if (_damageUp != null)
        {
            _damageUp.GetComponent<ParticleSystem>().Stop();
        }
    }
    #endregion

    #region 螺蛳粉技能
    [Header("LuosifenSkill")]
    public GameObject luosifenAreaPrefab;
    private GameObject _luosifenArea;

    public void ShowLuosifenArea()
    {
        if (_luosifenArea == null)
        {
            Debug.Log("初始化螺蛳粉区域动画");
            _luosifenArea = Instantiate(luosifenAreaPrefab, transform.position, Quaternion.identity);
            _luosifenArea.transform.position = transform.position;
            _luosifenArea.transform.parent = transform;
        }
        Debug.Log("播放螺蛳粉区域动画");
        _luosifenArea.GetComponent<ParticleSystem>().Play();
    }

    public void CloseLuosifenArea()
    {
        if(_luosifenArea != null)
        {
            _luosifenArea.GetComponent<ParticleSystem>().Stop();
        }
    }

    #endregion

    #region 跑鞋技能
    [Header("PaoxieSkill")]
    public GameObject _speedupPrefab;
    private GameObject _speedup;
    public void ShowSpeedupFeedback() {
        if (_speedup == null)
        {
            Debug.Log("初始化跑鞋加速动画");
            _speedup = Instantiate(_speedupPrefab, transform.position, Quaternion.identity);
            _speedup.transform.position = transform.position;
            _speedup.transform.parent = transform;
        }
        Debug.Log("播放跑鞋加速动画");
        _speedup.GetComponent<ParticleSystem>().Play();
    }
    public void StopSpeedupFeedback() {
        if(_speedup != null)
        {
            _speedup.GetComponent<ParticleSystem>().Stop();
        }
    }
    #endregion

    #region 电子游戏技能
    [Header("GameSkill")]
    public GameObject _attackspeedupPrefab;
    private GameObject _attackspeedup;
    public void ShowAttackSpeedupFeedback()
    {
        if (_attackspeedup == null)
        {
            Debug.Log("初始化攻速增加动画");
            _attackspeedup = Instantiate(_attackspeedupPrefab, transform.position, Quaternion.identity);
            GameObject weaponObject = GameObject.Find("KeyBoard");
            if (weaponObject != null)
            {
                _attackspeedup.transform.position = weaponObject.transform.position;
                _attackspeedup.transform.parent = weaponObject.transform;
            }
            else
            {
                Debug.LogWarning("未找到名为 'Weapon' 的对象");
            }
        }
        Debug.Log("播放攻速增加动画");
        _attackspeedup.GetComponent<ParticleSystem>().Play();
    }
    public void StopAttackSpeedupFeedback()
    {
        if (_attackspeedup != null)
        {
            _attackspeedup.GetComponent<ParticleSystem>().Stop();
        }
    }
    #endregion

    #region 护身符技能
    [Header("HushenfuSkill")]
    public GameObject _avoidRateupPrefab;
    private GameObject _avoidRateup;
    public void ShowAvoidRateupFeedback()
    {
        if (_avoidRateup == null)
        {
            Debug.Log("初始化闪避增加动画");
            _avoidRateup = Instantiate(_avoidRateupPrefab, transform.position, Quaternion.identity);
            _avoidRateup.transform.position = transform.position;
            _avoidRateup.transform.parent = transform;
        }
        Debug.Log("播放闪避增加动画");
        _avoidRateup.GetComponent<ParticleSystem>().Play();
    }
    public void StopAvoidRateupFeedback()
    {
        if (_avoidRateup != null)
        {
            _avoidRateup.GetComponent<ParticleSystem>().Stop();
        }
    }
    #endregion


}
