using UnityEngine;

public class MovimientoHorizontal : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float positiveXLimit = 4f; // Right limit from startX
    public float negativeXLimit = 2f; // Left limit from startX

    private Rigidbody2D rb;
    private Vector2 movimiento;
    private float startX;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = transform.position.x;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            movimiento = Vector2.zero;
            SetAnimState(0);
            return;
        }

        float entradaHorizontal = Input.GetAxisRaw("Horizontal");
        float currentX = rb.position.x;

        bool atRightLimit = currentX >= startX + positiveXLimit;
        bool atLeftLimit = currentX <= startX - negativeXLimit;

        if (entradaHorizontal > 0 && !atRightLimit)
        {
            SetAnimState(1); // Moving right
         
        }
        else if (entradaHorizontal < 0 && !atLeftLimit)
        {
            SetAnimState(2); // Moving left
          
        }
        else
        {
            SetAnimState(0); // Idle
        }

        movimiento = new Vector2(entradaHorizontal, 0).normalized;
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        float newX = rb.position.x + movimiento.x * velocidadMovimiento * Time.fixedDeltaTime;
        float clampedX = Mathf.Clamp(newX, startX - negativeXLimit, startX + positiveXLimit);
        rb.MovePosition(new Vector2(clampedX, rb.position.y));
    }

    void SetAnimState(int estado)
    {
        if (animator != null)
        {
            animator.SetInteger("Estado", estado);
        }
    }
}
