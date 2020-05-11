using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Arrow, Bullet, Missile }

public class WeaponManager : MonoBehaviour
{
    public GameObject arrow;
    public GameObject bullet;
    public GameObject missile;
    [SerializeField] GameObject currentWeapon;

    private IWeapon weapon;

    private void SetWeaponType(WeaponType weaponType)
    {
        Component c = (Component)gameObject.GetComponent<IWeapon>(); //현재 적용된 IWeapon 계열의 컴포넌트를 불러와서
        if (c != null) Destroy(c); //제거한다 (최초 실행 시에는 없으니 if문을 붙인다)


        //인자에 따라 새로운 컴포넌트를 불러와서 적용한다.
        switch (weaponType)
        {
            case WeaponType.Arrow:
                weapon = gameObject.AddComponent<Arrow>(); //현재 플레이어에게 적용된 무기 발사 매커니즘
                currentWeapon = arrow; //현재 발사할 투사체
                break;
            case WeaponType.Bullet:
                weapon = gameObject.AddComponent<Bullet>();
                currentWeapon = bullet;
                break;
            case WeaponType.Missile:
                weapon = gameObject.AddComponent<Missile>();
                currentWeapon = missile;
                break;
            default:
                weapon = gameObject.AddComponent<Arrow>();
                currentWeapon = arrow;
                break;
        }
    }

    private void Start()
    {
        SetWeaponType(WeaponType.Arrow);
    }

    public void ChangeToArrow()
    {
        SetWeaponType(WeaponType.Arrow);
    }
    public void ChangeToBullet()
    {
        SetWeaponType(WeaponType.Bullet);
    }
    public void ChangeToMissile()
    {
        SetWeaponType(WeaponType.Missile);
    }

    public void Fire()
    {
        weapon.Shoot(currentWeapon);
    }
}
