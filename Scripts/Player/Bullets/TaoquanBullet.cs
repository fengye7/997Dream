using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaoquanBullet : MonoBehaviour
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
            Debug.Log("��ס����");
            // ��ȡ����ű����������˷���
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage * _playerFSM._paramater._playerDamage);
            }
            DreamSceneAudios.Instance.PlayHitAudio();
            // ʵ�ֶ�ס����һ���ӵĹ���
            Destroy(gameObject);
            StartCoroutine(HoldEnemyForSeconds(enemy, 1f));
        }
    }

    IEnumerator HoldEnemyForSeconds(Enemy enemy, float seconds)
    {
        // �������ԭ�����ƶ��ٶ�
        float originalSpeed = enemy.moveSpeed;

        // ��ס����
        enemy.moveSpeed = 0f;

        // �ȴ�һ��ʱ��
        yield return new WaitForSeconds(seconds);

        // �ָ����˵��ƶ��ٶ�
        enemy.moveSpeed = originalSpeed;
    }
}
