using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    private float m_JumpForce = 400f;                           // Amount of force added when the player jumps.
    [Range(0, .3f)]
    [SerializeField]
    private float m_MovementSmoothing = .05f;   // How much to smooth out the movement
    [SerializeField]
    private bool m_AirControl = false;                          // Whether or not a player can steer while jumping;
    [SerializeField]
    private LayerMask m_WhatIsGround;                           // A mask determining what is ground to the character
    [SerializeField]
    private Transform m_GroundCheck;                            // A position marking where to check if the player is grounded.
    [SerializeField]
    private Transform m_CeilingCheck;                           // A position marking where to check for ceilings
    [SerializeField]
    private Collider2D m_CrouchDisableCollider;             // A collider that will be disabled when crouching

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D rb2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    private bool isDashing = false;
    private bool isDashCooldown = false;
    [SerializeField]
    private float dashCoolDown = 0.01f;
    [SerializeField]
    private float dashTimer = 1; // Amount of time the dash lasts
    [SerializeField]
    private float dashSpeedMultipler = 2;

    private Animator anim;
    [SerializeField]
    private Sprite jumpSprite;
    private SpriteRenderer sR;
    
    // Dashing
    private bool isDashingCooldown;
    private float dashCooldown;

    // Shooting
    private bool isShooting = false;
    private bool isShootingCooldown = false;
    [SerializeField]
    private float shootingCooldown = 0.3f;
    [SerializeField]
    private GameObject bullet;

    // Health
    int HP = 1;
    [SerializeField]
    private Sprite damagedSprite;
    [SerializeField]
    private float invicibilityTimer = 0.2f;
    private bool isInvicible;

    // Audio
    private AudioSource aS;
    [SerializeField]
    private AudioClip jumpClip;
    [SerializeField]
    private AudioClip dashClip;
    [SerializeField]
    private AudioClip shootClip;
    

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        aS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }


    private void Update()
    {
        if (isInvicible) return;
        Shoot();
    }


    private void FixedUpdate()
    {
        if (isInvicible) return;

        if (isDashing)
        {
            anim.enabled = true;
            return;
        }

        if (isShooting)
        {
            anim.enabled = true;
            return;
        }

        if (m_GroundCheck)
        {
            anim.enabled = false;
            sR.sprite = jumpSprite;
        }

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (wasGrounded)
                {
                    OnLandEvent.Invoke();
                    anim.enabled = true;
                }
            }
        }
    }


    public void Move(float move, bool dash, bool jump)
    {
        if (dash && !isDashing && !isDashingCooldown)
        {
            isDashing = true;
            isDashingCooldown = true;
            StartCoroutine(Dash());
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            
            Vector3 targetVelocity = isDashing ? new Vector2(move * 10f * dashSpeedMultipler, rb2D.velocity.y) :
                                                 new Vector2(move * 10f, rb2D.velocity.y);
            // And then smoothing it out and applying it to the character
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move == 0) anim.SetBool("run", false);
            else anim.SetBool("run", true);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            rb2D.AddForce(new Vector2(0f, m_JumpForce));
            aS.clip = jumpClip;
            aS.Play();
        }
    }


    private void Shoot()
    {
        if (isShootingCooldown) return;
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isDashing) anim.SetBool("shoot", true);
            StartCoroutine(_Shoot());
            aS.clip = shootClip;
            aS.Play();
            if (m_FacingRight) Instantiate(bullet, transform.position + transform.right, Quaternion.identity);
            else
            {
                GameObject mBullet = Instantiate(bullet, transform.position - 2f * transform.right, Quaternion.identity);
                mBullet.GetComponent<Bullet>().speed *= -1;
                mBullet.GetComponent<SpriteRenderer>().flipX = !mBullet.GetComponent<SpriteRenderer>().flipX;
            }
        }
    }
    

    private IEnumerator _Shoot()
    {
        isShooting = true;
        isShootingCooldown = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        isShooting = false;
        anim.SetBool("shoot", false);
        yield return new WaitForSeconds(shootingCooldown);
        isShootingCooldown = false;
    }


    private IEnumerator Dash()
    {
        anim.SetTrigger("dash");
        aS.clip = dashClip;
        aS.Play();
        yield return new WaitForSeconds(dashTimer);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        isDashingCooldown = false;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    private void TakeDamage()
    {
        if (isInvicible) return;
        rb2D.velocity = Vector2.zero;
        if (--HP < 0) StartCoroutine(Death());
        else StartCoroutine(_TakeDamage());
    }


    private IEnumerator _TakeDamage()
    {
        isInvicible = true;
        anim.enabled = false;
        sR.sprite = damagedSprite;
        sR.color = new Color(1, 0.47f, 0.47f);
        yield return new WaitForSeconds(invicibilityTimer);
        anim.enabled = true;
        isInvicible = false;
    }


    private IEnumerator Death()
    {
        isInvicible = true;
        anim.SetTrigger("dead");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);
        anim.enabled = false;
        sR.enabled = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}