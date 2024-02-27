using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 2; // 子弹伤害值

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
            // 如果当前子弹不是来自对象池，则等待一定时间后销毁子弹
            StartCoroutine(DestroyBulletWithDelay());
        }
    }

    public void SetBulletSpeed(Vector2 direction)
    {
        _rigidbody.velocity = direction * _bulletSpeed;
        // 根据方向计算物体的旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 设置物体的旋转角度
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void SetBulletSpeed(Vector2 direction, float speed)
    {
        _rigidbody.velocity = direction * speed;
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
            if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.GetComponent<PoolManager>() != null)
            {
                // "销毁"子弹——放回对象池
                gameObject.SetActive(false);
                PoolManager.ReturnToPool(_bulletPrefab, gameObject);
            }
            else
            {
                // 如果当前子弹不是来自对象池，则等待一定时间后销毁子弹
                StartCoroutine(DestroyBulletWithDelay());
            }
        }
    }

    // 等待一定时间后销毁子弹
    private IEnumerator DestroyBulletWithDelay()
    {
        yield return new WaitForSeconds(1f); // 等待1秒
        Destroy(gameObject);
    }
}
