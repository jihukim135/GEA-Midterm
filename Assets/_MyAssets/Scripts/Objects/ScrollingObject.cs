using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField] private float speed = 7f; 

    private void Update()
    {
        if (!GameManager.Instance.IsGameOver)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}