using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            Init();
            return _instance;
        }
    }

    private static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.FindWithTag("GameManager");
            if (go == null)
            {
                Debug.LogError("GameManager not found");
                return;
            }

            _instance = go.GetComponent<GameManager>();
        }
    }

    #endregion

    public bool IsGameOver { get; set; } = false;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private GameObject gameOverUI;

    private int score = 0;

    private void Update()
    {
        if (IsGameOver && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int newScore)
    {
        if (IsGameOver)
        {
            return;
        }

        score += newScore;
        scoreText.text = $"Score : {score}";
    }

    public void OnPlayerDead()
    {
        IsGameOver = true;
        gameOverUI.SetActive(true);

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        highScoreText.gameObject.SetActive(true);
        highScoreText.text = $"HIGH SCORE: {PlayerPrefs.GetInt("HighScore", 0)}";
        
        var itemDescriptions = ItemsInfo.Instance.Descriptions;
        foreach (var d in itemDescriptions)
        {
            d.SetActive(false);
        }
    }
}