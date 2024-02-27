using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillPrefabEntry
{
    public string skillName;
    public GameObject skillPrefab;
}

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public int damage;
    public Sprite weaponAvatar;
    // 其他武器属性...

    public WeaponData(string name, int dmg, Sprite avatar)
    {
        weaponName = name;
        damage = dmg;
        weaponAvatar = avatar;
    }
}

[System.Serializable]
public class SkillData
{
    public string skillName;
    public Sprite skillAvatar;
    public SkillData(string name, Sprite avatar)
    {
        skillName = name; skillAvatar = avatar;
    }
}

public class PlayerAttackSystem : MonoBehaviour
{
    [Header("UIInfo")]
    public HorizontalLayoutGroup weaponpoolHorizontalLayoutGroup;
    public TextMeshProUGUI bulletLevelText;
    public TextMeshProUGUI itemPoolText;

    public float avatarWidth = 50;
    public float avatarHeight = 200;

    [Header("WeaponPoolInfo")]
    public Scrollbar weaponpoolScrollbar;
    public float fillSpeed = 0.9f; // 每秒增加的填充速度

    [Header("ExperienceInfo")]
    public Slider experienceSlider; // 经验条UI
    public float experience = 0; // 玩家经验值
    public float experienceToLevelUp = 100; // 升级所需经验值


    [Header("StressInfo")]
    public float initialStress = 100;  // 初始压力值
    public float maxStress = 200; // 能够承受的极限压力值
    public float knockbackDistance = 0.5f;  // 击退的距离
    public float knockbackDuration = 0.1f; // 击退动画持续时间
    public Slider stressSlider;// 压力值UI
    private const float lerpDuration = 1f; // 缓慢靠近的持续时间
    private Coroutine lerpCoroutine; // 靠近协程

    [Header("CameraInfo")]
    public float shakeDuration = 0.1f;
    public float shakeStrength = 0.5f;


    [Header("SkillInfo")]
    public Slider itemSlider; // 道具项的解锁进度条
    public int itemToUnlock = 100; // 解锁所需的进度
    public int unlockValue = 0; //当前的解锁进度
    public int itemLevel = 0;

    [Header("Other")]
    public float avoidRate = 0.1f;

    [SerializeField]
    private List<SkillPrefabEntry> skillPrefabsList;
    public Dictionary<string, GameObject> skillPrefabs;

    private Coroutine knockbackCoroutine; // 击退协程

    private void Awake()
    {
        // 将列表转换为字典以供使用
        skillPrefabs = new Dictionary<string, GameObject>();
        foreach (var entry in skillPrefabsList)
        {
            skillPrefabs[entry.skillName] = entry.skillPrefab;
            DataHolder.Instance.availableSkillList.Add(new SkillData(entry.skillName, entry.skillPrefab.GetComponent<SkillItem>().skillAvatar));
        }
        foreach (var itemname in RealityData.Instance._itemList)
        {
            DataHolder.Instance.availableSkillList.Remove(new SkillData(itemname, skillPrefabs[itemname].GetComponent<SkillItem>().skillAvatar));
            DataHolder.Instance.availableSkillList.Add(new SkillData(itemname, skillPrefabs[itemname].GetComponent<SkillItem>().skillAvatar));
        }
        Utils.ShuffleList(DataHolder.Instance.availableSkillList);
        InitialItemPolol();
    }

    public void InitialItemPolol()
    {
        // 将可用武器池上的武器添加到列表中
        foreach (SkillData skill in DataHolder.Instance.availableSkillList)
        {
            // 实例化武器头像
            GameObject skillAvatarObj = new GameObject(skill.skillName + "Avatar");
            // 将武器头像作为 horizontalLayoutGroup 的子对象
            skillAvatarObj.transform.SetParent(weaponpoolHorizontalLayoutGroup.transform, false);

            Image skillAvatarImage = skillAvatarObj.AddComponent<Image>();
            skillAvatarImage.type = Image.Type.Filled;
            skillAvatarImage.fillAmount = 0;
            skillAvatarImage.sprite = skill.skillAvatar;  // 设置武器头像的 Sprite
            skillAvatarImage.rectTransform.sizeDelta = new Vector2(avatarWidth, avatarHeight); // ��������ͷ��ĳߴ�

        }
    }

    private void Start()
    {
        DataHolder.Instance.stressValue = initialStress; // 初始化当前压力值为初始压力值

        stressSlider.maxValue = maxStress;
        stressSlider.value = DataHolder.Instance.stressValue;

        experienceSlider.maxValue = experienceToLevelUp;
        experienceSlider.value = experience;

        itemSlider.maxValue = itemToUnlock;
        itemSlider.value = unlockValue;

        UpdateItemPoolUI(itemLevel);
    }


    public void GainExperience(int amount)
    {
        experience += amount;
        experienceSlider.value = experience;

        unlockValue += amount;
        itemSlider.value = unlockValue;

        if (experience >= experienceToLevelUp)
        {
            LevelUp();
        }
        if(unlockValue >= itemToUnlock)
        {
            int spwanLevel = UnityEngine.Random.Range(0, itemLevel);
            if(DataHolder.Instance.availableSkillList.Count!=0)
                SpawnSkills(DataHolder.Instance.availableSkillList[spwanLevel].skillName);
            itemSlider.value = 0;
            unlockValue = 0;
            itemLevel += 1;
            if(itemLevel >= DataHolder.Instance.availableSkillList.Count)
                itemLevel = DataHolder.Instance.availableSkillList.Count;
            UpdateItemPoolUI(itemLevel);
        }
    }

