using UnityEngine;

public class PlatformSpawner : MonoBehaviour 
{
    [SerializeField] private GameObject platformPrefab; 
    [SerializeField] private int count = 3;

    [SerializeField] private float timeBetSpawnMin = 1.5f; 
    [SerializeField] private float timeBetSpawnMax = 2.5f; 
    [SerializeField] private float timeBetSpawn; 

    [SerializeField] private float yMin = -3.5f; 
    [SerializeField] private float yMax = 1.5f; 
    private float _xPos = 20f; 

    private GameObject[] _platforms; 
    private Platform[] _platformsComponent;
    private int _currentIndex = 0; 

    private readonly Vector2 poolPosition = new Vector2(0, -25); 
    private float _lastSpawnTime; 


    void Start()
    {
        _platforms = new GameObject[count];
        _platformsComponent = new Platform[count];

        for (int i = 0; i < count; i++)
        {
            _platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
            _platformsComponent[i]= _platforms[i].GetComponent<Platform>();
        }

        _lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    void Update() 
    {
        if (GameManager.Instance.IsGameOver)
        {
            return;
        }

        if (Time.time >= _lastSpawnTime + timeBetSpawn)
        {
            _lastSpawnTime = Time.time;

            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            float yPos = Random.Range(yMin, yMax);

            _platformsComponent[_currentIndex].enabled = false;
            _platformsComponent[_currentIndex].enabled = true;

            _platforms[_currentIndex].transform.position = new Vector2(_xPos, yPos);
            _currentIndex++;

            if (_currentIndex >= count)
            {
                _currentIndex = 0;
            }
        }
    }
}