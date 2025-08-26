using UnityEngine;

public class SnowTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem snowParticleSystem;

    void OnCollisionEnter2D(Collision2D collision)
    {
        snowParticleSystem.Play();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        snowParticleSystem.Stop();
    }
}
