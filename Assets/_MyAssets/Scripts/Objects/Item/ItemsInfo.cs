using System;
using System.Collections.Generic;
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

    public int ItemsCount => orderedItemClassNames.Count;
    
    // 아이템이 플랫폼 몇 개에 한 번 꼴로 나타나는지를 결정
    public int PossibilityFactor => 5;

    private readonly List<string> orderedItemClassNames = new List<string>
    {
        // 클래스명과 일치해야 함
        "InvincibleItem",
        "InfiniteJumpItem"
    };

    [SerializeField] private GameObject[] descriptions;
    public GameObject[] Descriptions => descriptions;

    public int GetItemClassIndex(string className)
    {
        for (int i = 0; i < orderedItemClassNames.Count; i++)
        {
            if (orderedItemClassNames[i] == className)
            {
                return i;
            }
        }

        return -1;
    }
}