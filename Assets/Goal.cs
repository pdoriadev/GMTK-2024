using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int goal = 5;
    public TextMeshProUGUI text;
    public float width = 2.0f;
    public float height = 5.0f;

    public int count = 0;

    void Start()
    {
        
    }

    void Update()
    {
        text.text = ((int)Time.time).ToString();
        var bottomLeft = new Vector2(transform.position.x - width/2f, transform.position.y);
        var topRight = new Vector2(transform.position.x + width/2f, transform.position.y + height);
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.SetLayerMask(LayerMask.GetMask("Ooble"));
        List<Collider2D> results = new List<Collider2D>();
        count = Physics2D.OverlapArea(bottomLeft, topRight, filter2D, results);
        text.text = Mathf.Clamp(goal - count, 0, goal).ToString();
    }
}
