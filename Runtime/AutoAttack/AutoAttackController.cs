using Elysium.Utils;
using Elysium.Utils.Attributes;
using Elysium.Utils.Timers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public class AutoAttackController : MonoBehaviour, IDamageDealer
    {
        public RefValue<int> Damage { get; set; }
        public RefValue<float> BaseAttackSpeed { get; set; }
        public RefValue<float> BonusAttackSpeed { get; set; }

        [Header("Parameters")]
        [HideInInspector] public float AttackRange = 1f;
        [HideInInspector] public List<DamageTeam> OpposingTeams;

        [ReadOnly] public bool CanAttack = false;
        [ReadOnly] public bool IsAttacking = false;

        public IDamageable CombatTarget { get; set; }
        private IDamageable cachedTarget;

        public List<DamageTeam> DealsDamageToTeams => OpposingTeams;
        public GameObject DamageDealerObject
        {
            get
            {
                if (!gameObject || !transform.parent) { return null; }
                return transform.parent.gameObject;
            }
        }

        private TimerInstance attackTimer;
        private IAnimationEvents AnimationController;

        // ASPD Calculation
        public float AttackSpeed => BaseAttackSpeed.Value * BonusAttackSpeed.Value;
        private float attackInterval => 1 / AttackSpeed;

        // EVENTS
        public event Action<IDamageable> OnAttack;
        public event Action<IDamageable> OnTargetDied;

        public event Action OnAttackStart;
        public event Action OnAttackHit;
        public event Action OnAttackEnd;

        private void Start() => CheckForExistingTimer();

        public bool TryAttack()
        {
            if (CombatTarget == null) { /*Debug.Log("CAN'T AUTO ATTACK. TARGET IS NULL!");*/ return false; }
            if (CombatTarget.IsDead) { /*Debug.Log("CAN'T AUTO ATTACK. TARGET IS DEAD!");*/ OnTargetDied?.Invoke(CombatTarget); return false; }
            if (!InTargetAttackRange(CombatTarget.DamageableObject.transform)) { /*Debug.Log("CAN'T AUTO ATTACK. NOT WITHIN TARGET RANGE!");*/ return false; }
            if (!CanAttack) { /*Debug.Log("CAN'T AUTO ATTACK. ATTACK IS ON COOLDOWN!");*/ return false; }
            if (IsAttacking) { /*Debug.Log("CAN'T AUTO ATTACK. ALREADY ATTACKING!");*/ return false; }

            cachedTarget = CombatTarget;
            OnAttackStart?.Invoke();
            AnimationController.SetAttackSpeed(AttackSpeed);
            AnimationController.StartAnimation(Animation.Attack);
            IsAttacking = true;
            AnimationController.OnAttackHit += ExecuteAttack;
            AnimationController.OnAttackEnd += EndAttack;
            transform.LookAt(cachedTarget.DamageableObject.transform);
            return true;
        }

        public bool InTargetAttackRange(Transform tTarget) => Vector3.Distance(transform.position, tTarget.position) <= AttackRange;

        public void ExecuteAttack()
        {
            OnAttackHit?.Invoke();
            SetAttackDelay();
            if (cachedTarget != null) { cachedTarget.TakeDamage(this, Damage.Value); }
            OnAttack?.Invoke(cachedTarget);
            cachedTarget = null;
            AnimationController.OnAttackHit -= ExecuteAttack;
        }

        public void EndAttack()
        {
            OnAttackEnd?.Invoke();
            IsAttacking = false;
            AnimationController.OnAttackEnd -= EndAttack;
        }

        private void CheckForExistingTimer()
        {
            if (attackTimer != null) { return; }

            attackTimer = Timer.CreateTimer(0.1f, () => !this, false);
            attackTimer.OnTimerEnd += ResetAttackDelay;
        }

        private void SetAttackDelay()
        {
            CanAttack = false;
            attackTimer.AddTime(attackInterval);
        }
        private void ResetAttackDelay() => CanAttack = true;

        public void ResetToDefaultValues()
        {
            // CombatTarget = null;
            IsAttacking = false;
            CanAttack = true;
            cachedTarget = null;
        }

        private void FixedUpdate()
        {
            Vector3? _targetPos = null;

            if (cachedTarget != null)
            {
                _targetPos = cachedTarget.DamageableObject.transform.position;
            }

            if (!_targetPos.HasValue) { return; }

            float speed = 30f;
            Vector3 direction = _targetPos.Value - DamageDealerObject.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, DamageDealerObject.transform.up);
            DamageDealerObject.transform.rotation = Quaternion.Lerp(DamageDealerObject.transform.rotation, toRotation, speed * Time.deltaTime);
        }

        #region GIZMOS
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (!transform.parent) { return; }
            Gizmos.DrawWireSphere(transform.parent.position, AttackRange);
        }
        #endregion
    }
}