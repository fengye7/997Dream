using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class SkillItem : MonoBehaviour
{
    public string skillName;
    public Sprite skillAvatar;
    public float destroyDelay = 2f; // �Զ������ӳ�ʱ��

    [Header("BlinkInfo")]
    public float blinkDuration = 1f;
    public int blinkCount = 5;
    public SpriteRenderer spriteRenderer;

    PlayerSkills skills;

    private void Start()
    {
        // ����Э�̣���һ��ʱ���������Ϸ����
        StartCoroutine(DestroyAfterDelay(destroyDelay));
        skills = FindObjectOfType<PlayerSkills>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("���ǰ"+DataHolder.Instance.skillList.Count);
            if (CheckSkillNew(skillName))
            {
                DataHolder.Instance.skillList.AddLast(new SkillData(skillName, skillAvatar)); // ��Ӽ��ܵ���Ҽ��ܿ�
                skills.UpdateSkillUI();//���¼��ܲ�
            }
            Debug.Log("��Ӻ�" + DataHolder.Instance.skillList.Count);
            DreamSceneAudios.Instance.PlayFetchAudio();
            if (spriteRenderer != null) 
            {
                Utils.BlinkEffect(spriteRenderer, blinkDuration, blinkCount);
            }
            Destroy(gameObject); // ���ٵ�����Ʒ
        }
    }

    bool CheckSkillNew(string name)
    {
        bool isNew = true;
        foreach (SkillData skill in DataHolder.Instance.skillList)
        {
            if(skill.skillName == name)
            {
                isNew = false;
                break;
            }
        }
        return isNew;
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject); // ���ٵ�����Ʒ
    }
}
