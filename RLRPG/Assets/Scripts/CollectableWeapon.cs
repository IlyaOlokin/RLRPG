using UnityEngine;


public class CollectableWeapon : MonoBehaviour
{
    public Weapon weapon;
    private Transform player;
    [SerializeField] private GameObject keyE;
    [SerializeField] private float distToShowKeyE;
    private WeaponManager weaponManager;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = weapon.sprite;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    
        weaponManager = player.GetComponent<Player>().weaponManager;
    }

    void Update()
    {
        var readyToCollect = Vector2.Distance(transform.position, player.position) < distToShowKeyE;
        keyE.SetActive(readyToCollect);

        if (Input.GetKeyDown(KeyCode.E) && readyToCollect &&
            (weaponManager.GetEquippedWeaponIndex() != -1 || weaponManager.HasEmptyWeaponSlots())) 
        {
            weaponManager.CollectNewWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
