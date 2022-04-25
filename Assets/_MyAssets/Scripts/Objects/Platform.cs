using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Platform : MonoBehaviour
{
    public GameObject[] obstacles;
    [SerializeField] private int obstacleRandomSize;

    private ItemsInfo itemsInfo;
    [SerializeField] private SpriteRenderer[] _itemRenderers;

    private void Awake()
    {
        itemsInfo = ItemsInfo.Instance;
    }

    private void OnEnable()
    {
        SetObstacles();
        SetItem();
    }

    private void SetObstacles()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (Random.Range(0, obstacleRandomSize) == 0)
            {
                obstacles[i].SetActive(true);
            }
            else
            {
                obstacles[i].SetActive(false);
            }
        }
    }

    private void SetItem()
    {
        for (int i = 0; i < _itemRenderers.Length; i++)
        {
            _itemRenderers[i].enabled = false;
        }
        
        if (Random.Range(0, itemsInfo.PossibilityFactor) != 0)
        {
            return;
        }

        int index = Random.Range(0, itemsInfo.ItemsCount);
        for (int i = 0; i < _itemRenderers.Length; i++)
        {
            if (i == index)
            {
                _itemRenderers[i].enabled = true;
            }
        }
    }
}