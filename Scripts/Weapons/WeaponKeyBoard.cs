using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponKeyBoard : MonoBehaviour
{
    public Transform _aimTransform;
    public Transform _firePos;
    public GameObject _bulletPrefab;

    [Header("BulletInfo")]
    public float fireDuration = 0.5f; // ��������ʱ��
    public float fireRate = 0.1f; // �ӵ��������
    public float cooldownTime = 0.7f; // �ӵ���ȴʱ��
    public float bulletAngleStep = 5;

    [Header("ScaleInfo")]
    public float minScale = 0.9f;
    public float scaleDuration = 0.02f;
    private float originalLocalScale;

    private bool isFiring = false; // �Ƿ����������ӵ�
    private Coroutine fireCoroutine; // Э�̶���

    private Coroutine weaponScaleCoroutine; // ����Э��

    private void Awake()
    {
        _aimTransform = transform;
        originalLocalScale = _aimTransform.localScale.x;
        if(DataHolder.Instance.gametimes == 0)
        {
            DataHolder.Instance.bulletLevel = 1;
        }
    }

    public void HandleShooting()
    {
        // ������������£����Ҳ��������ӵ���״̬��
        if (Input.GetMouseButtonDown(0) && !isFiring)
        {
            // ��ʼ�����ӵ�
            fireCoroutine = StartCoroutine(FireBullets());
        }
        // ����������ͷţ��������������ӵ�
        else if (Input.GetMouseButtonUp(0) && isFiring)
        {
            // ֹͣ�����ӵ������Э�̲�Ϊ�գ�
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }
            isFiring = false;
        }
    }

    private IEnumerator FireBullets()
    {
        isFiring = true;

        float elapsedTime = 0f;

        // ������״̬�£�ÿ��fireRate�뷢��һ���ӵ�
        while (elapsedTime < fireDuration)
        {
            FireBullet();
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(fireRate);
        }

        // ����������ȴ���ȴʱ��
        yield return new WaitForSeconds(cooldownTime);

        isFiring = false;
    }

    private void FireBullet()
    {
        Vector3 mousePosition = Utils.GetMouseWorldPosition();
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
        Vector2 firePointPosition = new Vector2(_firePos.position.x, _firePos.position.y);
        Vector2 shootDirection = mousePosition2D - firePointPosition;

        // �������뷢���֮��ľ������0.5f����ʹ����귽����Ϊ������򣬷���ʹ�����Ϸ���Ϊ�������
        if (Vector2.Distance(mousePosition2D, firePointPosition) > 0.5f)
        {
            shootDirection = mousePosition2D - firePointPosition;
        }
        else
        {
            shootDirection = transform.up;
        }

        // �����ӵ�
        Fire(shootDirection);
        if (weaponScaleCoroutine != null)
        {
            StopCoroutine(weaponScaleCoroutine); // ֹ֮ͣǰ������Э��
            _aimTransform.localScale = new Vector3(originalLocalScale, originalLocalScale, originalLocalScale);
        }
        weaponScaleCoroutine = StartCoroutine(Utils.ScaleEffect(_aimTransform, minScale, scaleDuration)); // ��ʼ�µ�����Ч��
    }

    private void Fire(Vector2 direction)
    {
        if (DataHolder.Instance.bulletLevel == 1)
        {
            ShootSingleBullet(direction);
        }
        else
        {
            ShootBullets(direction, DataHolder.Instance.bulletLevel, bulletAngleStep);
        }
    }

    private void ShootSingleBullet(Vector2 direction)
    {
        GameObject bullet = PoolManager.Release(_bulletPrefab, _firePos.position, Quaternion.identity);
        if (bullet != null)
        {
            bullet.GetComponent<ControllBullet>().SetBulletSpeed(direction);
        }
    }

    private void ShootBullets(Vector2 direction, int bulletCount, float angleStep)
    {
        // ����ÿ���ӵ�֮��ĽǶȼ��
        float totalAngle = angleStep * (bulletCount - 1);
        float adjustedStartAngle = - totalAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = adjustedStartAngle + i * angleStep;
            Vector2 bulletDirection = Quaternion.Euler(0f, 0f, angle) * direction;
            GameObject bullet = PoolManager.Release(_bulletPrefab, _firePos.position, Quaternion.identity);
            if (bullet != null)
            {
                bullet.GetComponent<ControllBullet>().SetBulletSpeed(bulletDirection);
            }
        }
    }

}
