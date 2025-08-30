using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public event Action<int> OnFlipAchieved;
    public event Action<int> OnFinishAchieved;
    public event Action OnPowerupCollected;
    private InputAction moveAction;
    private Rigidbody2D rb;
    [SerializeField] private float torqueAmount = 1f;
    [SerializeField] private float baseTorque = 1f;
    [SerializeField] private float restartDelay = 1f;
    [SerializeField] private ParticleSystem crashEffect;
    [SerializeField] private float baseSpeed = 12f;
    [SerializeField] private float boostSpeed = 15f;
    private Vector2 moveInput;
    private SurfaceEffector2D surfaceEffector2D;
    bool canControl = true;
    [SerializeField] private int flipScore = 100;
    [SerializeField] private int finishScore = 500;

    // flip count
    float previousRotation;
    float totalRotation;
    int flipCount = 0;

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
        if (canControl)
        {
            MovePlayer();
            BoostPlayer();
            CalculateFlips();
        }
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
            OnFinishAchieved?.Invoke(finishScore);
            canControl = false;
        }

        int layerIndex = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layerIndex)
        {
            OnCrash();
            canControl = false;
        }

        layerIndex = LayerMask.NameToLayer("Collectibles");
        if (collision.gameObject.layer == layerIndex)
        {
            OnPowerupCollected?.Invoke();
            Destroy(collision.gameObject);
            return;
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

    private void CalculateFlips()
    {
        float currentRotation = transform.eulerAngles.z;
        totalRotation += Mathf.DeltaAngle(previousRotation, currentRotation);
        if (totalRotation > 340 || totalRotation < -340)
        {
            flipCount++;
            totalRotation = 0;
            OnFlipAchieved?.Invoke(flipScore);
        }
        previousRotation = currentRotation;
    }

    public void PowerupSpeed(PowerupSO speedPowerup)
    {
        surfaceEffector2D.speed += speedPowerup.Powerup;
        Invoke(nameof(ResetSpeed), speedPowerup.Duration);
    }

    public void PowerupTorque(PowerupSO torquePowerup)
    {
        torqueAmount += torquePowerup.Powerup;
        Invoke(nameof(ResetTorque), torquePowerup.Duration);
    }

    private void ResetSpeed()
    {
        surfaceEffector2D.speed = baseSpeed;
    }
    
    private void ResetTorque()
    {
        torqueAmount = baseTorque;
    }
}
