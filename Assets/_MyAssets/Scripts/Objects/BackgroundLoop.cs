using UnityEngine;

public class BackgroundLoop : MonoBehaviour 
{
    private float _width; 

    private void Awake()
    {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        _width = backgroundCollider.size.x;
    }

    private void Update() 
    {
        if (transform.position.x <= -_width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(_width * 2f, 0);
        var transform1 = transform;
        transform1.position = (Vector2) transform1.position + offset;
    }
}