using TMPro;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public float capacity = 100f;
    public float currentWater = 100f;
    public float waterPerSecond = 20f;
    public float useRange = 4f;
    public Camera playerCamera;
    public TextMeshProUGUI WateringCanText;

    void Start()
    {
        UpdateWaterUI();
    }


    void OnEnable()
    {
        UpdateWaterUI();
    }


    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (Input.GetMouseButton(0) && currentWater > 0f)
        {
            Water();
        }
    }
    void Water()
    {
        float used = waterPerSecond * Time.deltaTime;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, useRange))
        {
            PlantObjective plant = hit.collider.GetComponent<PlantObjective>();

            if (plant != null)
            {
                //add objective water
                plant.AddWater(used);

                //drain can water
                currentWater -= used;
                currentWater = Mathf.Clamp(currentWater, 0, capacity);

                UpdateWaterUI();

                Debug.Log("Water added to plant! Current water: " + Mathf.RoundToInt(currentWater));
            }
        }
    }

    public void RefillWater(float amount)
    {
        currentWater += amount;
        currentWater = Mathf.Clamp(currentWater, 0, capacity);
        UpdateWaterUI();
    }

    void UpdateWaterUI()
    {
        if (WateringCanText != null)
        {
            WateringCanText.text =
                "Water: " +
                Mathf.RoundToInt(currentWater) +
                " / " +
                Mathf.RoundToInt(capacity);
        }
    }
}