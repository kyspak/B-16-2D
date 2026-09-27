using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerInput _playerInput;
    
    private InputAction _moveAction;
    private InputAction _fireAction;
    private InputAction _jumpAction;
    
    public static Vector2 MoveDirection { get; private set; }

    public static event Action OnFire;
    public static event Action OnJump;
    

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindActionMap("Player")["Move"];
        _fireAction = _playerInput.actions.FindActionMap("Player")["Fire"];
        _jumpAction = _playerInput.actions.FindActionMap("Player")["Jump"];
    }

    private void OnEnable()
    {
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;
        //_fireAction.performed += _ => onFire?.Invoke();
        _fireAction.performed += OnFirehandle;
        _jumpAction.performed += _ => OnJump?.Invoke();
    }

    private void OnFirehandle(InputAction.CallbackContext obj) => OnFire?.Invoke();

    private void OnMovePerformed(InputAction.CallbackContext ctx) => MoveDirection = ctx.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveDirection = Vector2.zero;
    
    

    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
        _fireAction.performed -= OnFirehandle;
    }
    
    
}
