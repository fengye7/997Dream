using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllEnemyBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 10; // �ӵ��˺�ֵ

    public GameObject _explosion;
    public Rigidbody2D _rigidbody;
    public GameObject _bulletPrefab;

    private PlayerAttackSystem _playerAttackSystem;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAttackSystem = FindObjectOfType<PlayerAttackSystem>();
    }

    public void SetBulletSpeed(Vector2 direction)
    {
        _rigidbody.velocity = direction * _bulletSpeed;
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
        // �������ҵ���ײ
        if (collision.CompareTag("Player"))
        {
            _playerAttackSystem.TakeDamage(damage, _rigidbody.velocity.normalized);

            // "����"�ӵ������Żض����
            gameObject.SetActive(false);
            PoolManager.ReturnToPool(_bulletPrefab, gameObject);
        }
    }
}
