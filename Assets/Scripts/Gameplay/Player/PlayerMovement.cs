using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MovementBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float _speedWalking = 5.0f;
    [SerializeField] private float _speedRunning = 9.0f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private Rigidbody _rigidBody;
    
    private readonly RaycastHit[] _groundHits = new RaycastHit[8];
    
    private PlayerBehaviour _playerCharacter;
    private Weapon _equippedWeapon;

    private bool isGrounded;

    public void Init(PlayerBehaviour playerBehaviour)
    {
        _playerCharacter = playerBehaviour;
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    protected override void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.72f, _groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        if(isGrounded)
        {
            MoveCharacter();
        }
    }

    private void Jump()
    {
        _rigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    protected override void Update()
    {
        _equippedWeapon = _playerCharacter.GetInventory().EquippedWeapon;
    }

    private void MoveCharacter()
    {
        Vector2 frameInput = _playerCharacter.GetInputMovement();
        var movement = new Vector3(frameInput.x, 0.0f, frameInput.y);
            
        if(_playerCharacter.IsRunning())
            movement *= _speedRunning;
        else
        {
            movement *= _speedWalking;
        }

        movement = transform.TransformDirection(movement);

        _rigidBody.velocity = new Vector3(movement.x, movement.y, movement.z);
    }
}