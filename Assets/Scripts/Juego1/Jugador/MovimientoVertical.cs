using UnityEngine;

public class MovimientoVertical : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float positiveYLimit = 4f; // Upwards limit from startY
    public float negativeYLimit = 2f; // Downwards limit from startY

    private Rigidbody2D rb;
    private Vector2 movimiento;
    private float startY;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startY = transform.position.y;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Skip animator updates while paused
        if (Time.timeScale == 0f)
        {
            movimiento = Vector2.zero;
            return;
        }

        float entradaVertical = Input.GetAxisRaw("Vertical");
        float currentY = rb.position.y;

        // Determine if at limits
        bool atUpperLimit = currentY >= startY + positiveYLimit;
        bool atLowerLimit = currentY <= startY - negativeYLimit;

        // Only set animation if not stuck at limit
        if (entradaVertical > 0 && !atUpperLimit)
            SetAnimState(1);
        else if (entradaVertical < 0 && !atLowerLimit)
            SetAnimState(2);
        else
            SetAnimState(0);

        movimiento = new Vector2(0, entradaVertical).normalized;
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return; // Don't move if paused

        float newY = rb.position.y + movimiento.y * velocidadMovimiento * Time.fixedDeltaTime;
        float clampedY = Mathf.Clamp(newY, startY - negativeYLimit, startY + positiveYLimit);
        rb.MovePosition(new Vector2(rb.position.x, clampedY));
    }

    void SetAnimState(int estado)
    {
        if (animator != null)
        {
            animator.SetInteger("Estado", estado);
        }
    }
}
