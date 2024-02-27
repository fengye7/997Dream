using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform _aimTransform;
    public WeaponKeyBoard weaponKeyBoard; // 引用 WeaponKeyBoard 类的实例

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
        // 在这里检测玩家输入，实现武器切换逻辑
        // 其他武器切换逻辑...
    }

    private void SwitchWeapon(int index)
    {
        // 确保索引有效
        if (index >= 0 && index < DataHolder.Instance.playerWeaponList.Count)
        {
            // 在这里切换武器逻辑
            // 例如，更新当前使用的武器数据，并更新 UI 显示
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
            _aimTransform.localScale = new Vector3(-1, 1, 1); //翻转
        else _aimTransform.localScale = Vector3.one;
    }
}

