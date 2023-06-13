using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectModifier
{
    public interface ISynthesisModifier
    {
        public void Apply(float value);
    }
}
