using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Gameplay.Character
{
    public class Player : BaseCharacter
    {
        public bool blockProtection;
        public bool blockAttack;
        
        [SerializeField] private InputActionReference moveInput, look, attack, strongAttack;
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private AttackController rangeAttack;
        [SerializeField] private AttackController protection;
        [SerializeField] public float timeInvulnerability;

        private int _blockInputCount = 0;
        private bool _isTakeDamage;
        
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
            if(blockAttack) return;
            blockAttack = true;
            
            Debug.Log("Attack");
            SetWeaponPrefab(projectilePrefabs);
            rangeAttack.PerformAttack();
            
            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, attackCooldown).OnComplete(() =>
            {
                blockAttack = false;
            });
        }
    
        private void Protection(InputAction.CallbackContext obj)
        {
            if(_blockInputCount != 0) return;
            if(blockProtection) return;
            blockProtection = true;
            Debug.Log("Protection");
            SetWeaponPrefab(protectionsPrefab);
            protection.PerformProtection();
            
            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, protectionCooldown).OnComplete(() =>
            {
                blockProtection = false;
            });
        }
        
        public override void TakeDamage(int amount, DamageType type)
        {
            if(_isTakeDamage)
                return;
            if (immunityList.Any(immunity => type.ToString() == immunity.ToString())) 
                return;

            _isTakeDamage = true;
            hp -= amount;
        
            if (hp <= 0)
            {
                KillPlayer();
            }
            else
            {
                ReturnNormalState();
            }
        }
        
        public override void KnockBack(Vector3 dir, float force)
        {
            if(isKnockBack)
                return;
            if(immunityList.Contains(ImmunityType.KnockBack))
                return;
            BlockInput();
            isKnockBack = true;
            rb.isKinematic = false;
            rb.AddForce(dir.normalized * force, ForceMode.Impulse);
            ReturnNormalState();
        }
        
        private void KillPlayer()
        {
            Destroy(this);
            Destroy(navMeshAgent);
        }

        private void BlockInput()
        {
            _blockInputCount++;
        }
    
        private void UnBlockInput()
        {
            if (_blockInputCount <= 0)
            {
                _blockInputCount = 0;
                return;
            }
            _blockInputCount--;
        }
        
        private void ReturnNormalState()
        {
            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, protectionCooldown).OnComplete(() =>
            {
                UnBlockInput();
                isKnockBack = false; 
                _isTakeDamage = false; 
                rb.isKinematic = true;
            });
        }
    }
}
