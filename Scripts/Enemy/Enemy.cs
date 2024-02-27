using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Enemy : MonoBehaviour
{
    public Transform playerTransform; // ��ҵ� Transform
    public float moveSpeed = 3f; // �ƶ��ٶ�
    public float attackDistance = 5f; // ��������
    public float shootingInterval = 1f; // ������
    public GameObject bulletPrefab; // �ӵ�Ԥ����
    public float bulletSpeed = 5f; // �ӵ��ٶ�
    public float edgeDistance = 3f; // ��Ե����
    public int experienceValue = 20; // ���ܵ�������õľ���ֵ
    public int stressValue = 10; // ���ܵ����ͷŵ�ѹ��ֵ

    private bool isShooting = false;

    public float maxHealth = 100; // �����������ֵ
    private float currentHealth; // ��ǰ����ֵ

    public float minScale = 0.9f; // ���ŵ���Сֵ
    public float scaleDuration = 0.01f; // ���Ŷ�������ʱ��
    public float dieScaleDuration = 0.5f;
    public float destroyDelay = 0.6f; // �ӳ�����ʱ��
    private float localScale;

    private Coroutine scaleCoroutine; // ����Э��

    private float _lastShootTime = 0;

    private PlayerAttackSystem _playerAttackSystem;

    private void Start()
    {
        currentHealth = maxHealth; // ��ʼ����ǰ����ֵΪ�������ֵ
        _playerAttackSystem = FindObjectOfType<PlayerAttackSystem>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // ������Ҷ���
        StartCoroutine(EnemyRoutine());

        localScale = transform.localScale.x;

        //���ӵ�������
        DataHolder.Instance.enemyCount++;
    }

    private IEnumerator EnemyRoutine()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= attackDistance)
            {
                // ��ҽ��빥����Χ�����ֱ�Ե���벢���
                isShooting = true;
            }
            else
            {
                // ������빥����Χ��׷�����
                isShooting = false;
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position - direction * edgeDistance, moveSpeed * Time.deltaTime);
            }

            if (isShooting)
            {
                // ������
                ShootPlayer();
            }

            yield return null;
        }
    }

    private void ShootPlayer()
    {
        // ������
        if (Time.time - _lastShootTime> shootingInterval)
        {
            _lastShootTime = Time.time;
            Debug.Log("�ӵ�Ϊ��" + bulletPrefab.name);
            // �����ӵ�����������
            GameObject bullet = PoolManager.Release(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }
    }

    // �����˺�
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // ��������ֵ

        // �������ֵС�ڵ���0����������
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // ���������Ȼ����ִ������Ч��
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine); // ֹ֮ͣǰ������Э��
                transform.localScale =new Vector3(localScale, localScale, localScale);
            }
            scaleCoroutine = StartCoroutine(Utils.ScaleEffect(transform, minScale,scaleDuration)); // ��ʼ�µ�����Ч��
        }
    }

    // ��������
    private void Die()
    {
        // ������ʵ�ֹ����������߼������粥������������������Ч��������ҵ÷ֵ�

        // �ӳ����ٹ������
        // ʹ�� DoTween ���ŵ�Ŀ��ֵ����������ɺ�Ļص�
        transform.DOScale(0, dieScaleDuration);
        _playerAttackSystem.GainExperience(experienceValue);
        _playerAttackSystem.ReleaseStress(stressValue);
        StartCoroutine(DestroyAfterDelay());
    }


    private void OnDestroy()
    {
        PlayerAttackSystem player = FindObjectOfType<PlayerAttackSystem>();
        if (player != null)
        {
            player.GainExperience(experienceValue);
        }
    }

    // �ӳ�����
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
        //���ٵ�������
        DataHolder.Instance.enemyCount--;
    }

    public void ApplySlowEffect(float duration, float slowFactor)
    {
        StartCoroutine(SlowForDuration(duration, slowFactor));
    }

    IEnumerator SlowForDuration(float duration, float slowFactor)
    {
        // ����ԭʼ�ٶ�
        float originalSpeed = moveSpeed;

        // �����ٶ�
        moveSpeed *= slowFactor;

        // �ȴ�һ��ʱ��
        yield return new WaitForSeconds(duration);

        // �ָ�ԭʼ�ٶ�
        moveSpeed = originalSpeed;
    }
}