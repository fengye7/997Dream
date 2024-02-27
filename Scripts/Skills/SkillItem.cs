using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class SkillItem : MonoBehaviour
{
    public string skillName;
    public Sprite skillAvatar;
    public float destroyDelay = 2f; // 自动销毁延迟时间

    [Header("BlinkInfo")]
    public float blinkDuration = 1f;
    public int blinkCount = 5;
    public SpriteRenderer spriteRenderer;

    PlayerSkills skills;

    private void Start()
    {
        // 启动协程，在一定时间后销毁游戏对象
        StartCoroutine(DestroyAfterDelay(destroyDelay));
        skills = FindObjectOfType<PlayerSkills>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("添加前"+DataHolder.Instance.skillList.Count);
            if (CheckSkillNew(skillName))
            {
                DataHolder.Instance.skillList.AddLast(new SkillData(skillName, skillAvatar)); // 添加技能到玩家技能库
                skills.UpdateSkillUI();//更新技能槽
            }
            Debug.Log("添加后" + DataHolder.Instance.skillList.Count);
            DreamSceneAudios.Instance.PlayFetchAudio();
            if (spriteRenderer != null) 
            {
                Utils.BlinkEffect(spriteRenderer, blinkDuration, blinkCount);
            }
            Destroy(gameObject); // 销毁掉落物品
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
        Destroy(gameObject); // 销毁掉落物品
    }
}
