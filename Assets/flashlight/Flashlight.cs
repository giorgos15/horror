using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public bool isOn = false;
    public GameObject lightSource;
    public AudioSource clickSound;
    public bool failSafe = false;
    public float batteryLife = 100.0f; // Initial battery life (adjust as needed)
    public float batteryDrainRate = 5.0f; // Rate at which battery drains per second
    public float refillRate = 10.0f;
    public float maxBatteryLife = 100.0f;
    public float stationaryBatteryDrainRate = 2.0f;
    public float movingBatteryDrainRate = 10.0f;
    public Slider batterySlider;
    private CharacterController playerController;

    private void Start()
    {
        playerController = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isOn == false && failSafe == false && batteryLife > 0)
            {
                failSafe = true;
                lightSource.SetActive(true);
                clickSound.Play();
                isOn = true;
                StartCoroutine(FailSafe());
            }
            else if (isOn == true && failSafe == false)
            {
                failSafe = true;
                lightSource.SetActive(false);
                clickSound.Play();
                isOn = false;
                StartCoroutine(FailSafe());
            }
        }

        //check for movement
        bool isPlayerMoving = playerController.velocity.magnitude > 0.1f;
        float currentBatteryDrainRate = isOn ? (isPlayerMoving ? movingBatteryDrainRate : stationaryBatteryDrainRate) : 0;

        // Drain battery when the flashlight is on
        if (isOn)
        {
            DrainBattery(currentBatteryDrainRate);
        }
        else
        {
            RefillBattery();
        }

        if (isOn && batterySlider !=null)
        {
            float batteryPercentage = (batteryLife / maxBatteryLife) *100;
            batterySlider.value = batteryPercentage;
        }
    }

    void DrainBattery(float batteryDrainRate)
    {
        batteryLife -= batteryDrainRate * Time.deltaTime;

        // Ensure battery life does not go below zero
        batteryLife = Mathf.Max(batteryLife, 0);

        // Check if the battery has run out
        if (batteryLife <= 0)
        {
            BatteryEmpty();
        }
    }

    void RefillBattery()
    {
        batteryLife += refillRate * Time.deltaTime;

        // Ensure battery life does not exceed the maximum capacity
        batteryLife = Mathf.Min(batteryLife, maxBatteryLife);

        // Update the Slider's value here
        if (batterySlider != null)
        {
            float batteryPercentage = (batteryLife / maxBatteryLife) * 100;
            batterySlider.value = batteryPercentage;
        }
    }


    void BatteryEmpty()
    {
        // Implement logic for when the battery is empty (e.g., turn off the flashlight)
        lightSource.SetActive(false);
        isOn = false;
        // Optionally, play a low battery sound or perform other actions
    }

    IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(0.25f);
        failSafe = false;
    }
}