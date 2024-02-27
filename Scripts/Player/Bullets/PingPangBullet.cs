using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPangBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 2; // 子弹伤害值

    public Rigidbody2D _rigidbody;

    private PlayerFSM _playerFSM;

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
        // 检测与怪物的碰撞
        if (collision.CompareTag("Enemy"))
        {
            // 获取怪物脚本并调用受伤方法
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage * _playerFSM._paramater._playerDamage);
            }
            DreamSceneAudios.Instance.PlayHitAudio();
            // 实现乒乓球的反弹转向继续发射
            Vector2 normal = (transform.position - collision.transform.position).normalized;
            Vector2 newDirection = Vector2.Reflect(_rigidbody.velocity.normalized, normal);

            // 设置子弹速度和方向
            _rigidbody.velocity = newDirection * _bulletSpeed;
            // 更新子弹的旋转角度
            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }
}
