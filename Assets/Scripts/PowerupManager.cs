using System;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public PlayerController playerController;
    public PowerupSO speedPowerup;
    public PowerupSO torquePowerup;

    private void Start()
    {
        playerController.OnPowerupCollected += HandlePowerupCollected;
    }

    private void HandlePowerupCollected()
    {
        playerController.PowerupSpeed(speedPowerup);
        playerController.PowerupTorque(torquePowerup);
    }
}
