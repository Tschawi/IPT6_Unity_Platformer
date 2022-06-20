using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundlayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    public Vector2 Startposition;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        Startposition = transform.position;
    }

    private void Update()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalinput * speed, body.velocity.y);

        // Player Model umdrehen bei Richtungswechsel
        if (horizontalinput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalinput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //animation parameter setzten
        anim.SetBool("Run", horizontalinput != 0);
        anim.SetBool("Grounded", isGrounded());

        if (Input.GetKey(KeyCode.Space) && isGrounded())
            jump();

        //veränderbare Sprunghöhe
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        body.gravityScale = 7;
        body.velocity = new Vector2(horizontalinput * speed, body.velocity.y);

    }
    private void OnCollisionEnter(Collision collision)
    {
        

    }
    private void jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Downfall")
        {
            transform.position = Startposition;
            Camera.main.GetComponent<CamerController>().MoveToNewRoom(collision.collider.transform.parent);
        }
        
    }
    

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundlayer);
        return raycastHit.collider != null;
    }
}
