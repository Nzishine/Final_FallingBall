using UnityEngine;
using UnityEngine.UI;
using System.Collections;



[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallController : MonoBehaviour
{
    [Header("Game Start Settings")]
    public GameObject startPanel;
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float gravityStrength = 1.5f;
    [SerializeField] private float initialSpeed = 3f;

    [Header("Touch Controls")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float maxHorizontalVelocity = 4f;
    [SerializeField] private float scoreCooldown = 0.2f;
    [SerializeField] private ParticleSystem touchParticles;
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private float lastScoreTime;
    private bool gameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        lastScoreTime = -scoreCooldown;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }

    void Update()
    {
        if (gameStarted)
        {
            HandleInput();
            ClampHorizontalVelocity();
        }
    }

    public void StartGame()
    {
        StartCoroutine(DelayedPhysicsStart());
    }

    IEnumerator DelayedPhysicsStart()
    {
        startPanel.SetActive(false);
        yield return new WaitForSecondsRealtime(startDelay);
        
        gameStarted = true;
        rb.gravityScale = gravityStrength;
        rb.AddForce(Vector2.down * initialSpeed, ForceMode2D.Impulse);
        rb.WakeUp();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            Vector2 inputPos = GetInputPosition();
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(inputPos);

            if (GetComponent<Collider2D>().OverlapPoint(worldPos))
            {
                HandleSuccessfulTouch();
            }
        }
    }

    private Vector2 GetInputPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            return Input.mousePosition;
        }
        return Vector2.zero; // Default return value
    }

    private void HandleSuccessfulTouch()
    {
        if (Time.time > lastScoreTime + scoreCooldown)
        {
            AddScore();
            lastScoreTime = Time.time;
        }
        ApplyJumpForce();
        //PlayTouchEffects();
    }

    private void AddScore()
    {
        ScoreSystem.Instance?.AddScore(1);
    }

    private void ApplyJumpForce()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        float randomX = Random.Range(-1f, 1f);
        rb.AddForce(new Vector2(randomX, 0), ForceMode2D.Impulse);
    }

   

    private void ClampHorizontalVelocity()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalVelocity, maxHorizontalVelocity);
        rb.velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            LivesSystem.Instance?.LoseLife();
        }
    }
}