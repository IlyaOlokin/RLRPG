using UnityEngine;


public class CollectableBullet : MonoBehaviour
{
    public GameObject bullet;
    private Transform player;
    [SerializeField] private GameObject keyE;
    [SerializeField] private float distToShowKeyE;
    private WeaponManager weaponManager;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = bullet.GetComponent<SpriteRenderer>().sprite;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        weaponManager = player.GetComponent<Player>().weaponManager;
    }

    void Update()
    {
        var readyToCollect = Vector2.Distance(transform.position, player.position) < distToShowKeyE;
        keyE.SetActive(readyToCollect);

        if (Input.GetKeyDown(KeyCode.E) && readyToCollect &&
            (weaponManager.GetLoadedBulletIndex() != -1 || weaponManager.HasEmptyBulletSlots())) 
        {
            weaponManager.CollectNewBullet(bullet);
            Destroy(gameObject);
        }
    }
}