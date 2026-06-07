using UnityEngine;

public class Player1Attack : MonoBehaviour
{
    public bool hasHit;

    private void OnEnable()
    {
        hasHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryHit(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryHit(collision);
    }

    private void TryHit(Collider2D collision)
    {
        if (hasHit)
            return;

        if (collision.CompareTag("P2"))
        {
            hasHit = true;
            Player2Controls p2controls = collision.GetComponent<Player2Controls>();
            Player1Controls p1controls = GetComponentInParent<Player1Controls>();
            p2controls?.ReceiveHitFromPlayer1(p1controls);
        }
    }
}
