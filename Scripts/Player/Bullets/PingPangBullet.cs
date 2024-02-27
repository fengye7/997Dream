using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPangBullet : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 2; // �ӵ��˺�ֵ

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
            // ʵ��ƹ����ķ���ת���������
            Vector2 normal = (transform.position - collision.transform.position).normalized;
            Vector2 newDirection = Vector2.Reflect(_rigidbody.velocity.normalized, normal);

            // �����ӵ��ٶȺͷ���
            _rigidbody.velocity = newDirection * _bulletSpeed;
            // �����ӵ�����ת�Ƕ�
            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }
}
