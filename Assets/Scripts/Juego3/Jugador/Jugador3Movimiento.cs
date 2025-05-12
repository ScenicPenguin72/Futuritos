using UnityEngine;
using System.Collections;

public class Jugador3Movimiento : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool inputsEnabled = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!inputsEnabled || Time.timeScale == 0)
        {
            movementInput = Vector2.zero;

            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Side", false);

            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) > 0)
        {
            movementInput = new Vector2(horizontal, 0).normalized;

            spriteRenderer.flipX = horizontal < 0;

            animator.SetBool("Side", true);
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
        }
        else if (Mathf.Abs(vertical) > 0)
        {
            movementInput = new Vector2(0, vertical).normalized;

            if (vertical > 0)
            {
                animator.SetBool("Up", true);
                animator.SetBool("Down", false);
            }
            else
            {
                animator.SetBool("Up", false);
                animator.SetBool("Down", true);
            }
            animator.SetBool("Side", false);

        }
        else
        {
            movementInput = Vector2.zero;
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Side", false);
        }

        if (Input.GetKeyDown(KeyCode.Q) && Time.timeScale != 0)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(DisableInputs(0.35f));
        }

    }
    void FixedUpdate()
    {
        rb.velocity = movementInput * moveSpeed;
    }
    IEnumerator DisableInputs(float duration)
    {
        inputsEnabled = false;
        yield return new WaitForSeconds(duration);
        inputsEnabled = true;
    }
}
