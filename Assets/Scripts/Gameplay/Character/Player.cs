using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Gameplay.Character
{
    public class Player : BaseCharacter
    {
        [SerializeField] private InputActionReference moveInput, look, attack, strongAttack;
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private AttackController rangeAttack;
        [SerializeField] private AttackController protection;
        private int _blockInputCount = 0;
        private Plane _plane;
        private Camera _camera;


        private void Awake()
        {
            attack.action.performed += Attack;
            strongAttack.action.performed += Protection;
        
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
            characterMovement.MovementOnDirection(GetDirectionMovement(), navMeshAgent);
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
            if(_blockInputCount != 0) return;
            Debug.Log("Attack");
            SetWeaponPrefab(projectilePrefabs);
            rangeAttack.PerformAttack();
        }
    
        private void Protection(InputAction.CallbackContext obj)
        {
            if(_blockInputCount != 0) return;
            Debug.Log("Protection");
            SetWeaponPrefab(protectionsPrefab);
            protection.PerformProtection();
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
}
