using UnityEngine;

public class Player1Attack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool hasHit;

    private void OnEnable()
    {
        hasHit = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hasHit) 
        {
            return;
        }
        if (collision.CompareTag("P2"))
        {
            hasHit = true;
            Player2Controls p2controls = collision.GetComponent<Player2Controls>();
            p2controls.Hit();
        }
    }
}
