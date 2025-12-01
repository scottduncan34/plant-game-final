using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons;
    public bool[] unlocked;

    private int currentWeaponIndex = 0;

    void Awake()
    {
        //debug message
        if (weapons.Length == 0)
        {
            Debug.LogError("WeaponManager has NO weapons assigned!");
            return;
        }

        //ensure unlock array matches size
        if (unlocked == null || unlocked.Length != weapons.Length)
        {
            unlocked = new bool[weapons.Length];
        }

        //force spawn weapons
        unlocked[0] = true;
    }

    void Start()
    {
        currentWeaponIndex = 0;
        SelectWeapon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TrySelect(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TrySelect(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TrySelect(2);
    }

    void TrySelect(int index)
    {
        if (index >= weapons.Length)
            return;

        if (!unlocked[index])
        {
            Debug.Log("Weapon locked!");
            return;
        }

        currentWeaponIndex = index;
        SelectWeapon();
    }

    void SelectWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
                weapons[i].SetActive(i == currentWeaponIndex);
        }

        Debug.Log("Equipped: " + weapons[currentWeaponIndex].name);
    }

    //unlock new weapon
    public void UnlockWeapon(int index)
    {
        if (index >= weapons.Length)
            return;

        unlocked[index] = true;
        Debug.Log("Weapon unlocked: " + weapons[index].name);
    }
}
