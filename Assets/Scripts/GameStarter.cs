using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallController : MonoBehaviour
{
    [Header("SPAWN SETTINGS")]
    [Tooltip("Set exact starting coordinates for the ball")]
    public Vector3 manualStartPosition = new Vector3(0f, 3f, 0f);
    public GameObject startPanel;
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float gravityStrength = 1.5f;
    [SerializeField] private float initialSpeed = 3f;

    [Header("TOUCH CONTROLS")] 
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float maxHorizontalVelocity = 4f;
    [SerializeField] private float scoreCooldown = 0.2f;
    [SerializeField] private ParticleSystem touchParticles;
    [SerializeField] private AudioClip jumpSound;

    [Header("REFERENCES")] 
    [SerializeField] private Camera mainCamera;

    private Rigidbody2D rb;
    private float lastScoreTime;
    private bool gameStarted = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!mainCamera) mainCamera = Camera.main;
    }

    void Start()
    {
        ResetToStartPosition();
    }

    public void ResetToStartPosition()
    {
        // Reset transform and physics
        transform.position = manualStartPosition;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        
        // Reset game state
        gameStarted = false;
        lastScoreTime = -scoreCooldown;
        
        // Show start panel if exists
        if(startPanel) startPanel.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine(GameStartRoutine());
    }

    IEnumerator GameStartRoutine()
    {
        // Hide start UI
        if(startPanel) startPanel.SetActive(false);
        
        // Wait for start delay
        yield return new WaitForSecondsRealtime(startDelay);
        
        // Activate game physics
        gameStarted = true;
        rb.gravityScale = gravityStrength;
        rb.AddForce(Vector2.down * initialSpeed, ForceMode2D.Impulse);
    }

    void Update()
    {
        if(!gameStarted) return;
        
        HandleInput();
        ClampHorizontalVelocity();
    }

    private void HandleInput()
    {
        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || 
           Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(GetInputPosition());
            
            if(GetComponent<Collider2D>().OverlapPoint(worldPos))
            {
                OnValidTouch();
            }
        }
    }

    private Vector2 GetInputPosition()
    {
        return Input.touchCount > 0 ? 
               Input.GetTouch(0).position : 
               (Vector2)Input.mousePosition;
    }

    private void OnValidTouch()
    {
        if(Time.time > lastScoreTime + scoreCooldown)
        {
            ScoreSystem.Instance?.AddScore(1);
            lastScoreTime = Time.time;
        }
        
        ApplyJumpForce();
        PlayTouchEffects();
    }

    private void ApplyJumpForce()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), 0), ForceMode2D.Impulse);
    }

    private void PlayTouchEffects()
    {
        if(touchParticles) Instantiate(touchParticles, transform.position, Quaternion.identity);
        if(jumpSound) AudioSource.PlayClipAtPoint(jumpSound, transform.position);
    }

    private void ClampHorizontalVelocity()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalVelocity, maxHorizontalVelocity);
        rb.velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            LivesSystem.Instance?.LoseLife();
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(manualStartPosition, 0.25f);
        Gizmos.DrawWireCube(manualStartPosition, Vector3.one * 0.5f);
        UnityEditor.Handles.Label(manualStartPosition + Vector3.up * 0.5f, "Ball Start Pos");
    }
    #endif
}