using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public static class SkillNames
{
    public const string Douzhi = "Douzhi";
    public const string Luosifen = "Luosifen";
    public const string Kaochuan = "Kaochuan";
    public const string Popcorn = "Popcorn";

    public const string Pingpangqiu = "Pingpangqiu";
    public const string Taoquan = "Taoquan";
    public const string Wanou = "Wanou";

    public const string Xianrenzhang = "Xianrenzhang";
    public const string Game = "Game";
    public const string Caipiao = "Caipiao";
    public const string Paoxie = "Paoxie";
    public const string Hushenfu = "Hushenfu";
}

public class PlayerSkills: MonoBehaviour 
{
    [Header("UIInfo")]
    public VerticalLayoutGroup verticalLayoutGroup;

    [Header("SkillAvatarInfo")]
    public float avatarWidth = 80;
    public float avatarHeight = 40;

    [Header("RollSkill")]
    public Image cooldownBar; // 冷却读条的前景 Image
    public float _rollCooldown = 3.0f; // Roll技能的冷却时间，默认为3秒
    public float _lastRollTime = -Mathf.Infinity; // 上一次使用Roll技能的时间，默认为负无穷大
    public float _currentRollDownTime = 0;

    [Header("DouzhiSkill")]
    public Image douzhiCooldownBar;
    public float douzhiCooldown = 3f;
    public float lastdouzhiTime = -Mathf.Infinity;
    public float douzhiDuration = 3;
    public float currentDouzhiDownTime =0;
    public float douzhiSpeedDownRate = 0.5f;
    public float douzhiAttackUpRate = 1.2f;

    [Header("LuosifenSkill")]
    public Image luosifenCooldownBar;
    public float luosifenRadius = 5f; // 角色圆圈范围半径
    public float luosifenDuration = 3f; // 减速持续时间
    public float luosifenSlowFactor = 0.5f; // 减速因子（减速后速度的比例）
    public float luosifenCooldown = 2f;
    public float currentluosifenDownTime = 0;

    [Header("KaochuanSkill")]
    // 弹道预制体
    public GameObject bulletPrefab;
    // 发射点
    public Transform firePoint;
    public float kaochuanInterval = 0.1f;
    public int kaochuanNum = 3;

    [Header("PopcornSkill")]
    public GameObject bombPrefab;
    public float bombCooldown;

    [Header("XianrenzhangSkill")]
    public GameObject xianrenzhangBulletPrefab;
    public int xianrenzhangNum = 30;
    public float xianrenzhangSpeed = 12;

    [Header("GameSkill")]
    public float gameskillDuration = 3f;
    public float fireIntervalDownBeishu = 0.8f;

    [Header("CaipiaoSkill")]
    public float stressUpBeishu = 1.2f;
    public float stressDownBeishu =0.5f;
    private bool isStreesUp;

    [Header("PaoxieSkill")]
    public float paoxieDuration =1f;
    public float paoxieSpeedBeishu = 1.2f;

    [Header("PingpangqiuSkill")]
    public GameObject pingpangqiuPrefab; // 乒乓球预制体
    public Transform pingpangqiufirePoint; // 发射点
    public float pingpangqiuSpeed= 10f; // 发射速度
    public float pingpangqiuRemain = 1f; // 持续时间时间

    [Header("TaoquanSkill")]
    public GameObject taoquanPrefab;
    public float taoquanCooldown = 5f; // 套圈技能冷却时间
    public float taoquanInterval = 1f;
    public int taoquanNum = 5;
    public float holdDuration = 1f; // 定住敌人持续时间

    [Header("WanouSkill")]
    public GameObject wanouPrefab; // 玩偶预制体
    public float distanceFromPlayer = 2f; // 玩偶与角色之间的距离
    public float rotationSpeed = 50f; // 玩偶围绕角色的旋转速度
    private GameObject[] wanous; // 存储玩偶对象的数组
    private int wanouCount = 6; // 玩偶数量
    public float wanouRemain = 3f;

    [Header("HushenfuSkill")]
    public float hushenfuDuration = 3;
    public float hushenfuRate = 0.1f;

    PlayerFSM fsm;
    PlayerAttackSystem attackSystem;
    WeaponKeyBoard weaponKeyBoard;

