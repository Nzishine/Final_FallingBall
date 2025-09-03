using UnityEngine;

public class BallManager : MonoBehaviour
{
    [Header("Ball Sprites")]
    public Sprite basketBallSprite;
    public Sprite footballSprite;
    public Sprite baseBallSprite;
    public Sprite bowlingSprite;
    public Sprite babyBallSprite;
    public Sprite volleyballSprite;

    private SpriteRenderer currentBallRenderer;

    void Start()
    {
        currentBallRenderer = GetComponent<SpriteRenderer>();
        if (currentBallRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Ball GameObject!");
            return;
        }

        LoadSelectedBall();
    }

    void LoadSelectedBall()
    {
        string selectedBall = PlayerPrefs.GetString("SelectedBall", "BasketBall");

        switch (selectedBall)
        {
            case "Football":
                currentBallRenderer.sprite = footballSprite;
                break;

            case "BaseBall":
                currentBallRenderer.sprite = baseBallSprite;
                break;

            case "Bowling":
                currentBallRenderer.sprite = bowlingSprite;
                break;

            case "BabyBall":
                currentBallRenderer.sprite = babyBallSprite;
                break;

            case "Volleyball":
                currentBallRenderer.sprite = volleyballSprite;
                break;

            default:
                currentBallRenderer.sprite = basketBallSprite;
                break;
        }
    }
}
