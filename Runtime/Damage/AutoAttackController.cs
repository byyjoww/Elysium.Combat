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
        [HideInInspector] public string attackTrigger = "attack";
        [HideInInspector] public DamageTeam[] OpposingTeams;

        [ReadOnly] public bool CanAttack = false;
        [ReadOnly] public bool IsAttacking = false;

        public IDamageable CombatTarget { get; set; }
        private IDamageable cachedTarget;

        public DamageTeam[] DealsDamageToTeams => OpposingTeams;
        public GameObject DamageDealerObject
        {
            get
            {
                if (!gameObject || !transform.parent) { return null; }
                return transform.parent.gameObject;
            }
        }

        private TimerInstance attackTimer;
        private IModelController modelController;

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
            if (CombatTarget == null) { /*Debug.Log("CAN'T AUTO ATTACK. TARGET IS NULL!");*/ return false; }
            if (CombatTarget.IsDead) { /*Debug.Log("CAN'T AUTO ATTACK. TARGET IS DEAD!");*/ OnTargetDied?.Invoke(CombatTarget); return false; }
            if (!InTargetAttackRange(CombatTarget.DamageableObject.transform)) { /*Debug.Log("CAN'T AUTO ATTACK. NOT WITHIN TARGET RANGE!");*/ return false; }
            if (!CanAttack) { /*Debug.Log("CAN'T AUTO ATTACK. ATTACK IS ON COOLDOWN!");*/ return false; }
            if (IsAttacking) { /*Debug.Log("CAN'T AUTO ATTACK. ALREADY ATTACKING!");*/ return false; }

            cachedTarget = CombatTarget;
            OnAttackStart?.Invoke();
            modelController.SetAttackSpeed(AttackSpeed);
            modelController.PlayAnimation(attackTrigger);
            IsAttacking = true;
            modelController.OnAnimationHit += CheckForHit;
            modelController.OnAnimationEnd += CheckForEnd;
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
            modelController.OnAnimationHit -= CheckForHit;
        }

        public void EndAttack()
        {
            OnAttackEnd?.Invoke();
            IsAttacking = false;
            modelController.OnAnimationEnd -= CheckForEnd;
        }

        public void CriticalHit()
        {
            OnCriticalHit?.Invoke();
        }

        private void CheckForHit(string _animation) 
        {
            if (_animation != attackTrigger) { return; }
            ExecuteAttack(); 
        }

        private void CheckForEnd(string _animation) 
        {
            if (_animation != attackTrigger) { return; }
            EndAttack(); 
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