    void Start()
    {
        cooldownBar.fillAmount = 1f; // 初始化读条为完全填满
        fsm = GetComponent<PlayerFSM>();
        attackSystem = GetComponent<PlayerAttackSystem>();
        weaponKeyBoard = FindObjectOfType<WeaponKeyBoard>();

        UpdateSkillUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldowns();
        UseSkill();
        //TestSkill();
    }

    void TestSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //StartCoroutine(TestSkills());
            KaochuanSkill();
        }
        RotateWanous();
    }

    IEnumerator TestSkills()
    {
        DouzhiSkill();
        yield return new WaitForSeconds(3);
        LuosifenSkill();
        yield return new WaitForSeconds(3);
        KaochuanSkill();
        yield return new WaitForSeconds(3);
        PopcornSkill();
        yield return new WaitForSeconds(3);
        PaoxieSkill();
        yield return new WaitForSeconds(3);
        HushenfuSkill();
        yield return new WaitForSeconds(3);
        XianrenzhangSkill();
        yield return new WaitForSeconds(3);
        GameSkill();
        yield return new WaitForSeconds(3);
        CaipiaoSkill();
        yield return new WaitForSeconds(3);
        WanouSkill();
        yield return new WaitForSeconds(3);
        TaoquanSkill();
        yield return new WaitForSeconds(3);
        PingpangqiuSkill();
    }

    public void UpdateSkillUI()
    {
        // 删除 verticalLayoutGroup 下的所有子对象
        foreach (Transform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
        // 创建新的技能图标
        foreach (SkillData skillData in DataHolder.Instance.skillList)
        {
            // 实例化技能图标
            GameObject skillIcon = new GameObject("SkillIcon_" + skillData.skillName);
            skillIcon.transform.SetParent(verticalLayoutGroup.transform, false);

            // 添加 Image 组件
            Image iconImage = skillIcon.AddComponent<Image>();
            iconImage.sprite = skillData.skillAvatar; // 设置图标图片
            iconImage.rectTransform.sizeDelta = new Vector2(avatarWidth, avatarHeight); // 设置图标尺寸

        }
    }

    public void UseSkill()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (DataHolder.Instance.skillList.Count >= i) {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    SkillData midskillData = DataHolder.Instance.GetSkillAtPosition(i - 1);
                    // 当按下数字键 1-9 时执行的操作
                    Debug.Log("Key " + i + " is pressed." + "使用："+midskillData.skillName);
                    UseSkill(midskillData.skillName);
                    //使用该skill后删除对应道具
                    DataHolder.Instance.skillList.Remove(midskillData);
                    UpdateSkillUI();
                } 
            }
        }
    }

    public void UseSkill(string skillName)
    {
        switch (skillName)
        {
            case SkillNames.Douzhi:
                DouzhiSkill();
                break;
            case SkillNames.Luosifen:
                LuosifenSkill();
                break;
            case SkillNames.Kaochuan:
                KaochuanSkill();
                break;
            case SkillNames.Popcorn:
                PopcornSkill();
                break;

            case SkillNames.Pingpangqiu:
                PingpangqiuSkill();
                break;
            case SkillNames.Taoquan: 
                TaoquanSkill();
                break;
            case SkillNames.Wanou:
                WanouSkill();
                break;

            case SkillNames.Xianrenzhang:
                XianrenzhangSkill();
                break;
            case SkillNames.Game:
                GameSkill();
                break;
            case SkillNames.Caipiao:
                CaipiaoSkill();
                break;
            case SkillNames.Paoxie:
                PaoxieSkill();
                break;
            case SkillNames.Hushenfu:
                HushenfuSkill();
                break;
            default:
                break;
        }
    }

    // 开始翻滚冷却读条
    public void StartRollCooldown()
    {
        _currentRollDownTime = _rollCooldown;
    }

    // 更新技能冷却时间和持续效果
    void UpdateCooldowns()
    {
        if (_currentRollDownTime > 0)
        {
            _currentRollDownTime -= Time.deltaTime;
            cooldownBar.fillAmount = _currentRollDownTime / _rollCooldown; // 更新读条进度
        }
        if (currentDouzhiDownTime > 0)
        {
            currentDouzhiDownTime -= Time.deltaTime;
            douzhiCooldownBar.fillAmount = currentDouzhiDownTime / douzhiCooldown; // 更新读条进度
        }
        if(currentluosifenDownTime > 0)
        {
            currentluosifenDownTime -= Time.deltaTime;
            luosifenCooldownBar.fillAmount = currentluosifenDownTime / luosifenCooldown;
        }
    }

    #region 豆汁技能
    // 使用豆汁效果
    public void DouzhiSkill()
    {
        Debug.Log("使用技能——豆汁");
        // 随机决定效果：50%减速，50%增攻击力
        if (Random.value < 0.5f)
        {
            // 减少移动速度
            fsm._paramater._playerSpeed *= douzhiSpeedDownRate; // 减少50%移动速度
            SkillsFeedBack.Instance.ShowSpeedReductionFeedback();
            SkillsFeedBack.Instance.ShowFeedbackText("豆汁吃坏肚子了，移动速度下降50%");
            Debug.Log("移动速度减少：" + fsm._paramater._playerSpeed);
            // 设置持续时间后速度恢复
            StartCoroutine(RecoverSpeedAfterDuration(douzhiDuration, douzhiSpeedDownRate));
        }
        else
        {
            // 增加攻击力
            fsm._paramater._playerDamage *= douzhiAttackUpRate;
            SkillsFeedBack.Instance.ShowDamageIncreaseFeedback();
            SkillsFeedBack.Instance.ShowFeedbackText("我是吃豆汁的大力水手，攻击力上升50%");
            Debug.Log("攻击力增加：" + fsm._paramater._playerDamage);

            StartCoroutine(RecoverDamageAfterDuration());
        }
    }

    // 恢复移动速度
    IEnumerator RecoverSpeedAfterDuration(float duration, float rate)
    {
        yield return new WaitForSeconds(duration);

        // 恢复移动速度为原始值
        fsm._paramater._playerSpeed /= rate;
        SkillsFeedBack.Instance.StopDouzhiFeedback();
        SkillsFeedBack.Instance.StopSpeedupFeedback();
    }

    // 恢复移动速度
    IEnumerator RecoverDamageAfterDuration()
    {
        yield return new WaitForSeconds(douzhiDuration);

        // 恢复移动速度为原始值
        fsm._paramater._playerDamage /= douzhiAttackUpRate;
        SkillsFeedBack.Instance.StopDouzhiFeedback();
    }
    #endregion

    #region 螺蛳粉技能
    public void LuosifenSkill()
    {
        Debug.Log("使用技能——螺蛳粉");
        SkillsFeedBack.Instance.ShowFeedbackText("螺蛳粉立场中敌人减速");
        // 获取角色圆圈范围内的所有敌人
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, luosifenRadius);

        // 激活减速范围并设置持续时间
        SkillsFeedBack.Instance.ShowLuosifenArea();

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                // 对每个敌人应用减速效果
                Enemy enemy = col.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ApplySlowEffect(luosifenDuration, luosifenSlowFactor);
                }
            }
        }
        StartCoroutine(ClearSlowArea());
    }

    IEnumerator ClearSlowArea()
    {
        yield return new WaitForSeconds(luosifenDuration);

        SkillsFeedBack.Instance.CloseLuosifenArea();
    }

    #endregion

    #region 烤串技能

    // 朝随机敌人发射三个穿刺子弹
    public void KaochuanSkill()
    {
        Debug.Log("使用技能——烤串");
        SkillsFeedBack.Instance.ShowFeedbackText("烤串穿成串");
        StartCoroutine(FireKaochuanBullets());
    }

    IEnumerator FireKaochuanBullets()
    {
        // 获取所有敌人对象
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 如果有敌人存在
        if (enemies.Length > 0)
        {
            // 随机选择一个敌人
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            for (int i = 0; i < kaochuanNum; i++)
            {
                // 计算子弹方向为朝向随机敌人
                Vector2 direction = randomEnemy.transform.position - firePoint.position;
                direction.Normalize();

                // 实例化子弹
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                if (bullet != null)
                {
                    // 设置子弹速度和方向
                    bullet.GetComponent<KaochuanBullet>().SetBulletSpeed(direction);
                }

                // 等待一小段时间再发射下一颗子弹
                yield return new WaitForSeconds(kaochuanInterval); // 设置适当的延迟时间
            }
        }
    }
    #endregion

    #region 爆米花技能
    // 爆米花技能：朝随机敌人发射爆炸子弹
    public void PopcornSkill()
    {
        Debug.Log("使用技能——爆米花");
        StartCoroutine(FireBombs());
    }

    IEnumerator FireBombs()
    {
        // 获取所有敌人对象
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 如果有敌人存在
        if (enemies.Length > 0)
        {
            // 随机选择一个敌人
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];

            // 计算子弹方向为朝向随机敌人
            Vector2 direction = randomEnemy.transform.position - firePoint.position;
            direction.Normalize();

            // 实例化爆炸子弹
            GameObject bomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
            if (bomb != null)
            {
                // 设置爆炸子弹的目标敌人和方向
                bomb.GetComponent<PopcornBomb>().SetBulletSpeed(direction);
            }

            // 等待子弹发射完成
            yield return new WaitForSeconds(bombCooldown); // 设置适当的延迟时间
        }
    }

    #endregion

    #region 仙人掌技能
    public void XianrenzhangSkill()
    {
        Debug.Log("使用仙人掌技能");
        // 计算子弹发射方向的初始角度
        float initialAngle = 0f;

        // 发射多个子弹，每个子弹间隔12度
        for (int i = 0; i < xianrenzhangNum; i++)
        {
            // 计算当前子弹的角度
            float angle = initialAngle + i * (360f / xianrenzhangNum);

            // 将角度转换为方向向量
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            // 实例化子弹
            GameObject bullet = Instantiate(xianrenzhangBulletPrefab, transform.position, Quaternion.identity);
            if (bullet != null)
            {
                // 设置子弹速度和方向
                bullet.GetComponent<ControllBullet>().SetBulletSpeed(direction, xianrenzhangSpeed);
            }
        }
    }

    #endregion

    #region 购买电子游戏技能
    public void GameSkill()
    {
        Debug.Log("使用购买电子游戏技能");
        SkillsFeedBack.Instance.ShowFeedbackText("电子游戏提升我的攻速20%");
        weaponKeyBoard.fireRate *= fireIntervalDownBeishu;
        SkillsFeedBack.Instance.ShowAttackSpeedupFeedback();
        Debug.Log("攻击速度增大：" + fsm._paramater._playerSpeed);
        // 设置持续时间后速度恢复
        StartCoroutine(RecoverAttackSpeedAfterDuration());
    }

    // 恢复攻击速度
    IEnumerator RecoverAttackSpeedAfterDuration()
    {
        yield return new WaitForSeconds(gameskillDuration);
        // 恢复移动速度为原始值
        fsm._paramater._playerSpeed /= fireIntervalDownBeishu;
        SkillsFeedBack.Instance.StopAttackSpeedupFeedback();
    }
    #endregion

    #region 彩票技能
    public void CaipiaoSkill()
    {
        Debug.Log("使用技能——彩票");
        // 随机决定效果：50%减少压力值，50%增加压力值
        if (Random.value < 0.5f)
        {
            // 减少压力值
            isStreesUp = false;
            DataHolder.Instance.stressValue *= stressDownBeishu;
            Debug.Log("压力值减少：" + DataHolder.Instance.stressValue);
            SkillsFeedBack.Instance.ShowFeedbackText("彩票中奖啦，压力值下降50%");
        }
        else
        {
            // 增加压力值
            isStreesUp=true;
            DataHolder.Instance.stressValue *= stressUpBeishu;
            Debug.Log("压力值增加：" + DataHolder.Instance.stressValue);
            SkillsFeedBack.Instance.ShowFeedbackText("彩票没中奖，压力值上升20%");
        }
    }
    #endregion

    #region 跑鞋技能
    public void PaoxieSkill()
    {
        Debug.Log("使用技能——跑鞋");
        // 减少移动速度
        fsm._paramater._playerSpeed *= paoxieSpeedBeishu;
        SkillsFeedBack.Instance.ShowSpeedupFeedback();
        Debug.Log("移动速度增加：" + fsm._paramater._playerSpeed);
        SkillsFeedBack.Instance.ShowFeedbackText("穿上我的新跑鞋，移速上升20%");
        // 设置持续时间后速度恢复
        StartCoroutine(RecoverSpeedAfterDuration(paoxieDuration, paoxieSpeedBeishu));
    }
    #endregion

    #region 护身符技能
    public void HushenfuSkill()
    {
        Debug.Log("使用技能——护身符");
        attackSystem.avoidRate += hushenfuRate;
        SkillsFeedBack.Instance.ShowAvoidRateupFeedback();
        Debug.Log("闪避率增加：" + attackSystem.avoidRate);
        SkillsFeedBack.Instance.ShowFeedbackText("身带护身符，闪避率上升10%");
        StartCoroutine(RecoverAvoidRateAfterDuration());
    }
    IEnumerator RecoverAvoidRateAfterDuration()
    {
        yield return new WaitForSeconds(hushenfuDuration);
        attackSystem.avoidRate -= hushenfuRate;
        SkillsFeedBack.Instance.StopAvoidRateupFeedback();
    }
    #endregion

    #region 乒乓球技能
    public void PingpangqiuSkill()
    {
        Debug.Log("使用技能——乒乓球");
        StartCoroutine(FirePingpangqiu());
    }

    IEnumerator FirePingpangqiu() {
        // 获取所有敌人对象
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 如果有敌人存在
        if (enemies.Length > 0)
        {
            // 随机选择一个敌人
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];

            // 计算子弹方向为朝向随机敌人
            Vector2 direction = randomEnemy.transform.position - pingpangqiufirePoint.position;
            direction.Normalize();

            // 实例化乒乓球
            GameObject pingpang = Instantiate(pingpangqiuPrefab, pingpangqiufirePoint.position, Quaternion.identity);
            if (pingpang != null)
            {
                pingpang.GetComponent<PingPangBullet>().SetBulletSpeed(direction);
            }

            // 等待子弹发射完成(不管发射后是否反弹反弹了几次都销毁)
            yield return new WaitForSeconds(pingpangqiuRemain); // 设置适当的延迟时间
            Destroy(pingpang);
        }
    }
    #endregion

    #region 套圈技能
    public void TaoquanSkill()
    {
        Debug.Log("使用技能——套圈");
        SkillsFeedBack.Instance.ShowFeedbackText("都给我别动我套住你啦！");
        StartCoroutine(FireTaochuanBullet());
    }

    IEnumerator FireTaochuanBullet()
    {
        // 获取所有敌人对象
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 如果有敌人存在
        if (enemies.Length > 0)
        {
            for (int i = 0; i < taoquanNum; i++)
            {
                // 随机选择一个敌人
                GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
                // 计算子弹方向为朝向随机敌人
                Vector2 direction = randomEnemy.transform.position - transform.position;
                direction.Normalize();

                // 实例化子弹
                GameObject bullet = Instantiate(taoquanPrefab, transform.position, Quaternion.identity);
                if (bullet != null)
                {
                    // 设置子弹速度和方向
                    bullet.GetComponent<TaoquanBullet>().SetBulletSpeed(direction);
                }

                // 等待一小段时间再发射下一颗子弹
                yield return new WaitForSeconds(taoquanInterval); // 设置适当的延迟时间
            }
        }
    }
    #endregion

    #region 玩偶技能
    public void WanouSkill()
    {
        Debug.Log("使用技能——玩偶");
        StartCoroutine(FireWanous());
    }
    IEnumerator FireWanous()
    {
        CreateWanous();
        // 等待一小段时间
        yield return new WaitForSeconds(wanouRemain); // 设置适当的延迟时间

        // 销毁玩偶并停止旋转
        DestroyWanous();
    }

    void DestroyWanous()
    {
        foreach (var wanou in wanous)
        {
            if (wanou != null)
            {
                Destroy(wanou);
            }
        }
        // 清空玩偶数组
        wanous = new GameObject[0];
    }
    private void CreateWanous()
    {
        // 创建玩偶对象并初始化位置
        if(wanous != null)
        {
            DestroyWanous();
        }
        wanous = new GameObject[wanouCount];
        for (int i = 0; i < wanouCount; i++)
        {
            float angle = i * (360f / wanouCount); // 计算玩偶的初始角度
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Vector3 position = transform.position + rotation * Vector3.right * distanceFromPlayer; // 初始位置
            wanous[i] = Instantiate(wanouPrefab, position, rotation);
        }
    }
    private void RotateWanous()
    {
        // 如果玩偶数组不为空，继续旋转
        if (wanous != null && wanous.Length > 0)
        {
            for (int i = 0; i < wanouCount; i++)
            {
                float angle = Time.time * rotationSpeed + i * (360f / wanouCount);
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                Vector3 position = transform.position + rotation * Vector3.right * distanceFromPlayer;
                wanous[i].transform.position = position;
            }
        }
    }
    #endregion
}
