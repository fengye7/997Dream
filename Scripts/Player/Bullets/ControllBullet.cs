using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 2; // �ӵ��˺�ֵ

    public GameObject _explosion;
    public Rigidbody2D _rigidbody;
    public GameObject _bulletPrefab;

    private PlayerFSM _playerFSM;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerFSM = FindObjectOfType<PlayerFSM>();
    }

    private void Start()
    {
        if (!(transform.parent != null && transform.parent.parent != null && transform.parent.parent.GetComponent<PoolManager>() != null))
        {
            // �����ǰ�ӵ��������Զ���أ���ȴ�һ��ʱ��������ӵ�
            StartCoroutine(DestroyBulletWithDelay());
        }
    }

    public void SetBulletSpeed(Vector2 direction)
    {
        _rigidbody.velocity = direction * _bulletSpeed;
        // ���ݷ�������������ת�Ƕ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // �����������ת�Ƕ�
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void SetBulletSpeed(Vector2 direction, float speed)
    {
        _rigidbody.velocity = direction * speed;
        // ���ݷ�������������ת�Ƕ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // �����������ת�Ƕ�
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����������ײ
        if (collision.CompareTag("Enemy"))
        {
            // ��ȡ����ű����������˷���
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage * _playerFSM._paramater._playerDamage);
            }
            DreamSceneAudios.Instance.PlayHitAudio();
            if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.GetComponent<PoolManager>() != null)
            {
                // "����"�ӵ������Żض����
                gameObject.SetActive(false);
                PoolManager.ReturnToPool(_bulletPrefab, gameObject);
            }
            else
            {
                // �����ǰ�ӵ��������Զ���أ���ȴ�һ��ʱ��������ӵ�
                StartCoroutine(DestroyBulletWithDelay());
            }
        }
    }

    // �ȴ�һ��ʱ��������ӵ�
    private IEnumerator DestroyBulletWithDelay()
    {
        yield return new WaitForSeconds(1f); // �ȴ�1��
        Destroy(gameObject);
    }
}
