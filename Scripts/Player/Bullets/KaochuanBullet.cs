using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaochuanBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 3; // 子弹伤害值

    public GameObject _explosion;
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
        }
    }
}
