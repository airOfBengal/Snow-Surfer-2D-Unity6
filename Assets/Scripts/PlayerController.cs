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
    [SerializeField] private float baseSpeed = 12f;
    [SerializeField] private float boostSpeed = 15f;
    private Vector2 moveInput;
    private SurfaceEffector2D surfaceEffector2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindFirstObjectByType<SurfaceEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        BoostPlayer();
    }

    private void MovePlayer()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        rb.AddTorque(-moveInput.x * torqueAmount);
    }

    private void BoostPlayer()
    {
        if (moveInput.y > 0)
        {
            surfaceEffector2D.speed = boostSpeed;
        }
        else
        {
            surfaceEffector2D.speed = baseSpeed;
        }
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
