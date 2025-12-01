using UnityEngine;

public class GunBuyZone : MonoBehaviour
{
    public int weaponIndexToUnlock = 1; //which weapon in WeaponManager
    public int cost = 50;
    public KeyCode buyKey = KeyCode.E;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(buyKey))
        {
            TryBuyWeapon();
        }
    }

    //unlock weapon if player meets point requirement
    void TryBuyWeapon()
    {
        if (GameStateManager.Instance == null)
            return;

        if (GameStateManager.Instance.SpendPoints(cost))
        {
            WeaponManager wm = FindObjectOfType<WeaponManager>();
            if (wm != null)
            {
                wm.UnlockWeapon(weaponIndexToUnlock);
                Debug.Log("Weapon Purchased!");
            }
        }
        else
        {
            Debug.Log("Not enough points to buy weapon.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Press E to buy weapon");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
