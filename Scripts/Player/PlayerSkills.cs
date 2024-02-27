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
    public Image cooldownBar; // ��ȴ������ǰ�� Image
    public float _rollCooldown = 3.0f; // Roll���ܵ���ȴʱ�䣬Ĭ��Ϊ3��
    public float _lastRollTime = -Mathf.Infinity; // ��һ��ʹ��Roll���ܵ�ʱ�䣬Ĭ��Ϊ�������
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
    public float luosifenRadius = 5f; // ��ɫԲȦ��Χ�뾶
    public float luosifenDuration = 3f; // ���ٳ���ʱ��
    public float luosifenSlowFactor = 0.5f; // �������ӣ����ٺ��ٶȵı�����
    public float luosifenCooldown = 2f;
    public float currentluosifenDownTime = 0;

    [Header("KaochuanSkill")]
    // ����Ԥ����
    public GameObject bulletPrefab;
    // �����
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
    public GameObject pingpangqiuPrefab; // ƹ����Ԥ����
    public Transform pingpangqiufirePoint; // �����
    public float pingpangqiuSpeed= 10f; // �����ٶ�
    public float pingpangqiuRemain = 1f; // ����ʱ��ʱ��

    [Header("TaoquanSkill")]
    public GameObject taoquanPrefab;
    public float taoquanCooldown = 5f; // ��Ȧ������ȴʱ��
    public float taoquanInterval = 1f;
    public int taoquanNum = 5;
    public float holdDuration = 1f; // ��ס���˳���ʱ��

    [Header("WanouSkill")]
    public GameObject wanouPrefab; // ��żԤ����
    public float distanceFromPlayer = 2f; // ��ż���ɫ֮��ľ���
    public float rotationSpeed = 50f; // ��żΧ�ƽ�ɫ����ת�ٶ�
    private GameObject[] wanous; // �洢��ż���������
    private int wanouCount = 6; // ��ż����
    public float wanouRemain = 3f;

    [Header("HushenfuSkill")]
    public float hushenfuDuration = 3;
    public float hushenfuRate = 0.1f;

    PlayerFSM fsm;
    PlayerAttackSystem attackSystem;
    WeaponKeyBoard weaponKeyBoard;

    void Start()
    {
        cooldownBar.fillAmount = 1f; // ��ʼ������Ϊ��ȫ����
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
        // ɾ�� verticalLayoutGroup �µ������Ӷ���
        foreach (Transform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
        // �����µļ���ͼ��
        foreach (SkillData skillData in DataHolder.Instance.skillList)
        {
            // ʵ��������ͼ��
            GameObject skillIcon = new GameObject("SkillIcon_" + skillData.skillName);
            skillIcon.transform.SetParent(verticalLayoutGroup.transform, false);

            // ��� Image ���
            Image iconImage = skillIcon.AddComponent<Image>();
            iconImage.sprite = skillData.skillAvatar; // ����ͼ��ͼƬ
            iconImage.rectTransform.sizeDelta = new Vector2(avatarWidth, avatarHeight); // ����ͼ��ߴ�

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
                    // ���������ּ� 1-9 ʱִ�еĲ���
                    Debug.Log("Key " + i + " is pressed." + "ʹ�ã�"+midskillData.skillName);
                    UseSkill(midskillData.skillName);
                    //ʹ�ø�skill��ɾ����Ӧ����
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

    // ��ʼ������ȴ����
    public void StartRollCooldown()
    {
        _currentRollDownTime = _rollCooldown;
    }

    // ���¼�����ȴʱ��ͳ���Ч��
    void UpdateCooldowns()
    {
        if (_currentRollDownTime > 0)
        {
            _currentRollDownTime -= Time.deltaTime;
            cooldownBar.fillAmount = _currentRollDownTime / _rollCooldown; // ���¶�������
        }
        if (currentDouzhiDownTime > 0)
        {
            currentDouzhiDownTime -= Time.deltaTime;
            douzhiCooldownBar.fillAmount = currentDouzhiDownTime / douzhiCooldown; // ���¶�������
        }
        if(currentluosifenDownTime > 0)
        {
            currentluosifenDownTime -= Time.deltaTime;
            luosifenCooldownBar.fillAmount = currentluosifenDownTime / luosifenCooldown;
        }
    }

    #region ��֭����
    // ʹ�ö�֭Ч��
    public void DouzhiSkill()
    {
        Debug.Log("ʹ�ü��ܡ�����֭");
        // �������Ч����50%���٣�50%��������
        if (Random.value < 0.5f)
        {
            // �����ƶ��ٶ�
            fsm._paramater._playerSpeed *= douzhiSpeedDownRate; // ����50%�ƶ��ٶ�
            SkillsFeedBack.Instance.ShowSpeedReductionFeedback();
            SkillsFeedBack.Instance.ShowFeedbackText("��֭�Ի������ˣ��ƶ��ٶ��½�50%");
            Debug.Log("�ƶ��ٶȼ��٣�" + fsm._paramater._playerSpeed);
            // ���ó���ʱ����ٶȻָ�
            StartCoroutine(RecoverSpeedAfterDuration(douzhiDuration, douzhiSpeedDownRate));
        }
        else
        {
            // ���ӹ�����
            fsm._paramater._playerDamage *= douzhiAttackUpRate;
            SkillsFeedBack.Instance.ShowDamageIncreaseFeedback();
            SkillsFeedBack.Instance.ShowFeedbackText("���ǳԶ�֭�Ĵ���ˮ�֣�����������50%");
            Debug.Log("���������ӣ�" + fsm._paramater._playerDamage);

            StartCoroutine(RecoverDamageAfterDuration());
        }
    }

    // �ָ��ƶ��ٶ�
    IEnumerator RecoverSpeedAfterDuration(float duration, float rate)
    {
        yield return new WaitForSeconds(duration);

        // �ָ��ƶ��ٶ�Ϊԭʼֵ
        fsm._paramater._playerSpeed /= rate;
        SkillsFeedBack.Instance.StopDouzhiFeedback();
        SkillsFeedBack.Instance.StopSpeedupFeedback();
    }

    // �ָ��ƶ��ٶ�
    IEnumerator RecoverDamageAfterDuration()
    {
        yield return new WaitForSeconds(douzhiDuration);

        // �ָ��ƶ��ٶ�Ϊԭʼֵ
        fsm._paramater._playerDamage /= douzhiAttackUpRate;
        SkillsFeedBack.Instance.StopDouzhiFeedback();
    }
    #endregion

    #region ���Ϸۼ���
    public void LuosifenSkill()
    {
        Debug.Log("ʹ�ü��ܡ������Ϸ�");
        SkillsFeedBack.Instance.ShowFeedbackText("���Ϸ������е��˼���");
        // ��ȡ��ɫԲȦ��Χ�ڵ����е���
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, luosifenRadius);

        // ������ٷ�Χ�����ó���ʱ��
        SkillsFeedBack.Instance.ShowLuosifenArea();

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                // ��ÿ������Ӧ�ü���Ч��
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

    #region ��������

    // ��������˷������������ӵ�
    public void KaochuanSkill()
    {
        Debug.Log("ʹ�ü��ܡ�������");
        SkillsFeedBack.Instance.ShowFeedbackText("�������ɴ�");
        StartCoroutine(FireKaochuanBullets());
    }

    IEnumerator FireKaochuanBullets()
    {
        // ��ȡ���е��˶���
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ����е��˴���
        if (enemies.Length > 0)
        {
            // ���ѡ��һ������
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            for (int i = 0; i < kaochuanNum; i++)
            {
                // �����ӵ�����Ϊ�����������
                Vector2 direction = randomEnemy.transform.position - firePoint.position;
                direction.Normalize();

                // ʵ�����ӵ�
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                if (bullet != null)
                {
                    // �����ӵ��ٶȺͷ���
                    bullet.GetComponent<KaochuanBullet>().SetBulletSpeed(direction);
                }

                // �ȴ�һС��ʱ���ٷ�����һ���ӵ�
                yield return new WaitForSeconds(kaochuanInterval); // �����ʵ����ӳ�ʱ��
            }
        }
    }
    #endregion

    #region ���׻�����
    // ���׻����ܣ���������˷��䱬ը�ӵ�
    public void PopcornSkill()
    {
        Debug.Log("ʹ�ü��ܡ������׻�");
        StartCoroutine(FireBombs());
    }

    IEnumerator FireBombs()
    {
        // ��ȡ���е��˶���
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ����е��˴���
        if (enemies.Length > 0)
        {
            // ���ѡ��һ������
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];

            // �����ӵ�����Ϊ�����������
            Vector2 direction = randomEnemy.transform.position - firePoint.position;
            direction.Normalize();

            // ʵ������ը�ӵ�
            GameObject bomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
            if (bomb != null)
            {
                // ���ñ�ը�ӵ���Ŀ����˺ͷ���
                bomb.GetComponent<PopcornBomb>().SetBulletSpeed(direction);
            }

            // �ȴ��ӵ��������
            yield return new WaitForSeconds(bombCooldown); // �����ʵ����ӳ�ʱ��
        }
    }

    #endregion

    #region �����Ƽ���
    public void XianrenzhangSkill()
    {
        Debug.Log("ʹ�������Ƽ���");
        // �����ӵ����䷽��ĳ�ʼ�Ƕ�
        float initialAngle = 0f;

        // �������ӵ���ÿ���ӵ����12��
        for (int i = 0; i < xianrenzhangNum; i++)
        {
            // ���㵱ǰ�ӵ��ĽǶ�
            float angle = initialAngle + i * (360f / xianrenzhangNum);

            // ���Ƕ�ת��Ϊ��������
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            // ʵ�����ӵ�
            GameObject bullet = Instantiate(xianrenzhangBulletPrefab, transform.position, Quaternion.identity);
            if (bullet != null)
            {
                // �����ӵ��ٶȺͷ���
                bullet.GetComponent<ControllBullet>().SetBulletSpeed(direction, xianrenzhangSpeed);
            }
        }
    }

    #endregion

    #region ���������Ϸ����
    public void GameSkill()
    {
        Debug.Log("ʹ�ù��������Ϸ����");
        SkillsFeedBack.Instance.ShowFeedbackText("������Ϸ�����ҵĹ���20%");
        weaponKeyBoard.fireRate *= fireIntervalDownBeishu;
        SkillsFeedBack.Instance.ShowAttackSpeedupFeedback();
        Debug.Log("�����ٶ�����" + fsm._paramater._playerSpeed);
        // ���ó���ʱ����ٶȻָ�
        StartCoroutine(RecoverAttackSpeedAfterDuration());
    }

    // �ָ������ٶ�
    IEnumerator RecoverAttackSpeedAfterDuration()
    {
        yield return new WaitForSeconds(gameskillDuration);
        // �ָ��ƶ��ٶ�Ϊԭʼֵ
        fsm._paramater._playerSpeed /= fireIntervalDownBeishu;
        SkillsFeedBack.Instance.StopAttackSpeedupFeedback();
    }
    #endregion

    #region ��Ʊ����
    public void CaipiaoSkill()
    {
        Debug.Log("ʹ�ü��ܡ�����Ʊ");
        // �������Ч����50%����ѹ��ֵ��50%����ѹ��ֵ
        if (Random.value < 0.5f)
        {
            // ����ѹ��ֵ
            isStreesUp = false;
            DataHolder.Instance.stressValue *= stressDownBeishu;
            Debug.Log("ѹ��ֵ���٣�" + DataHolder.Instance.stressValue);
            SkillsFeedBack.Instance.ShowFeedbackText("��Ʊ�н�����ѹ��ֵ�½�50%");
        }
        else
        {
            // ����ѹ��ֵ
            isStreesUp=true;
            DataHolder.Instance.stressValue *= stressUpBeishu;
            Debug.Log("ѹ��ֵ���ӣ�" + DataHolder.Instance.stressValue);
            SkillsFeedBack.Instance.ShowFeedbackText("��Ʊû�н���ѹ��ֵ����20%");
        }
    }
    #endregion

    #region ��Ь����
    public void PaoxieSkill()
    {
        Debug.Log("ʹ�ü��ܡ�����Ь");
        // �����ƶ��ٶ�
        fsm._paramater._playerSpeed *= paoxieSpeedBeishu;
        SkillsFeedBack.Instance.ShowSpeedupFeedback();
        Debug.Log("�ƶ��ٶ����ӣ�" + fsm._paramater._playerSpeed);
        SkillsFeedBack.Instance.ShowFeedbackText("�����ҵ�����Ь����������20%");
        // ���ó���ʱ����ٶȻָ�
        StartCoroutine(RecoverSpeedAfterDuration(paoxieDuration, paoxieSpeedBeishu));
    }
    #endregion

    #region ���������
    public void HushenfuSkill()
    {
        Debug.Log("ʹ�ü��ܡ��������");
        attackSystem.avoidRate += hushenfuRate;
        SkillsFeedBack.Instance.ShowAvoidRateupFeedback();
        Debug.Log("���������ӣ�" + attackSystem.avoidRate);
        SkillsFeedBack.Instance.ShowFeedbackText("��������������������10%");
        StartCoroutine(RecoverAvoidRateAfterDuration());
    }
    IEnumerator RecoverAvoidRateAfterDuration()
    {
        yield return new WaitForSeconds(hushenfuDuration);
        attackSystem.avoidRate -= hushenfuRate;
        SkillsFeedBack.Instance.StopAvoidRateupFeedback();
    }
    #endregion

    #region ƹ������
    public void PingpangqiuSkill()
    {
        Debug.Log("ʹ�ü��ܡ���ƹ����");
        StartCoroutine(FirePingpangqiu());
    }

    IEnumerator FirePingpangqiu() {
        // ��ȡ���е��˶���
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ����е��˴���
        if (enemies.Length > 0)
        {
            // ���ѡ��һ������
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];

            // �����ӵ�����Ϊ�����������
            Vector2 direction = randomEnemy.transform.position - pingpangqiufirePoint.position;
            direction.Normalize();

            // ʵ����ƹ����
            GameObject pingpang = Instantiate(pingpangqiuPrefab, pingpangqiufirePoint.position, Quaternion.identity);
            if (pingpang != null)
            {
                pingpang.GetComponent<PingPangBullet>().SetBulletSpeed(direction);
            }

            // �ȴ��ӵ��������(���ܷ�����Ƿ񷴵������˼��ζ�����)
            yield return new WaitForSeconds(pingpangqiuRemain); // �����ʵ����ӳ�ʱ��
            Destroy(pingpang);
        }
    }
    #endregion

    #region ��Ȧ����
    public void TaoquanSkill()
    {
        Debug.Log("ʹ�ü��ܡ�����Ȧ");
        SkillsFeedBack.Instance.ShowFeedbackText("�����ұ�����ס������");
        StartCoroutine(FireTaochuanBullet());
    }

    IEnumerator FireTaochuanBullet()
    {
        // ��ȡ���е��˶���
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ����е��˴���
        if (enemies.Length > 0)
        {
            for (int i = 0; i < taoquanNum; i++)
            {
                // ���ѡ��һ������
                GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
                // �����ӵ�����Ϊ�����������
                Vector2 direction = randomEnemy.transform.position - transform.position;
                direction.Normalize();

                // ʵ�����ӵ�
                GameObject bullet = Instantiate(taoquanPrefab, transform.position, Quaternion.identity);
                if (bullet != null)
                {
                    // �����ӵ��ٶȺͷ���
                    bullet.GetComponent<TaoquanBullet>().SetBulletSpeed(direction);
                }

                // �ȴ�һС��ʱ���ٷ�����һ���ӵ�
                yield return new WaitForSeconds(taoquanInterval); // �����ʵ����ӳ�ʱ��
            }
        }
    }
    #endregion

    #region ��ż����
    public void WanouSkill()
    {
        Debug.Log("ʹ�ü��ܡ�����ż");
        StartCoroutine(FireWanous());
    }
    IEnumerator FireWanous()
    {
        CreateWanous();
        // �ȴ�һС��ʱ��
        yield return new WaitForSeconds(wanouRemain); // �����ʵ����ӳ�ʱ��

        // ������ż��ֹͣ��ת
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
        // �����ż����
        wanous = new GameObject[0];
    }
    private void CreateWanous()
    {
        // ������ż���󲢳�ʼ��λ��
        if(wanous != null)
        {
            DestroyWanous();
        }
        wanous = new GameObject[wanouCount];
        for (int i = 0; i < wanouCount; i++)
        {
            float angle = i * (360f / wanouCount); // ������ż�ĳ�ʼ�Ƕ�
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Vector3 position = transform.position + rotation * Vector3.right * distanceFromPlayer; // ��ʼλ��
            wanous[i] = Instantiate(wanouPrefab, position, rotation);
        }
    }
    private void RotateWanous()
    {
        // �����ż���鲻Ϊ�գ�������ת
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
