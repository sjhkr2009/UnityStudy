using UnityEngine;

public class AmmoPack : MonoBehaviour, IItem
{
    public int ammo = 30;

    public void Use(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        if(playerShooter != null)
            playerShooter.gun.ammoRemain += ammo;

        Destroy(gameObject);
    }
}