using UnityEngine;

public class MovimientoVertical : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float positiveYLimit = 4f; 
    public float negativeYLimit = 2f; 

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
        if (Time.timeScale == 0f)
        {
            movimiento = Vector2.zero;
            return;
        }

        float entradaVertical = Input.GetAxisRaw("Vertical");
        float currentY = rb.position.y;

        bool atUpperLimit = currentY >= startY + positiveYLimit;
        bool atLowerLimit = currentY <= startY - negativeYLimit;

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
        if (Time.timeScale == 0f) return;

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
