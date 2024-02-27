using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsFeedBack : MonoBehaviour
{
    private static SkillsFeedBack instance;

    public TextMeshProUGUI feedbackText; // Unity�༭����ָ����UI Text����

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

    // �����ı���ʾ�����λ��
    private void UpdateFeedbackTextPosition()
    {
        if (feedbackText != null)
        {
            // ���ı���ʾ�����λ������Ϊ��ɫλ���Ϸ�һ������
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

            // ��һ��ʱ���ر��ı���ʾ
            StartCoroutine(HideFeedbackText());
        }
    }

    private IEnumerator HideFeedbackText()
    {
        yield return new WaitForSeconds(2f); // �����ı���ʾʱ��
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }

    #region ��֭����
    [Header("DouzhiSkill")]
    public GameObject speedReductionParticles;
    public GameObject damageIncreaseParticles;

    private GameObject _speedDown;
    private GameObject _damageUp;

    public void ShowSpeedReductionFeedback()
    {
        if(_speedDown == null)
        {
            Debug.Log("��ʼ���ƶ��ٶ��½�����");
            _speedDown = Instantiate(speedReductionParticles,transform.position,Quaternion.identity);
            _speedDown.transform.position = transform.position;
            _speedDown.transform.parent = transform;
        }
        Debug.Log("�����ƶ��ٶ��½�����");
        _speedDown.GetComponent<ParticleSystem>().Play();
    }

    public void ShowDamageIncreaseFeedback()
    {
        if (_damageUp == null)
        {
            Debug.Log("��ʼ����������������");
            _damageUp = Instantiate(damageIncreaseParticles, transform.position, Quaternion.identity);
            _damageUp.transform.position = transform.position;
            _damageUp.transform.parent = transform;
        }
        Debug.Log("���Ź�������������");
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

    #region ���Ϸۼ���
    [Header("LuosifenSkill")]
    public GameObject luosifenAreaPrefab;
    private GameObject _luosifenArea;

    public void ShowLuosifenArea()
    {
        if (_luosifenArea == null)
        {
            Debug.Log("��ʼ�����Ϸ����򶯻�");
            _luosifenArea = Instantiate(luosifenAreaPrefab, transform.position, Quaternion.identity);
            _luosifenArea.transform.position = transform.position;
            _luosifenArea.transform.parent = transform;
        }
        Debug.Log("�������Ϸ����򶯻�");
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

    #region ��Ь����
    [Header("PaoxieSkill")]
    public GameObject _speedupPrefab;
    private GameObject _speedup;
    public void ShowSpeedupFeedback() {
        if (_speedup == null)
        {
            Debug.Log("��ʼ����Ь���ٶ���");
            _speedup = Instantiate(_speedupPrefab, transform.position, Quaternion.identity);
            _speedup.transform.position = transform.position;
            _speedup.transform.parent = transform;
        }
        Debug.Log("������Ь���ٶ���");
        _speedup.GetComponent<ParticleSystem>().Play();
    }
    public void StopSpeedupFeedback() {
        if(_speedup != null)
        {
            _speedup.GetComponent<ParticleSystem>().Stop();
        }
    }
    #endregion

    #region ������Ϸ����
    [Header("GameSkill")]
    public GameObject _attackspeedupPrefab;
    private GameObject _attackspeedup;
    public void ShowAttackSpeedupFeedback()
    {
        if (_attackspeedup == null)
        {
            Debug.Log("��ʼ���������Ӷ���");
            _attackspeedup = Instantiate(_attackspeedupPrefab, transform.position, Quaternion.identity);
            GameObject weaponObject = GameObject.Find("KeyBoard");
            if (weaponObject != null)
            {
                _attackspeedup.transform.position = weaponObject.transform.position;
                _attackspeedup.transform.parent = weaponObject.transform;
            }
            else
            {
                Debug.LogWarning("δ�ҵ���Ϊ 'Weapon' �Ķ���");
            }
        }
        Debug.Log("���Ź������Ӷ���");
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

    #region ���������
    [Header("HushenfuSkill")]
    public GameObject _avoidRateupPrefab;
    private GameObject _avoidRateup;
    public void ShowAvoidRateupFeedback()
    {
        if (_avoidRateup == null)
        {
            Debug.Log("��ʼ���������Ӷ���");
            _avoidRateup = Instantiate(_avoidRateupPrefab, transform.position, Quaternion.identity);
            _avoidRateup.transform.position = transform.position;
            _avoidRateup.transform.parent = transform;
        }
        Debug.Log("�����������Ӷ���");
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
