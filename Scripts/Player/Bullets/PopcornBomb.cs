using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornBomb : MonoBehaviour
{
    public float _bulletSpeed = 10;

    public int damage = 2; // �ӵ��˺�ֵ

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
        // ���ݷ�������������ת�Ƕ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // �����������ת�Ƕ�
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void Update()
    {

    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        // �����������ײ
        if (collision.CompareTag("Enemy"))
        {
            // ���ű�ըЧ��
            GameObject bomb = Instantiate(_explosion, transform.position, Quaternion.identity);
            ParticleSystem ps = bomb.GetComponent<ParticleSystem>();
            ps.Play();

            Debug.Log("��ը");

            // ��ȡ�����ڱ�ը��Χ�ڵĵ���
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                // ����Ƿ��ǵ���
                if (enemyCollider.CompareTag("Enemy"))
                {
                    // ��ȡ����ű����������˷���
                    Enemy enemy = enemyCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage * _playerFSM._paramater._playerDamage);
                    }
                }
            }
            DreamSceneAudios.Instance.PlayBombAudio();
            // "����"�ӵ�
            Destroy(gameObject);
            // �ȴ���ը��Ч�������
            yield return new WaitForSeconds(ps.main.duration);
            Destroy(bomb);
        }
    }
}
