using UnityEngine;

public class Player2Attack : MonoBehaviour
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

        if (collision.CompareTag("P1"))
        {
            hasHit = true;
            Player1Controls p1controls = collision.GetComponent<Player1Controls>();
            p1controls.Hit();
        }
    }
}
