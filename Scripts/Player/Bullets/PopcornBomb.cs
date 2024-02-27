using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornBomb : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 2; // 子弹伤害值

    public GameObject _explosion;
    public Rigidbody2D _rigidbody;

    public float explosionRadius = 2;

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

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        // 检测与怪物的碰撞
        if (collision.CompareTag("Enemy"))
        {
            // 播放爆炸效果
            GameObject bomb = Instantiate(_explosion, transform.position, Quaternion.identity);
            ParticleSystem ps = bomb.GetComponent<ParticleSystem>();
            ps.Play();

            Debug.Log("爆炸");

            // 获取所有在爆炸范围内的敌人
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                // 检查是否是敌人
                if (enemyCollider.CompareTag("Enemy"))
                {
                    // 获取怪物脚本并调用受伤方法
                    Enemy enemy = enemyCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage * _playerFSM._paramater._playerDamage);
                    }
                }
            }
            DreamSceneAudios.Instance.PlayBombAudio();
            // "销毁"子弹
            Destroy(gameObject);
            // 等待爆炸特效播放完毕
            yield return new WaitForSeconds(ps.main.duration);
            Destroy(bomb);
        }
    }
}
