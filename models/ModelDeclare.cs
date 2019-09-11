using System;
using System.Collections.Generic;

namespace code_generator.models
{
    public class ModelDeclare
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<ModelProperty> ModelProperties { get; set; }
    }

    public class ModelProperty
    {
        public string Name { get; set; }

        public string ValueType { get; set; }

        public string UnitType { get; set; }

        public string Description { get; set; }
    }
}