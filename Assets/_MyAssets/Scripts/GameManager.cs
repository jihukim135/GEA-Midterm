using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // 싱글톤을 할당할 전역 변수

    public static GameManager Instance
    {
        get
        {
            Init();
            return _instance;
        }
    }

    public bool IsGameOver { get; set; } = false; // 게임 오버 상태
    public Text scoreText; // 점수를 출력할 UI 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트

    private int score = 0; // 게임 점수

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

    void Update()
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
        gameoverUI.SetActive(true);
    }
}