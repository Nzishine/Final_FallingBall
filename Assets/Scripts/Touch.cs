using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallTouchController : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float maxHorizontalVelocity = 5f;
    [SerializeField] private float scoreCooldown = 0.2f; // Prevent rapid scoring

    [Header("Feedback")]
    [SerializeField] private ParticleSystem touchParticles;
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private float lastScoreTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        lastScoreTime = -scoreCooldown; // Allow immediate first score
    }

    void Update()
    {
        HandleInput();
        ClampHorizontalVelocity();
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
        return Input.touchCount > 0 ? 
            Input.GetTouch(0).position : 
            (Vector2)Input.mousePosition;
    }

    private void HandleSuccessfulTouch()
    {
        // Score only if cooldown has passed
        if (Time.time > lastScoreTime + scoreCooldown)
        {
            AddScore();
            lastScoreTime = Time.time;
        }

        ApplyJumpForce();
        PlayTouchEffects();
    }

    private void AddScore()
    {
        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.AddScore(1);
        }
        else
        {
            Debug.LogWarning("ScoreSystem instance missing!");
        }
    }

    private void ApplyJumpForce()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        float randomX = Random.Range(-1f, 1f);
        rb.AddForce(new Vector2(randomX, 0), ForceMode2D.Impulse);
    }

    private void PlayTouchEffects()
    {
        if (touchParticles != null) touchParticles.Play();
        if (jumpSound != null) AudioSource.PlayClipAtPoint(jumpSound, transform.position);
    }

    private void ClampHorizontalVelocity()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalVelocity, maxHorizontalVelocity);
        rb.velocity = velocity;
    }

    // Add this to your Ball's existing script:
void OnCollisionEnter2D(Collision2D col)
{
    if (col.gameObject.CompareTag("Ground"))
    {
        LivesSystem.Instance.LoseLife(); // Heart decrease happens here
    }
}

void ResetColor()
{
    GetComponent<SpriteRenderer>().color = Color.white;
}
}