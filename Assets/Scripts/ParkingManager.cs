using UnityEngine;
using UnityEngine.UI;

public class ParkingManager : MonoBehaviour
{
    public Car car;
    public Slider parkingSlider;
    public Transform parkingLot;
    public Toggle[] carParkingToggle;

    private bool isParkingCorrect = false;
    private float parkingSpeed = 0.5f; // Adjust this value as needed

    void Update()
    {
        if (car.IsInsideParkingLot && parkingSlider != null && !isParkingCorrect)
        {
            float targetValue = CalculateParkingPercentage();
            parkingSlider.value = Mathf.MoveTowards(parkingSlider.value, targetValue, parkingSpeed * Time.deltaTime);
            if (Mathf.Approximately(parkingSlider.value, targetValue))
            {
                isParkingCorrect = targetValue >= 0.85f; // Threshold for parking correctness
                if (isParkingCorrect)
                {
                    carParkingToggle[0].isOn = true;
                    car.DisplayMessage("Car is parked correctly!");
                }
            }
        }
    }

    private float CalculateParkingPercentage()
    {
        // Get the size of the parking space
        Vector2 parkingLotSize = parkingLot.GetComponent<Renderer>().bounds.size;
        Vector2 localCarPosition = parkingLot.InverseTransformPoint(car.transform.position);

        float percentageX = Mathf.Clamp01(1f - Mathf.Abs(localCarPosition.x) / (parkingLotSize.x / 2f));
        float percentageY = Mathf.Clamp01(1f - Mathf.Abs(localCarPosition.y) / (parkingLotSize.y / 2f));
        return Mathf.Max(percentageX, percentageY);
    }

    public void ExitParkingLot()
    {
        if (parkingSlider != null)
            parkingSlider.value = 0f;
        isParkingCorrect = false;
    }
}
