using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player2Controls : MonoBehaviour
{
    public InputActionReference player2Left;
    public InputActionReference player2Right;
    public InputActionReference player2Sprint;
    public InputActionReference player2Attack;
    public InputActionReference player2Parry;
    public InputActionReference player2Lunge;
    public InputActionReference player2Feint;

    public Vector2 startPos;
    public float moveSpeed = 1.5f;
    public float sprintSpeed = 3f;
    public float lungeSpeed = 8f;
    public float lungeDuration = 0.15f;

    public GameObject p2Hitbox;
    public GameObject p1;

    public AudioSource footstepSource;
    public AudioSource sfxSource;
    public AudioClip walkSound;
    public AudioClip sprintSound;
    public AudioClip attackSound;
    public AudioClip lungeSound;
    public float walkStepInterval = 0.4f;
    public float sprintStepInterval = 0.25f;

    private bool isAttacking;
    private bool isParrying;
    private bool isMobile;
    private SpriteRenderer sr;
    private Animator animator;
    private Coroutine attackCoroutine;
    private Coroutine parryCoroutine;
    private Coroutine footstepCoroutine;
    private bool lastSprinting;

    void Awake()
    {
        if (p2Hitbox != null)
            p2Hitbox.SetActive(false);
    }

    void Start()
    {
        player2Left.action.Enable();
        player2Right.action.Enable();
        player2Sprint.action.Enable();
        player2Attack.action.Enable();
        player2Parry.action.Enable();
        player2Lunge.action.Enable();
        player2Feint.action.Enable();

        isAttacking = false;
        isParrying = false;
        isMobile = true;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool movingLeft = player2Left.action.IsPressed() && isMobile;
        bool movingRight = player2Right.action.IsPressed() && isMobile;
        bool sprinting = player2Sprint.action.IsPressed() && isMobile;

        animator.SetBool("moving_backward", movingRight);
        animator.SetBool("moving_forward", movingLeft);
        animator.SetBool("sprint", sprinting);

        if (movingLeft)
        {
            float speed = sprinting ? sprintSpeed : moveSpeed;
            transform.position = (Vector2)transform.position + Vector2.left * speed * Time.deltaTime;
        }
        if (movingRight)
        {
            float speed = sprinting ? sprintSpeed : moveSpeed;
            transform.position = (Vector2)transform.position + Vector2.right * speed * Time.deltaTime;
        }

        HandleFootsteps(movingLeft || movingRight, sprinting);

        if (player2Attack.action.WasPressedThisFrame() && !isAttacking && isMobile)
        {
            animator.SetTrigger("attack");
            attackCoroutine = StartCoroutine(Attack());
        }
        if (player2Parry.action.WasPressedThisFrame() && !isParrying && isMobile)
        {
            animator.SetTrigger("parry");
            parryCoroutine = StartCoroutine(Parry());
        }
        if (player2Lunge.action.WasPressedThisFrame() && !isAttacking && isMobile)
        {
            animator.SetTrigger("attack");
            attackCoroutine = StartCoroutine(Lunge());
        }
        if (player2Feint.action.WasPressedThisFrame() && !isAttacking && isMobile)
        {
            animator.SetTrigger("attack");
            attackCoroutine = StartCoroutine(Feint());
        }
    }

    void HandleFootsteps(bool moving, bool sprinting)
    {
        if (footstepSource == null) return;

        if (moving)
        {
            if (footstepCoroutine == null || lastSprinting != sprinting)
            {
                if (footstepCoroutine != null) StopCoroutine(footstepCoroutine);
                lastSprinting = sprinting;
                footstepCoroutine = StartCoroutine(FootstepLoop(sprinting));
            }
        }
        else if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
            footstepSource.Stop();
        }
    }

    IEnumerator FootstepLoop(bool sprinting)
    {
        while (true)
        {
            AudioClip clip = sprinting ? sprintSound : walkSound;
            float interval = sprinting ? sprintStepInterval : walkStepInterval;
            float vol = AudioManager.Instance != null ? AudioManager.Instance.SfxVolume : 1f;
            if (clip != null) footstepSource.PlayOneShot(clip, vol);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        isMobile = false;

        float vol = AudioManager.Instance != null ? AudioManager.Instance.SfxVolume : 1f;
        if (sfxSource != null && attackSound != null)
            sfxSource.PlayOneShot(attackSound, vol);

        yield return new WaitForSeconds(0.22f);

        if (p2Hitbox != null)
            p2Hitbox.SetActive(true);

        yield return new WaitForSeconds(0.05f);

        if (p2Hitbox != null)
            p2Hitbox.SetActive(false);

        yield return new WaitForSeconds(0.13f);
        isAttacking = false;
        isMobile = true;
    }

    IEnumerator Lunge()
    {
        isAttacking = true;
        isMobile = false;

        float vol = AudioManager.Instance != null ? AudioManager.Instance.SfxVolume : 1f;
        if (sfxSource != null && lungeSound != null)
            sfxSource.PlayOneShot(lungeSound, vol);

        yield return new WaitForSeconds(0.1f);

        if (p2Hitbox != null)
            p2Hitbox.SetActive(true);

        float elapsed = 0f;
        while (elapsed < lungeDuration)
        {
            transform.position = (Vector2)transform.position + Vector2.left * lungeSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (p2Hitbox != null)
            p2Hitbox.SetActive(false);

        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
        isMobile = true;
    }

    IEnumerator Feint()
    {
        isAttacking = true;
        isMobile = false;

        yield return new WaitForSeconds(0.22f);

        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
        isMobile = true;
    }

    IEnumerator Parry()
    {
        isMobile = false;
        yield return new WaitForSeconds(0.05f);
        isParrying = true;
        sr.color = Color.blue;
        yield return new WaitForSeconds(0.2f);
        isParrying = false;
        sr.color = Color.white;
        yield return new WaitForSeconds(0.15f);
        isMobile = true;
    }

    public void Hit()
    {
        Player1Controls p1controls = p1.GetComponent<Player1Controls>();
        if (isParrying)
        {
            Player1Attack p1attack = p1.GetComponentInChildren<Player1Attack>();
            p1attack.hasHit = false;
            StartCoroutine(p1controls.Stun());
            return;
        }

        UIController.Instance?.AddScore(1);
        Restart();
        p1controls.Restart();
    }

    public void Restart()
    {
        transform.position = startPos;
    }

    public IEnumerator Stun()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        if (parryCoroutine != null)
        {
            StopCoroutine(parryCoroutine);
            parryCoroutine = null;
        }
        if (p2Hitbox != null)
            p2Hitbox.SetActive(false);
        isParrying = false;
        sr.color = Color.red;
        isMobile = false;
        isAttacking = false;
        yield return new WaitForSeconds(0.4f);
        sr.color = Color.white;
        isMobile = true;
    }
}
