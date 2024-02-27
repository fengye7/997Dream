using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform _aimTransform;
    public WeaponKeyBoard weaponKeyBoard; // ���� WeaponKeyBoard ���ʵ��

    private void Awake()
    {
        _aimTransform = transform.Find("KeyBoard");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleAiming();
        weaponKeyBoard.HandleShooting();
        // ��������������룬ʵ�������л��߼�
        // ���������л��߼�...
    }

    private void SwitchWeapon(int index)
    {
        // ȷ��������Ч
        if (index >= 0 && index < DataHolder.Instance.playerWeaponList.Count)
        {
            // �������л������߼�
            // ���磬���µ�ǰʹ�õ��������ݣ������� UI ��ʾ
        }
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = Utils.GetMouseWorldPosition();
        
        Vector3 _aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x)*Mathf.Rad2Deg;
        _aimTransform.eulerAngles = new Vector3(0,0,angle);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < _aimTransform.position.x)
            _aimTransform.localScale = new Vector3(-1, 1, 1); //��ת
        else _aimTransform.localScale = Vector3.one;
    }
}

