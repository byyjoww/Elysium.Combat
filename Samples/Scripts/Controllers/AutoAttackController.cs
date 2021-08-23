using Elysium.Utils;
using Elysium.Utils.Attributes;
using Elysium.Utils.Timers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public class AutoAttackController : MonoBehaviour, IDamageDealer
    {
        public RefValue<int> Damage { get; set; }
        public RefValue<float> BaseAttackSpeed { get; set; }
        public RefValue<float> BonusAttackSpeed { get; set; }

        [Header("Parameters")]
        [HideInInspector] public float AttackRange = 1f;
        [HideInInspector] public string attackStateName = "Attack";
        [HideInInspector] public DamageTeam[] OpposingTeams;

        [ReadOnly] public bool CanAttack = false;
        [ReadOnly] public bool IsAttacking = false;

        public Tuple<Transform, IDamageable> Target { get; set; }
        private Tuple<Transform, IDamageable> CachedTarget { get; set; }

        public DamageTeam[] DealsDamageToTeams => OpposingTeams;
        protected virtual NullElement element => new NullElement();
        public GameObject DamageDealerObject
        {
            get
            {
                if (!gameObject || !transform.parent) { return null; }
                return transform.parent.gameObject;
            }
        }

        private TimerInstance attackTimer;
        // private IModelController modelController;

        // ASPD Calculation
        public float AttackSpeed => BaseAttackSpeed.Value * BonusAttackSpeed.Value;
        private float attackInterval => 1 / AttackSpeed;

        // EVENTS
        public event UnityAction<IDamageable> OnAttack;
        public event UnityAction<IDamageable> OnTargetDied;
        public event UnityAction OnAttackStart;
        public event UnityAction OnAttackHit;
        public event UnityAction OnAttackEnd;
        public event UnityAction OnCriticalHit;

        private void Start() => CheckForExistingTimer();

        public bool TryAttack()
        {
            if (Target.Item1 == null) { /*Debug.Log("CAN'T AUTO ATTACK. TARGET IS NULL!");*/ return false; }
            if (Target.Item2.IsDead) { /*Debug.Log("CAN'T AUTO ATTACK. TARGET IS DEAD!");*/ OnTargetDied?.Invoke(Target.Item2); return false; }
            if (!InTargetAttackRange(Target.Item1)) { /*Debug.Log("CAN'T AUTO ATTACK. NOT WITHIN TARGET RANGE!");*/ return false; }
            if (!CanAttack) { /*Debug.Log("CAN'T AUTO ATTACK. ATTACK IS ON COOLDOWN!");*/ return false; }
            if (IsAttacking) { /*Debug.Log("CAN'T AUTO ATTACK. ALREADY ATTACKING!");*/ return false; }

            CachedTarget = Target;
            OnAttackStart?.Invoke();
            // modelController.Animator.SetFloat("aspd", AttackSpeed);
            // modelController.PlayAnimation(attackStateName);
            IsAttacking = true;
            // modelController.OnAnimationHit += CheckForHit;
            // modelController.OnAnimationEnd += CheckForEnd;
            transform.LookAt(CachedTarget.Item1);
            return true;
        }

        public bool InTargetAttackRange(Transform tTarget) => Vector3.Distance(transform.position, tTarget.position) <= AttackRange;

        public void ExecuteAttack()
        {
            OnAttackHit?.Invoke();
            SetAttackDelay();

            if (CachedTarget != null)
            {
                CombatCalculator.DealDamage(this, new NullElement(), CachedTarget.Item2, Damage.Value, null, null);
            }

            OnAttack?.Invoke(CachedTarget.Item2);
            CachedTarget = null;
            // modelController.OnAnimationHit -= CheckForHit;
        }

        public void EndAttack()
        {
            OnAttackEnd?.Invoke();
            IsAttacking = false;
            // modelController.OnAnimationEnd -= CheckForEnd;
        }

        public void OnCriticalHitCallback()
        {
            OnCriticalHit?.Invoke();
        }

        private void CheckForHit(string _animation) 
        {
            if (_animation != attackStateName) { return; }
            ExecuteAttack(); 
        }

        private void CheckForEnd(string _animation) 
        {
            if (_animation != attackStateName) { return; }
            EndAttack(); 
        }

        private void CheckForExistingTimer()
        {
            if (attackTimer != null) { return; }

            attackTimer = Timer.CreateTimer(0.1f, () => !this, false);
            attackTimer.OnEnd += ResetAttackDelay;
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
            CachedTarget = null;
        }

        private void FixedUpdate()
        {
            Vector3? _targetPos = null;

            if (CachedTarget != null)
            {
                _targetPos = CachedTarget.Item1.position;
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