using UnityEngine;

public class WaterRefillZone : MonoBehaviour
{
    public float refillRate = 50f;   //water per second

    private bool playerInside = false;
    private WateringCan wateringCan;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            wateringCan = other.GetComponentInChildren<WateringCan>();

            Debug.Log("Entered Water Refill Zone");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            wateringCan = null;

            Debug.Log("Exited Water Refill Zone");
        }
    }

    void Update()
    {
        if (!playerInside || wateringCan == null)
            return;

        if (Input.GetKey(KeyCode.E))
        {
            RefillWater();
        }
    }

    void RefillWater()
    {
        if (wateringCan == null)
            return;

        wateringCan.RefillWater(refillRate * Time.deltaTime);
    }
}
