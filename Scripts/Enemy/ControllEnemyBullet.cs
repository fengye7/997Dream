using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllEnemyBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 10; // 子弹伤害值

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
        // 检测与玩家的碰撞
        if (collision.CompareTag("Player"))
        {
            _playerAttackSystem.TakeDamage(damage, _rigidbody.velocity.normalized);

            // "销毁"子弹——放回对象池
            gameObject.SetActive(false);
            PoolManager.ReturnToPool(_bulletPrefab, gameObject);
        }
    }
}
