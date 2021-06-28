using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elysium.Utils;
using Elysium.Utils.Attributes;

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

    public class ElementFactory : MonoBehaviour, IElementFactory
    {
        public bool Initialized { get; private set; } = false;

        [Separator("Settings", true)]
        [SerializeField] private bool runOnAwake = true;
        [SerializeField] private TextAsset config = default;

        // Builders
        private IModelBuilder modelBuilder = default;

        // Dictionaries
        private Dictionary<string, IElement> ElementDictionary = new Dictionary<string, IElement>();

        private void Awake()
        {
            if (runOnAwake && !Initialized)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            modelBuilder = new YamlModelBuilder(config);
            var ModelDictionary = modelBuilder.GetModels<Dictionary<string, Dictionary<string, object>>>();

            List<IElement> elements = new List<IElement>();
            foreach (var kvp in ModelDictionary)
            {
                elements.Add(GenerateElementFromConfig(kvp));
            }

            ElementDictionary = elements.ToDictionary(x => x.Name, x => x);

            Debug.Log($"Loaded Elements: {string.Join(", ", ElementDictionary.Keys.Select(x => x.ToString().Title()))}");
            Initialized = true;
        }

        public IElement GetElementByKeyOrDefault(Element _key)
        {
            return GetElementByKey(_key.ToString(), out IElement element) ? element : new NullElement();
        }

        public IElement GetElementByKeyOrDefault(string _key)
        {
            return GetElementByKey(_key, out IElement element) ? element : new NullElement();
        }

        private bool GetElementByKey(string _key, out IElement _element)
        {
            _key = _key.ToLower();
            if (!Initialized) { Initialize(); }
            bool dictionaryContainsKey = ElementDictionary.ContainsKey(_key);
            _element = dictionaryContainsKey ? ElementDictionary[_key] : new NullElement();
            return dictionaryContainsKey;
        }

        private IElement GenerateElementFromConfig(KeyValuePair<string, Dictionary<string, object>> _config)
        {
            string name = _config.Key.ToLower();
            _config.Value.TryGetColor("color", out Color color);
            if (_config.Value.TryGetDictionary("multipliers", out var _multipliers))
            {
                var multipliers = _multipliers.ToDictionary(x => x.Key.ToLower(), x => Convert.ToSingle(x.Value));
                return new GenericElement(name, color, multipliers);
            }
            return new GenericElement(name, color);
        }
    }
}