using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemywalk : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer = ~0;
    [Range(0, 1)] [SerializeField] private float floorOffset = 0.15f;
    [Range(0, 1)] [SerializeField] private float wallOffset = 0.15f;
    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;        // How much to smooth out the movement
    public float walkingSpeed = 10f;
    //[Range(0, 300)] [SerializeField] private float knockBackMultiplier = 100f;


    BoxCollider2D boxCollider;
    Rigidbody2D enemyRB;
    private bool isHit = false;
    private bool justFlipped = false;

    Vector3 targetVelocity;                                                         // Generating TargetVelocity Variable 
    private Vector3 Velocity = Vector3.zero;                                        // Generating Zero Velocity Reference

    Color colorRayCast = Color.red;
    Color colorWallRayCast = Color.red;

    SpriteRenderer sprite;
    Vector2 walkingDirection = Vector2.left;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ground
        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down * (boxCollider.bounds.extents.y + floorOffset), colorRayCast);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down * (boxCollider.bounds.extents.y + floorOffset), colorRayCast);
        // Wall
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y * 0.8f, 0), walkingDirection * (boxCollider.bounds.extents.y + 0.15f), colorWallRayCast);
    }

    private void FixedUpdate()
    {
        if (!isHit && OnGround())
        {
            Move();
        }

        OnWall();
        FlipNoGround();

    }
    private bool OnGround() //Check if Player is OnGround
    {
        RaycastHit2D hit0 = Physics2D.Raycast(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down, boxCollider.bounds.extents.y + floorOffset, groundLayer);
        RaycastHit2D hit1 = Physics2D.Raycast(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down, boxCollider.bounds.extents.y + floorOffset, groundLayer);

        if (hit0.collider != null || hit1.collider != null)
        {
            colorRayCast = Color.red;
            return (true);
        }
        else
        {
            colorRayCast = Color.green;
            return (false);
        }
    }

    void FlipNoGround()
    {
        RaycastHit2D hit0 = Physics2D.Raycast(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down, boxCollider.bounds.extents.y + floorOffset, groundLayer);
        RaycastHit2D hit1 = Physics2D.Raycast(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down, boxCollider.bounds.extents.y + floorOffset, groundLayer);

        if (((hit0.collider == null && hit1.collider != null) || (hit0.collider != null && hit1.collider == null)) && !justFlipped)
        {
            StartCoroutine(FlipNoGroundTimer());
        }
    }

    IEnumerator FlipNoGroundTimer()
    {
        justFlipped = true;
        Flip();
        yield return new WaitForSeconds(1f);
        justFlipped = false;
    }

    private bool OnWall() //Check if Player is OnGround
    {
        RaycastHit2D wallRay = Physics2D.Raycast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y * 0.8f, 0), walkingDirection, boxCollider.bounds.extents.y + wallOffset, groundLayer);

        if (wallRay.collider != null)
        {
            colorWallRayCast = Color.red;
            Flip();
            return (true);
        }
        else
        {
            colorWallRayCast = Color.green;
            return (false);
        }
    }

    private void Move()
    {
        targetVelocity = new Vector2(walkingSpeed * walkingDirection.x * Time.fixedDeltaTime * 10f, enemyRB.velocity.y);
        //And then smoothing it out and applying it to the character
        enemyRB.velocity = Vector3.SmoothDamp(enemyRB.velocity, targetVelocity, ref Velocity, MovementSmoothing);
        //If the input is moving the player right and the player is facing left...
    }

    void Flip()
    {
        walkingDirection.x = walkingDirection.x * -1;
        sprite.flipX = !sprite.flipX;
    }

    //public void TakeHit(Collider2D collision, GameObject player)
    //{
    //    if (!isHit)
    //    {
    //        Health health;
    //        player.gameObject.TryGetComponent<Health>(out health);
    //        if (health != null)
    //        {
    //            enemyHealth.Damage(health.damage);
    //        }

    //        Vector2 knockback = (this.transform.position - player.transform.position).normalized;
    //        knockback.y = 1;
    //        StartCoroutine(KnockBack(knockback));
    //        if (new Vector2(knockback.x, 0).normalized == walkingDirection)
    //        {
    //            Flip();
    //        }
    //    }
    //}
    //IEnumerator KnockBack(Vector2 knockBack)
    //{
    //    isHit = true;
    //    enemyRB.velocity = new Vector2(0, 0);
    //    enemyRB.AddForce(knockBack * knockBackMultiplier);
    //    yield return new WaitForSeconds(0.5f);
    //    isHit = false;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Flip();
        }
        //GameObject collisionObject = collision.gameObject;

        //if (collisionObject.tag == "Player" && isHit == false)
        //{
        //    Vector2 knockBack = (this.transform.root.position - collisionObject.transform.root.transform.position);
        //    knockBack.y = 1; //Math.Clamp(knockBack.y, 1, 1);

        //}
    }


}