using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanouBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 2; // �ӵ��˺�ֵ
    public float rotationSpeed = 50;

    public Rigidbody2D _rigidbody;

    private PlayerFSM _playerFSM;

    public GameObject _bulletPrefab;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerFSM = FindObjectOfType<PlayerFSM>();
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
        // ���������ӵ�����ײ
        if (collision.CompareTag("EnemyBullet"))
        {
            ControllEnemyBullet enemyBullet = collision.GetComponent<ControllEnemyBullet>();
            if (enemyBullet != null)
            {
                // ���ٵ����ӵ�
                PoolManager.ReturnToPool(_bulletPrefab, enemyBullet.gameObject);
            }
        }
    }
}
