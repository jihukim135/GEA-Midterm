using System;
using UnityEngine;

public class ItemsInfo : MonoBehaviour
{
    #region Singleton

    private static ItemsInfo _instance;

    public static ItemsInfo Instance
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
            GameObject go = GameObject.FindWithTag("ItemsInfo");
            if (go == null)
            {
                Debug.LogError("ItemsInfo not found");
                return;
            }

            _instance = go.GetComponent<ItemsInfo>();
        }
    }

    #endregion
    
    public int ItemsCount => 2;
    public int PossibilityFactor => 5;

    [SerializeField] private GameObject[] descriptions;
    public GameObject[] Descriptions => descriptions;
}