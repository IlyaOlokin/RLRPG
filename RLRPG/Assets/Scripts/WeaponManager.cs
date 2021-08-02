using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EZCameraShake;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    private Weapon equippedWeapon;
    private GameObject loadedBullet;
    public CameraShaker cameraShaker;
    public GameObject collectableWeaponPref;
    public GameObject collectableBulletPref;

    public Weapon defaultWeapon;
    public GameObject defaultBullet;

    public Weapon[] weapons = new Weapon[2];
    public GameObject[] bullets = new GameObject[2];
    
    public SlotsUI slotsUI;

    private float currentCd;
    private LineRenderer lr;
    private Player player;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        player = transform.parent.GetComponent<Player>();
        EquipWeapon(-1);
        LoadBullet(-1);
        slotsUI.UpdateSlotSprites();
    }
    
    private void Update()
    {
        currentCd += Time.deltaTime;
        if (currentCd > equippedWeapon.coolDown) currentCd = equippedWeapon.coolDown;
        if (Input.GetMouseButtonDown(0) && !equippedWeapon.autoShoot)
        {
            Shoot();
        }
        else if (Input.GetMouseButton(0) && equippedWeapon.autoShoot)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ThrowWeapon(GetEquippedWeaponIndex());
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowBullet(GetLoadedBulletIndex());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadBullet(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LoadBullet(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LoadBullet(1);
        }

        if (equippedWeapon.hasLaserSight)
        {
            lr.enabled = true;
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var startPos = transform.position;
            var endPos = Vector2.MoveTowards(startPos, mousePosition, equippedWeapon.laserSightLenght);
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, endPos);
        }
    }

    private void Shoot()
    {
        if (currentCd >= equippedWeapon.coolDown)
        {
            StartCoroutine(MakeShots());
            currentCd -= equippedWeapon.coolDown;
        }
    }

    private void WeaponShoot()
    {
        equippedWeapon.Shoot();
    }

    IEnumerator MakeShots()
    {
        for (int i = 0; i < equippedWeapon.multiShot; i++)
        {
            WeaponShoot();
            cameraShaker.ShakeOnce(2f, 1, 0.05f, 0.2f);
            yield return new WaitForSeconds(0.05f);
            
        }
    }

    private void EquipWeapon(int index)
    {
        if (index == -1)
        {
            equippedWeapon = defaultWeapon;
        }
        else
        {
            if (weapons[index] == null) return;
            equippedWeapon = weapons[index];
        }
        LoadBullet(GetLoadedBulletIndex());
        equippedWeapon.SourceTransform = transform.parent.transform;
        switch (equippedWeapon.weaponType)
        {
            case Weapon.WeaponType.Rifle:
                equippedWeapon.weapon = new Rifle();
                break;
            case Weapon.WeaponType.Shotgun:
                equippedWeapon.weapon = new Shotgun();
                break;
            case Weapon.WeaponType.PlasmaGun:
                equippedWeapon.weapon = new PlasmaGun();
                break;
        }
        lr.enabled = equippedWeapon.hasLaserSight;
        slotsUI.SetSelectorIndicatorWeaponPosition(index);
    }

    private void LoadBullet(int index)
    {
        if (index == -1)
        {
            loadedBullet = defaultBullet;
        }
        else
        {
            if (bullets[index] == null) return;
            loadedBullet = bullets[index];
        }

        equippedWeapon.bullet = loadedBullet;
        slotsUI.SetSelectorIndicatorBulletPosition(index);
    }

    public void CollectNewWeapon(Weapon newWeapon)
    {
        int slotIndex = -1;
        if (HasEmptyWeaponSlots())
        {
            slotIndex = FindEmptyWeaponSlot();
        }
        else if (equippedWeapon == defaultWeapon)
        {
            return;
        }
        else
        {
            var equippedWeaponIndex = GetEquippedWeaponIndex();
            slotIndex = equippedWeaponIndex;
            ThrowWeapon(equippedWeaponIndex);
        }
        weapons[slotIndex] = newWeapon;
        EquipWeapon(slotIndex);
        slotsUI.UpdateSlotSprites();
    }
    
    public void CollectNewBullet(GameObject newBullet)
    {
        int slotIndex = -1;
        if (HasEmptyBulletSlots())
        {
            slotIndex = FindEmptyBulletSlot();
        }
        else if (loadedBullet == defaultBullet)
        {
            return;
        }
        else
        {
            var loadedBulletIndex = GetLoadedBulletIndex();
            slotIndex = loadedBulletIndex;
            ThrowBullet(loadedBulletIndex);
        }
        bullets[slotIndex] = newBullet;
        LoadBullet(slotIndex);
        slotsUI.UpdateSlotSprites();
    }

    private void ThrowWeapon(int index)
    {
        if (index == -1) return;
        var droppedWeapon = Instantiate(collectableWeaponPref);
        droppedWeapon.transform.position = transform.position;
        droppedWeapon.GetComponent<CollectableWeapon>().weapon = weapons[index];
        weapons[index] = null;
        
        EquipWeapon(AllWeaponSlotsEmpty() ? -1 : ++index % 2);
        
        slotsUI.UpdateSlotSprites();
    }
    
    private void ThrowBullet(int index)
    {
        if (index == -1) return;
        var droppedBullet = Instantiate(collectableBulletPref);
        droppedBullet.transform.position = transform.position;
        droppedBullet.GetComponent<CollectableBullet>().bullet = bullets[index];
        bullets[index] = null;
        
        LoadBullet(AllBulletSlotsEmpty() ? -1 : ++index % 2);
        
        slotsUI.UpdateSlotSprites();
    }

    private int FindWeaponIndex(Weapon weapon)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == weapon)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetEquippedWeaponIndex()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == equippedWeapon)
            {
                return i;
            }
        }
        return -1;
    }
    
    public int GetLoadedBulletIndex()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i] == loadedBullet)
            {
                return i;
            }
        }
        return -1;
    }

    public bool HasEmptyWeaponSlots()
    {
        bool hasEmptyWeaponSlots = false;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                hasEmptyWeaponSlots = true;
            }
        }

        return hasEmptyWeaponSlots;
    }
    
    public bool HasEmptyBulletSlots()
    {
        bool hHasEmptyBulletSlots = false;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i] == null)
            {
                hHasEmptyBulletSlots = true;
            }
        }

        return hHasEmptyBulletSlots;
    }

    private int FindEmptyWeaponSlot()
    {
        var emptySlotIndex = -1;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                emptySlotIndex = i;
                break;
            }
        }

        return emptySlotIndex;
    }
    
    private int FindEmptyBulletSlot()
    {
        var emptySlotIndex = -1;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i] == null)
            {
                emptySlotIndex = i;
                break;
            }
        }

        return emptySlotIndex;
    }

    private bool AllWeaponSlotsEmpty()
    {
        return weapons.All(t => t == null);
    }
    private bool AllBulletSlotsEmpty()
    {
        return bullets.All(t => t == null);
    }

    
}
