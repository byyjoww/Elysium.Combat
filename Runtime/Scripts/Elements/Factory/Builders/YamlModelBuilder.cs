using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

namespace Elysium.Combat
{
    public class YamlModelBuilder : IModelBuilder
    {
        private TextAsset skillConfigYaml = default;

        public YamlModelBuilder(TextAsset _config)
        {
            this.skillConfigYaml = _config;
        }

        public T GetModels<T>()
        {
            var deserializer = new DeserializerBuilder().Build();
            var skills = deserializer.Deserialize<T>(skillConfigYaml.text);
            return skills;
        }
    }
}