using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaoquanBullet : MonoBehaviour
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
            Debug.Log("定住敌人");
            // 获取怪物脚本并调用受伤方法
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage * _playerFSM._paramater._playerDamage);
            }
            DreamSceneAudios.Instance.PlayHitAudio();
            // 实现定住敌人一秒钟的功能
            Destroy(gameObject);
            StartCoroutine(HoldEnemyForSeconds(enemy, 1f));
        }
    }

    IEnumerator HoldEnemyForSeconds(Enemy enemy, float seconds)
    {
        // 保存敌人原本的移动速度
        float originalSpeed = enemy.moveSpeed;

        // 定住敌人
        enemy.moveSpeed = 0f;

        // 等待一定时间
        yield return new WaitForSeconds(seconds);

        // 恢复敌人的移动速度
        enemy.moveSpeed = originalSpeed;
    }
}
