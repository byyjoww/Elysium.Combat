using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Combat
{
    public enum Element
    {
        UNKNOWN,
        FIRE,
        WATER,
        WIND,
        EARTH,
        POISON,
    }

    public static class ElementFactory
    {
        private static IElement[] elements = new IElement[]
        {
            new GenericElement(Element.FIRE, Color.red, new Dictionary<Element, float>{ 
                { Element.WATER, 0.5f }, 
                { Element.EARTH, 2.0f },
            }),

            new GenericElement(Element.WATER, Color.blue, new Dictionary<Element, float>{
                { Element.WIND, 0.5f },
                { Element.FIRE, 2.0f },
            }),

            new GenericElement(Element.WIND, Color.yellow, new Dictionary<Element, float>{
                { Element.EARTH, 0.5f },
                { Element.WATER, 2.0f },
            }),

            new GenericElement(Element.EARTH, Color.green, new Dictionary<Element, float>{
                { Element.FIRE, 0.5f },
                { Element.WIND, 2.0f },
            }),

            new GenericElement(Element.POISON, Color.magenta, DefaultElementInteractions),
        };

        public static Element[] AllElementKeys => Enum.GetValues(typeof(Element)).Cast<Element>().ToArray();
        public static Dictionary<Element, float> DefaultElementInteractions => AllElementKeys.ToDictionary(x => x, x => 1f);
        public static IElement[] Elements => elements;

        public static IElement GetElementByKeyOrDefault(Element _key)
        {
            return GetElementByKey(_key, out IElement element) ? element : new NullElement();
        }

        public static IElement GetElementByKeyOrDefault(string _key)
        {
            return GetElementByKey(_key, out IElement element) ? element : new NullElement();
        }

        public static bool GetElementByKey(Element _key, out IElement _element)
        {
            Dictionary<Element, IElement> dictionary = elements.ToDictionary(x => x.Name, x => x);
            bool dictionaryContainsKey = dictionary.ContainsKey(_key);
            _element = dictionaryContainsKey ? dictionary[_key] : new NullElement();
            return dictionaryContainsKey;
        }        

        public static bool GetElementByKey(string _key, out IElement _element)
        {
            bool parsed = Enum.TryParse(_key.ToUpper(), out Element key);
            bool hasKey = GetElementByKey(key, out IElement element);
            _element = parsed && hasKey ? element : new NullElement();
            return parsed && hasKey;
        }
    }
}