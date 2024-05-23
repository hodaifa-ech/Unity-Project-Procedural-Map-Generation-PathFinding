using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected GameObject muzzle;
    [SerializeField]
    protected int ammo = 10;
    [SerializeField]
    protected WeaponDataSO weaponData;
    public int Ammo
    {
        get { return ammo; }
        set
        {
            ammo = Mathf.Clamp(value, 0, weaponData.AmmoCapacity);
            OnAmmoChange?.Invoke(ammo);
        }
    }
    public Player coin;
    public bool AmmoFull { get => Ammo >= weaponData.AmmoCapacity; }

    protected bool isShooting = false;
    [SerializeField]
    private UILevel uilevel;

    [SerializeField]
    protected bool reloadCoroutine = false;

    [field: SerializeField]
    public UnityEvent OnShoot { get; set; }

    [field: SerializeField]
    public UnityEvent OnShootNoAmmo { get; set; }
    [field: SerializeField]
    public UnityEvent<int> OnAmmoChange { get; set; }
    public Button level;

    private bool upgradeActivated = false; // Flag to check if upgrade is activated

    private void Start()
    {
        Ammo = weaponData.AmmoCapacity;
        level.onClick.AddListener(UpgradeButtonClick); // Assign the button click event
    }





    private void UpgradeButtonClick()
    {
        if (coin.coin >= 200)
        {
            upgradeActivated = true;
            uilevel.Udpatelevel(2);
        }
    }

    public void TryShooting()
    {
        Debug.Log("Shooting");
        isShooting = true;
    }
    public void StopShooting()
    {
        Debug.Log("Stop shooting");
        isShooting = false;
    }

    public void Reload(int ammo)
    {
        Ammo += ammo;
    }

    private void Update()
    {
        UseWeapon();
    }

    private void UseWeapon()
    {
        if (isShooting && reloadCoroutine == false)
        {
            if (Ammo > 0)
            {
                Ammo--;
                OnShoot?.Invoke();
                for (int i = 0; i < weaponData.GetBulletCountToSpawn(); i++)
                {
                    ShootBullet();
                }
            }
            else
            {
                isShooting = false;
                OnShootNoAmmo?.Invoke();
                return;
            }
            FinishShooting();
        }
    }

    private void FinishShooting()
    {
        StartCoroutine(DelayNextShootCoroutine());
        if (weaponData.AutomaticFire == false)
        {
            isShooting = false;
        }
    }

    protected IEnumerator DelayNextShootCoroutine()
    {
        reloadCoroutine = true;
        yield return new WaitForSeconds(weaponData.WeaponDelay);
        reloadCoroutine = false;
    }

    public void ShootBullet()
    {
        SpawnBullet(muzzle.transform.position, CalculateAngle(muzzle));
    }

    public void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        var bulletPrefab = Instantiate(weaponData.BulletData.bulletPrefab, position, rotation);
        if (upgradeActivated)
        {
            Renderer bulletRenderer = bulletPrefab.GetComponent<Renderer>();
            if (bulletRenderer != null)
            {
                bulletRenderer.material.color = Color.HSVToRGB(0.58f, 0.7f, 1f); // Change color to blue
            }
        }

        bulletPrefab.GetComponent<Bullet>().BulletData = weaponData.BulletData;
    }

    private Quaternion CalculateAngle(GameObject muzzle)
    {
        float spread = Random.Range(-weaponData.SpreadAngle, weaponData.SpreadAngle);
        Quaternion bulletSpreadRotation = Quaternion.Euler(new Vector3(0, 0, spread));
        return muzzle.transform.rotation * bulletSpreadRotation;
    }
}