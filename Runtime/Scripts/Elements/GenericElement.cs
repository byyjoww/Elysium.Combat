using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Combat
{
    public class GenericElement : IElement
    {
        private Element name;
        private Color color;
        private Dictionary<Element, float> interactions;
        public Element Name => name;

        public GenericElement(Element _name, Color _color, Dictionary<Element, float> _interactions)
        {
            this.name = _name;
            this.color = _color;            
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

        private Dictionary<Element, float> validate(Dictionary<Element, float> _interactions)
        {
            Element[] allElements = ElementFactory.AllElementKeys;
            _interactions = addMissingKeys(allElements, _interactions);
            _interactions = removeExtraKeys(allElements, _interactions);
            return _interactions;
        }

        private Dictionary<Element, float> addMissingKeys(Element[] _elements, Dictionary<Element, float> _interactions)
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

        private Dictionary<Element, float> removeExtraKeys(Element[] _elements, Dictionary<Element, float> _interactions)
        {
            foreach (var i in _interactions)
            {
                if (!_elements.Contains(i.Key))
                {
                    _interactions.Remove(i.Key);
                }
            }

            return _interactions;
        }
    }
}