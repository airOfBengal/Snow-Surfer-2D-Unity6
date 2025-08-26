using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private ParticleSystem finishEffect;

    public void PlayFinishEffect()
    {
        finishEffect.gameObject.SetActive(true);
        finishEffect.Play();
    }
}
