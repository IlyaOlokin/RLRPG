using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsUI : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private GameObject selectIndicatorWeapon;
    [SerializeField] private GameObject selectIndicatorBullet;
    public GameObject[] weaponSpriteSlots = new GameObject[2];
    public GameObject defaultWeaponSpriteSlot;
    public GameObject[] bulletSpriteSlots = new GameObject[2];
    public GameObject defaultBulletSpriteSlot;



    public void UpdateSlotSprites()
    {
        defaultWeaponSpriteSlot.GetComponent<Image>().sprite = weaponManager.defaultWeapon.sprite;
        defaultBulletSpriteSlot.GetComponent<Image>().sprite =
            weaponManager.defaultBullet.GetComponent<SpriteRenderer>().sprite;
        
        //temp code
        defaultBulletSpriteSlot.GetComponent<Image>().color =
            weaponManager.defaultBullet.GetComponent<SpriteRenderer>().color;
        //
        
        for (int i = 0; i < weaponManager.weapons.Length; i++)
        {
            if (weaponManager.weapons[i] != null)
            {
                weaponSpriteSlots[i].SetActive(true);
                weaponSpriteSlots[i].GetComponent<Image>().sprite = weaponManager.weapons[i].sprite;
            }
            else
            {
                weaponSpriteSlots[i].GetComponent<Image>().sprite = null;
                weaponSpriteSlots[i].SetActive(false);
            }
        }
        
        for (int i = 0; i < weaponManager.bullets.Length; i++)
        {
            if (weaponManager.bullets[i] != null)
            {
                bulletSpriteSlots[i].SetActive(true);
                bulletSpriteSlots[i].GetComponent<Image>().sprite =
                    weaponManager.bullets[i].GetComponent<SpriteRenderer>().sprite;
                // temp code
                bulletSpriteSlots[i].GetComponent<Image>().color =
                    weaponManager.bullets[i].GetComponent<SpriteRenderer>().color;
                //
            }
            else
            {
                bulletSpriteSlots[i].GetComponent<Image>().sprite = null;
                bulletSpriteSlots[i].SetActive(false);
            }
        }
    }

    public void SetSelectorIndicatorWeaponPosition(int index)
    {
        if (index == -1)
        {
            selectIndicatorWeapon.transform.position = defaultWeaponSpriteSlot.transform.position;
        }

        for (int i = 0; i < weaponSpriteSlots.Length; i++)
        {
            if (index == i)
            {
                selectIndicatorWeapon.transform.position = weaponSpriteSlots[i].transform.position;
            }
        }
    }

    public void SetSelectorIndicatorBulletPosition(int index)
    {
        if (index == -1)
        {
            selectIndicatorBullet.transform.position = defaultBulletSpriteSlot.transform.position;
        }
        for (int i = 0; i < bulletSpriteSlots.Length; i++)
        {
            if (index == i)
            {
                selectIndicatorBullet.transform.position = bulletSpriteSlots[i].transform.position;
            }
        }
    }
}
