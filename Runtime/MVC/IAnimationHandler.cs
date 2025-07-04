using UnityEngine;
using System.Collections.Generic;

namespace Corelib.Utils
{
    public interface IAnimationHandler
    {
        void PlayAnimation(string animationName, string partName = null);
        void SetAnimationTrigger(string triggerName, string partName = null);
        void SetAnimationFloat(string floatName, float value, string partName = null);
        void SetAnimationBool(string boolName, bool value, string partName = null);
    }
}