using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Combat
{
    public class GenericElement : IElement
    {
        private string name;
        private Color color;
        private Dictionary<string, float> interactions;
        public string Name => name;

        public GenericElement(string _name, Color _color, Dictionary<string, float> _interactions = null)
        {
            this.name = _name;
            this.color = _color;

            _interactions = _interactions != null ? _interactions : new Dictionary<string, float>();
            this.interactions = validate(_interactions);
        }

        public float Against(IElement _element)
        {
            if (!interactions.ContainsKey(_element.Name)) 
            {
                Debug.LogError($"Interactions map doesn't contain key {_element.Name}");
                return 1f; 
            }
            return interactions[_element.Name];
        }

        public virtual DamagePopupStyle GetStyle(bool _crit)
        {
            var style = new DamagePopupStyle
            {
                Color = color,
                FontSize = _crit ? 7 : 5,
                SortOrder = 1,
                Format = "{0}",
                Billboard = true,
                FadeInTime = 1f,
                FadeOutTime = 0.7f,
                Movement = new Vector3(0.2f, 1f) * 3f,
                Offset = Vector3.zero,
                Rotation = Quaternion.identity,
            };

            return style;
        }

        private Dictionary<string, float> validate(Dictionary<string, float> _interactions)
        {
            string[] allElements = Enum.GetValues(typeof(Element)).Cast<Element>().Select(x => x.ToString().ToLower()).ToArray();
            _interactions = addMissingKeys(allElements, _interactions);
            _interactions = removeExtraKeys(allElements, _interactions);
            return _interactions;
        }

        private Dictionary<string, float> addMissingKeys(string[] _elements, Dictionary<string, float> _interactions)
        {
            foreach (var e in _elements)
            {
                if (!_interactions.ContainsKey(e))
                {
                    _interactions[e] = 1f;
                }
            }

            return _interactions;
        }

        private Dictionary<string, float> removeExtraKeys(string[] _elements, Dictionary<string, float> _interactions)
        {
            List<string> keysToRemove = new List<string>();
            foreach (var i in _interactions)
            {
                if (!_elements.Contains(i.Key))
                {
                    keysToRemove.Add(i.Key);                    
                }
            }

            foreach (var key in keysToRemove)
            {
                _interactions.Remove(key);
            }

            return _interactions;
        }
    }
}