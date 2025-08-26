using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private Rigidbody2D rb;
    [SerializeField] private float torqueAmount = 1f;
    [SerializeField] private float restartDelay = 1f;
    [SerializeField] private ParticleSystem crashEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        rb.AddTorque(-moveInput.x * torqueAmount);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FinishLine"))
        {
            FinishLine finishLine = collision.GetComponent<FinishLine>();
            finishLine.PlayFinishEffect();
        }

        int layerIndex = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layerIndex)
        {
            OnCrash();
        }

        Invoke(nameof(ReloadScene), restartDelay);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    void OnCrash()
    {
        crashEffect.gameObject.SetActive(true);
        crashEffect.Play();
    }
}
