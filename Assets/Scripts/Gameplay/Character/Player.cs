using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Character
{
    public class Player : BaseCharacter
    {
        public static Player Instance;
        public bool blockProtection;
        public bool blockAttack;
        
        [SerializeField] private InputActionReference moveInput, look, attack, strongAttack, pauseInput;
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private AttackController rangeAttack;
        [SerializeField] private AttackController protection;
        [SerializeField] private Animator playerAnim;
        [SerializeField] public float timeInvulnerability;
        private CinemachineVirtualCamera _virtualCamera;

        private int _blockInputCount = 0;
        private bool _isTakeDamage;
        private bool _isAnim = true;
        
        private Plane _plane;
        private Camera _camera;
        [HideInInspector] public bool isVampireAbility = false;
        [HideInInspector] public int increaseDmg;
        [HideInInspector] public float increaseSpeedProjectile;
        [HideInInspector] public float increaseLifeTime;
        
        private void Awake()
        {
            if(Instance != null)
                Destroy(Player.Instance.gameObject);
            Instance = this;
            attack.action.performed += Attack;
            strongAttack.action.performed += Protection;
            pauseInput.action.performed += OnPause;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _plane = new Plane(Vector3.up, Vector3.zero);
            _camera = Camera.main;
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _virtualCamera.Follow = transform;
        }

        private void OnPause(InputAction.CallbackContext obj)
        {
            EventGameManager.Instance.OnPause?.Invoke();
        }

        private void OnEnable()
        {
            look.action.Enable();
            moveInput.action.Enable();
            attack.action.Enable();
            strongAttack.action.Enable();
            pauseInput.action.Enable();
        }

        private void OnDisable()
        {
            look.action.Disable();
            moveInput.action.Disable();
            attack.action.Disable();
            strongAttack.action.Disable();
            pauseInput.action.Disable();
        }
        

        private void Update()
        {
            if(!_isAnim) return;
            playerAnim.SetInteger("Speed", (int)navMeshAgent.velocity.magnitude);
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
            _plane = new Plane(Vector3.up, transform.position);
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
            playerAnim.SetBool("Attack", true);
            SetWeaponPrefab(projectilePrefabs);

            IncreaseAttack();

            rangeAttack.PerformAttack();

            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, 0.3f).OnComplete(() =>
            {
                playerAnim.SetBool("Attack", false);
            });

            DOTween.To(() => inVal, x => inVal = x, 1, AttackCooldown).OnComplete(() =>
            {
                blockAttack = false;
            });
        }

        void IncreaseAttack()
        {
            projectilePrefabs.First().dmg += increaseDmg;
            projectilePrefabs.First().speed += increaseSpeedProjectile;
            increaseDmg = 0;
            increaseSpeedProjectile = 0;
        }
        void IncreaseProtection()
        {
            protectionsPrefab.First().lifeTime += increaseLifeTime;
            increaseLifeTime = 0;
        }
    
        private void Protection(InputAction.CallbackContext obj)
        {
            if(_blockInputCount != 0) return;
            if(blockProtection) return;
            blockProtection = true;
            SetWeaponPrefab(protectionsPrefab);
            protection.PerformProtection();
            
            IncreaseProtection();
            
            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, ProtectionCooldown).OnComplete(() =>
            {
                blockProtection = false;
            });
        }
        
        public override void TakeDamage(int amount, DamageType type, Weapon.Weapon source)
        {
            if(_isTakeDamage)
                return;
            if (immunityList.Any(immunity => type.ToString() == immunity.ToString()))
                return;
            _isTakeDamage = true;
            Hp -= amount;
            EventGameManager.Instance.OnPlayerHpChange?.Invoke();
            if (Hp <= 0)
            {
                KillPlayer();
            }
            else
            {
                ReturnNormalState();
            }
        }

        public void GetHp(int count)
        {
            Hp += count;
            EventGameManager.Instance.OnPlayerHpChange?.Invoke();
        }

        public void VampireHeal(int count)
        {
            if (isVampireAbility)
            {
                GetHp((int)count/10);
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
            EventGameManager.Instance.OnPlayerDead?.Invoke();
            _isAnim = false;
            playerAnim.SetTrigger("Dead");
            BlockInput();
            GetComponent<Collider>().enabled = false;
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
            DOTween.To(() => inVal, x => inVal = x, 1, timeInvulnerability).OnComplete(() =>
            {
                UnBlockInput();
                isKnockBack = false; 
                _isTakeDamage = false; 
                rb.isKinematic = true;
            });
        }
    }
}
