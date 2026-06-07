using UnityEngine;

public class Player2Attack : MonoBehaviour
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

        if (collision.CompareTag("P1"))
        {
            hasHit = true;
            Player1Controls p1controls = collision.GetComponent<Player1Controls>();
            Player2Controls p2controls = GetComponentInParent<Player2Controls>();
            p1controls?.ReceiveHitFromPlayer2(p2controls);
        }
    }
}
