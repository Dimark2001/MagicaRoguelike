using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private InputActionReference moveInput, look, attack, strongAttack;
    [SerializeField] private CharacterMovement characterMovement;

    private int _blockInputCount = 0;
    private Plane _plane;
    private Camera _camera;


    private void Awake()
    {
        attack.action.performed += Attack;
        strongAttack.action.performed += StrongAttack;
        
        _plane = new Plane(Vector3.up, Vector3.zero);
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        look.action.Enable();
        moveInput.action.Enable();
        attack.action.Enable();
        strongAttack.action.Enable();
    }

    private void OnDisable()
    {
        look.action.Disable();
        moveInput.action.Disable();
        attack.action.Disable();
        strongAttack.action.Disable();
    }

    private void Update()
    {
        if(_blockInputCount != 0) return;
        
        RotatePlayer(GetMouseAngle());
        Move();
    }

    private void Move()
    {
        characterMovement.MovementOnDirection(GetDirectionMovement(), agent);
    }
    
    private void RotatePlayer(Vector3 angle)
    {
        transform.LookAt(angle);
    }
    
    private Vector3 GetDirectionMovement()
    {
        var xRaw = moveInput.action.ReadValue<Vector2>().x;
        var zRaw = moveInput.action.ReadValue<Vector2>().y;
        var moveDir = new Vector3(xRaw, 0, zRaw);
        return moveDir;
    }
    
    public Vector3 GetMouseAngle()
    {
        var rayCam = _camera.ScreenPointToRay(new Vector3(look.action.ReadValue<Vector2>().x, look.action.ReadValue<Vector2>().y , 0));

        if (_plane.Raycast(rayCam, out var enter))
        {
            var hitPoint = rayCam.GetPoint(enter);
            var hitPointWithCharY = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
            return hitPointWithCharY;
        }
        return Vector3.zero;
    }
    
    private void Attack(InputAction.CallbackContext obj)
    {
        Debug.Log("Attack");
    }
    
    private void StrongAttack(InputAction.CallbackContext obj)
    {
        Debug.Log("StrongAttack");
    }

    private void BlockInput()
    {
        _blockInputCount++;
    }
    
    private void UnBlockInput()
    {
        _blockInputCount--;
    }
}
