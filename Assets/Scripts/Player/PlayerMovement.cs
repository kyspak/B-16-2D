using System.Threading;
using Combat;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
   [Header("Movement")]
   [SerializeField] private float _speed = 5f;
   
   [Header("Jump")]
   [SerializeField] private float _jumpForce = 5f;
   [SerializeField] private int _jumpCount = 2;
   
   [Header("Ground Check")]
   [SerializeField] private Transform _groundCheck;
   [SerializeField] private float _groundCheckRadius = 0.2f;
   [SerializeField] private LayerMask _groundMask;
   
   private Rigidbody2D _rb;
   private Weapon _weapon;
   
   private Vector2 _movement;
   
   private bool _jumpRequested;
   private int _jumpRemaining;
   private bool _isGrounded;

   private Animator _animator;
   

   private void Awake()
   {
      _rb = GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();
      _weapon = GetComponent<Weapon>();
      _jumpRemaining = _jumpCount;
   }

   private void OnEnable()
   {
      InputReader.OnFire += HandleFire;
      InputReader.OnJump += Jump;
   }

   private void OnDisable()
   {
      InputReader.OnFire -= HandleFire;
      InputReader.OnJump -= Jump;
   }

   private void Move()
   {
      Vector2 direction = InputReader.MoveDirection;
      if(direction.magnitude > 1)
         direction.Normalize();
      
      Vector2 velocity = _rb.linearVelocity;
      velocity.x = direction.x * _speed;
      _rb.linearVelocity = velocity;

      if (direction.x != 0)
      {
         Vector3 scale = transform.localScale;
         scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
         transform.localScale = scale;
      }
      
      _animator.SetFloat("Movement", Mathf.Abs(velocity.x));
      
      
     // _rb.linearVelocity = new Vector2(direction.x * _speed, _rb.linearVelocity.y);
   }

   private void Jump()
   {

      if (_jumpRemaining <= 0)
      {
         return;
      }
      
      Vector2 velocity = _rb.linearVelocity;
      velocity.y = _jumpForce;
      _rb.linearVelocity = velocity;
      _jumpRemaining--;
      if (_jumpRemaining < 1 && _jumpRemaining >= 0)
      {
         _animator.SetTrigger("DoubleJump");
      }
   }

   private void FixedUpdate()
   {
      Move();
      UpdateGrounded();
      if (IsGrounded())
      {
         _animator.SetBool("IsGrounded", true);
      }
      else
      {
         _animator.SetBool("IsGrounded", false);
      }
   }


   private void UpdateGrounded()
   {
      bool isGrounded = IsGrounded();
      if (isGrounded && !_isGrounded)
         _jumpRemaining = _jumpCount;
      
      _isGrounded = isGrounded;
   }

   private bool IsGrounded()
   {
      // return Physics2D.Raycast(
      //    _groundCheck.position,
      //    Vector2.down,
      //    _groundCheckRadius,
      //    _groundMask
      // ) != null;
      return Physics2D.OverlapCircle(
         _groundCheck.position,
         _groundCheckRadius,
         _groundMask
      );
   }

   private void OnDrawGizmosSelected()
   {
      if (!_groundCheck)
         return;
      
      Gizmos.DrawLine(
         _groundCheck.position,
         _groundCheck.position + Vector3.down * _groundCheckRadius
         );
   }


   private void HandleFire()
   {
      _weapon.TryFire();
   }

   // private void OnCollisionEnter2D(Collision2D collision)
   // {
   //    if (collision.gameObject.CompareTag("Ground")) 
   //       _jumpRequested = true;
   // }
}
