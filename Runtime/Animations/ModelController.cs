using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public class ModelController : MonoBehaviour, IModelController
    {
        [SerializeField] private Transform firepoint = default;
        [SerializeField] private bool mockAnimation = false;

        private string lastPlayedAnimation = "";
        private Animator anim = default;        

        public event UnityAction<string> OnAnimationHit;
        public event UnityAction<string> OnAnimationEnd;

        public Transform Firepoint => firepoint;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void HitAnimation()
        {
            if (lastPlayedAnimation == "") { return; }
            Debug.Log($"Animation {lastPlayedAnimation} has hit.");
            OnAnimationHit?.Invoke(lastPlayedAnimation);
        }

        private void EndAnimation()
        {
            if (lastPlayedAnimation == "") { return; }
            Debug.Log($"Animation {lastPlayedAnimation} ended.");
            OnAnimationEnd?.Invoke(lastPlayedAnimation);

            if (!mockAnimation) { anim.ResetTrigger(lastPlayedAnimation); }
            lastPlayedAnimation = "";
        }

        public void PlayAnimation(string _stateName)
        {
            Debug.Log($"Playing animation {_stateName}...");
            lastPlayedAnimation = _stateName;

            if (!mockAnimation)
            {
                anim.Play(_stateName);
                StartCoroutine(WaitForAnimation());
            }

            else { StartCoroutine(MockAnimation()); }
        }

        private IEnumerator WaitForAnimation()
        {
            // if (anim.IsInTransition(0)) { yield return new WaitUntil(() => !anim.IsInTransition(0)); }

            yield return new WaitUntil(() => anim.IsInTransition(0));
            Debug.Log($"Animation {lastPlayedAnimation} is transitioning in");
            float duration = anim.GetAnimatorTransitionInfo(0).duration;
            yield return new WaitForSeconds(duration);
            yield return new WaitUntil(() => anim.IsInTransition(0));
            Debug.Log($"Animation {lastPlayedAnimation} is transitioning out");
            EndAnimation();
            yield return null;
        }

        private IEnumerator MockAnimation()
        {
            yield return new WaitForSeconds(1);
            HitAnimation();
            yield return new WaitForSeconds(1);
            EndAnimation();
            yield return null;
        }

        public Material SetMaterial(Material _material)
        {
            MeshRenderer rend = GetComponent<MeshRenderer>();
            Material prev = rend.sharedMaterial;
            rend.material = _material;
            return prev;
        }

        public void SetAttackSpeed(float _aspd)
        {
            anim.SetFloat("aspd", _aspd);
        }
    }
}