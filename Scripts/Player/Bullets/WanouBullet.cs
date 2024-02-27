using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanouBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 2; // 子弹伤害值
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
        // 根据方向计算物体的旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 设置物体的旋转角度
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检测与敌人子弹的碰撞
        if (collision.CompareTag("EnemyBullet"))
        {
            ControllEnemyBullet enemyBullet = collision.GetComponent<ControllEnemyBullet>();
            if (enemyBullet != null)
            {
                // 销毁敌人子弹
                PoolManager.ReturnToPool(_bulletPrefab, enemyBullet.gameObject);
            }
        }
    }
}
