using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class GameStarter : MonoBehaviour
{
    public Vector3 manualStartPosition = new Vector3(0f, 3f, 0f);
    public GameObject startPanel;
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float gravityStrength = 1.5f;
    [SerializeField] private float initialSpeed = 3f;

    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float maxHorizontalVelocity = 4f;
    [SerializeField] private float scoreCooldown = 0.2f;
    [SerializeField] private ParticleSystem touchParticles;
    [SerializeField] private AudioClip jumpSound;

    [SerializeField] private Camera mainCamera;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private float lastScoreTime;
    private bool gameStarted = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!mainCamera) mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        ResetToStartPosition();
    }

    public void ResetToStartPosition()
    {
        transform.position = manualStartPosition;
        transform.rotation = Quaternion.identity;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        gameStarted = false;
        lastScoreTime = -scoreCooldown;
        if (startPanel) startPanel.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine(GameStartRoutine());
    }

    private IEnumerator GameStartRoutine()
    {
        if (startPanel) startPanel.SetActive(false);
        yield return new WaitForSecondsRealtime(startDelay);
        gameStarted = true;
        rb.gravityScale = gravityStrength;
        rb.linearVelocity = Vector2.down * initialSpeed;
    }

    void Update()
    {
        HandleInput();
        ClampHorizontalVelocity();
    }

    private void HandleInput()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(GetInputPosition());
            if (GetComponent<Collider2D>().OverlapPoint(worldPos))
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
        if (Time.time > lastScoreTime + scoreCooldown)
        {
            ScoreSystem.Instance?.AddScore(1);
            lastScoreTime = Time.time;
        }
        ApplyJumpForce();
        PlayTouchEffects();
    }

    private void ApplyJumpForce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), 0), ForceMode2D.Impulse);
    }

    private void PlayTouchEffects()
    {
        if (touchParticles) Instantiate(touchParticles, transform.position, Quaternion.identity);
        if (jumpSound && audioSource) audioSource.PlayOneShot(jumpSound);
    }

    private void ClampHorizontalVelocity()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalVelocity, maxHorizontalVelocity);
        rb.linearVelocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            LivesSystem.Instance?.LoseLife();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(manualStartPosition, 0.25f);
        Gizmos.DrawWireCube(manualStartPosition, Vector3.one * 0.5f);
        UnityEditor.Handles.Label(manualStartPosition + Vector3.up * 0.5f, "Ball Start Pos");
    }
#endif
}
