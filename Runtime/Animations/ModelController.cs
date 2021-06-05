using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public class ModelController : MonoBehaviour, IModelController
    {
        [SerializeField] private bool mockAnimation = false;

        private string lastPlayedAnimation = "";
        private Animator anim = default;

        public event UnityAction<string> OnAnimationHit;
        public event UnityAction<string> OnAnimationEnd;

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

        public void EndAnimation()
        {
            if (lastPlayedAnimation == "") { return; }
            Debug.Log($"Animation {lastPlayedAnimation} ended.");
            OnAnimationEnd?.Invoke(lastPlayedAnimation);

            if (!mockAnimation) { anim.ResetTrigger(lastPlayedAnimation); }
            lastPlayedAnimation = "";
        }

        public void PlayAnimation(string _trigger)
        {
            Debug.Log($"Playing animation {_trigger}...");
            lastPlayedAnimation = _trigger;
            if (!mockAnimation) { anim.SetTrigger(_trigger); }
            else { StartCoroutine(MockAnimation()); }
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
            Material prev = rend.material;
            rend.material = _material;
            return prev;
        }

        public void SetAttackSpeed(float _aspd)
        {
            anim.SetFloat("aspd", _aspd);
        }
    }
}