    /// <summary>
    /// 更新可用武器池的UI显示
    /// </summary>
    /// <param name="level"></param>
    void UpdateItemPoolUI(int level)
    {
        StartCoroutine(GraduallyFillImage(level));
        if(itemPoolText != null)
        {
            itemPoolText.text = "ItemPoolLevel:" + (level+1);
        }
        weaponpoolScrollbar.value = level / (DataHolder.Instance.availableSkillList.Count==0?1: DataHolder.Instance.availableSkillList.Count);
    }

    IEnumerator GraduallyFillImage(int level)
    {
        if (weaponpoolHorizontalLayoutGroup != null)
        {
            if (level >= weaponpoolHorizontalLayoutGroup.transform.childCount)
                level = weaponpoolHorizontalLayoutGroup.transform.childCount - 1;

            Transform child = weaponpoolHorizontalLayoutGroup.transform.GetChild(level);
            GameObject childObject = child.gameObject;
            Image image = childObject.GetComponent<Image>();
            if (image != null)
            {
                while (image.fillAmount < 1)
                {
                    image.fillAmount += fillSpeed * Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    private void LevelUp()
    {
        Debug.Log("弹道升级");
        DataHolder.Instance.bulletLevel++; // 子弹等级升级
        if (DataHolder.Instance.bulletLevel >= 20)
            DataHolder.Instance.bulletLevel = 20;//设定的最大值
      // TODO: 更新子弹弹道为散射,这个在绑定武器KeyBoardWeapon上实现了
        if (bulletLevelText!= null)
        {
            bulletLevelText.text = "子弹等级：" + DataHolder.Instance.bulletLevel;
        }
        experience -= experienceToLevelUp;
        experienceToLevelUp *= 2; // 升级所需经验值翻倍
    }

    // 接收伤害
    public void TakeDamage(int damage, Vector3 knockbackDirection)
    {
        if (UnityEngine.Random.value < avoidRate)
        {
            // 玩家成功闪避伤害，不执行后续的受伤逻辑
            Debug.Log("Player dodged the attack!");
            return;
        }
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpStressSlider(DataHolder.Instance.stressValue + damage));

        // 如果当前压力值大于最大压力值，玩家死亡，游戏结束
        if (DataHolder.Instance.stressValue >= maxStress)
        {
            Utils.GameOver("123");
        }
        else
        {
            DreamSceneAudios.Instance.PlayInjuredAudio();
            // 执行受伤效果
            if (knockbackCoroutine != null)
            {
                StopCoroutine(knockbackCoroutine);// 停止之前的后退协程
                CameraController.Instance.CameraShake(shakeDuration, shakeStrength);
            }
            knockbackCoroutine = StartCoroutine(KnockbackEffect(knockbackDirection));// 开始新的后退效果
        }
    }

    /// <summary>
    /// 实现进度条缓慢变化
    /// </summary>
    /// <param name="targetStress"></param>
    /// <returns></returns>
    IEnumerator LerpStressSlider(float targetStress)
    {
        float startStress = Mathf.RoundToInt(stressSlider.value);
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;
            int lerpedStress = Mathf.RoundToInt(Mathf.Lerp(startStress, targetStress, t));
            stressSlider.value = Mathf.Clamp(lerpedStress, 0, maxStress);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终值准确
        stressSlider.value = targetStress;
        DataHolder.Instance.stressValue = targetStress;
    }

    /// <summary>
    ///  击败enemy释放压力值
    /// </summary>
    /// <param name="amount"></param>
    public void ReleaseStress(int amount)
    {
        DataHolder.Instance.stressValue -= amount;
        lerpCoroutine = StartCoroutine(LerpStressSlider(DataHolder.Instance.stressValue + amount));

        if (DataHolder.Instance.stressValue <= 0)
        {
            // TODO: 游戏结束当前梦境
            Utils.GameOver("压力释放完毕！！！");
        }
    }


    // 受伤后退效果
    private IEnumerator KnockbackEffect(Vector3 knockbackDirection)
    {
        // 计算后退的目标位置
        Vector3 targetPosition = transform.position + knockbackDirection.normalized * knockbackDistance;

        // 使用 DoTween 移动到目标位置
        transform.DOMove(targetPosition, knockbackDuration);

        yield return new WaitForSeconds(knockbackDuration);
    }


    private void SpawnSkills(string skillName)
    {
        //获取玩家位置和主摄像机
        Vector3 playerPosition = transform.position;
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            //  将视口中心转换为世界坐标
            Vector3 viewportCenter = new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane);
            Vector3 dropPositionViewport = mainCamera.ViewportToWorldPoint(viewportCenter);

            // 在镜头视野内生成随机偏移
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 5;
            Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y);

            // 计算最终掉落位置
            Vector3 dropPosition = dropPositionViewport + randomOffset;

            // 实例化技能掉落物品
            GameObject skillItemObj = Instantiate(skillPrefabs[skillName], dropPosition, Quaternion.identity);
            SkillItem skillItem = skillItemObj.GetComponent<SkillItem>();
        }
    }

}

