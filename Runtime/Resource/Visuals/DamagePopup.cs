using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Elysium.Utils.Components;
using Elysium.Utils.Timers;

namespace Elysium.Combat
{
    public struct DamagePopupStyle
    {
        public Color Color;
        public int FontSize;
        public int SortOrder;        
        public string Format;
        public bool Billboard;
        public float FadeInTime;
        public float FadeOutTime;
        public Vector3 Movement;
        public Vector3 Offset;
        public Quaternion Rotation;
        
        public static DamagePopupStyle Default => new DamagePopupStyle
        {
            Color = Color.white,
            FontSize = 5,
            SortOrder = 1,
            Format = "{0}",
            Billboard = true,            
            FadeInTime = 1f,
            FadeOutTime = 0.7f,
            Movement = new Vector3(0.2f, 1f) * 3f,
            Offset = Vector3.zero,
            Rotation = Quaternion.identity,
        };
    }

    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textComponent = default;

        private TimerInstance timer = default;

        private float fadeInTime;
        private float fadeOutTime; 
        private Vector3 moveVector;
        private bool fadingIn;

        public DamagePopup Create(Vector3 _position, int _amount, DamagePopupStyle? _style = null)
        {
            DamagePopupStyle style = _style.GetValueOrDefault(DamagePopupStyle.Default);
            DamagePopup popup = Instantiate(this, _position + style.Offset, style.Rotation);
            popup.ApplyStyle(_amount, style);
            popup.StartFadingIn();
            return popup;
        }

        private void Awake()
        {
            if (textComponent == null) { textComponent = transform.GetComponent<TextMeshPro>(); }
        }

        private void ApplyStyle(int _amount, DamagePopupStyle _style)
        {
            textComponent.fontSize = _style.FontSize;
            textComponent.color = _style.Color;            
            textComponent.sortingOrder = _style.SortOrder;
            textComponent.text = string.Format(_style.Format, _amount);

            moveVector = _style.Movement;
            fadeInTime = _style.FadeInTime;
            fadeOutTime = _style.FadeOutTime;

            if (_style.Billboard) 
            { 
                var billboard = gameObject.AddComponent<Billboard>();
                billboard.Type = Billboard.A3DBillboardType.Flat;
            }
        }
        private void HandleMovement()
        {
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 1.5f * Time.deltaTime;
        }

        private void HandleSize()
        {
            if (fadingIn && timer.Time > fadeInTime * 0.5f)
            {
                float increaseScaleAmount = 0.5f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                float decreaseScaleAmount = 0.5f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }
        }

        private void HandleColor()
        {
            float alpha = timer.Time / fadeOutTime;
            Color textColor = textComponent.color;
            textColor.a = alpha;
            textComponent.color = textColor;
        }

        // ------------------------------------------ FADE IN ------------------------------------------
        private void StartFadingIn()
        {
            fadingIn = true;
            timer = Timer.CreateTimer(fadeInTime, () => !this, false);
            timer.OnTick += OnFadingInTimerTick;
            timer.OnEnd += OnFadingInTimerEnd;
        }

        private void OnFadingInTimerTick()
        {
            HandleMovement();
            HandleSize();
        }

        private void OnFadingInTimerEnd()
        {
            StartFadingOut();
        }

        // ------------------------------------------ FADE OUT -----------------------------------------
        private void StartFadingOut()
        {
            fadingIn = false;
            timer = Timer.CreateTimer(fadeOutTime, () => !this, false);
            timer.OnTick += OnFadingOutTimerTick;
            timer.OnEnd += OnFadingOutTimerEnd;
        }

        private void OnFadingOutTimerTick()
        {
            HandleMovement();
            HandleSize();
            HandleColor();
        }

        private void OnFadingOutTimerEnd()
        {
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            if (textComponent == null) { textComponent = transform.GetComponent<TextMeshPro>(); }
        }
    }
}