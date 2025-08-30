using UnityEngine;

[CreateAssetMenu(fileName = "Powerup", menuName = "PowerupSO")]
public class PowerupSO : ScriptableObject
{
    [SerializeField] private string powerupType;
    [SerializeField] private float powerup;
    [SerializeField] private float duration;

    public string PowerupType => powerupType;
    public float Powerup => powerup;  
    public float Duration => duration;
}
