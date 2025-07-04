using UnityEngine;
using System.Collections.Generic;
using Corelib.Utils;

namespace Corelib.Utils
{
    public class ViewBaseBehaviour : MonoBehaviour, IAnimationHandler
    {
        [LifecycleInject]
        protected AutoEventSubscriber autoEventSubscriber;
        private Dictionary<string, Animator> animators;

        protected virtual void Awake()
        {
            animators = new Dictionary<string, Animator>();
            var allAnimators = GetComponentsInChildren<Animator>();
            foreach (var animator in allAnimators)
            {
                animators[animator.name] = animator;
            }
            LifecycleInjectionUtil.ConstructLifecycleObjects(this);
        }

        protected virtual void OnEnable()
        {
            LifecycleInjectionUtil.OnEnable(this);
        }

        protected virtual void OnDisable()
        {
            LifecycleInjectionUtil.OnDisable(this);
        }

        public void PlayAnimation(string animationName, string partName = null)
        {
            if (partName != null && animators.TryGetValue(partName, out var specificAnimator))
            {
                specificAnimator.Play(animationName);
            }
            else
            {
                foreach (var animator in animators.Values)
                {
                    animator.Play(animationName);
                }
            }
        }

        public void SetAnimationTrigger(string triggerName, string partName = null)
        {
            if (partName != null && animators.TryGetValue(partName, out var specificAnimator))
            {
                specificAnimator.SetTrigger(triggerName);
            }
            else
            {
                foreach (var animator in animators.Values)
                {
                    animator.SetTrigger(triggerName);
                }
            }
        }

        public void SetAnimationFloat(string floatName, float value, string partName = null)
        {
            if (partName != null && animators.TryGetValue(partName, out var specificAnimator))
            {
                specificAnimator.SetFloat(floatName, value);
            }
            else
            {
                foreach (var animator in animators.Values)
                {
                    animator.SetFloat(floatName, value);
                }
            }
        }

        public void SetAnimationBool(string boolName, bool value, string partName = null)
        {
            if (partName != null && animators.TryGetValue(partName, out var specificAnimator))
            {
                specificAnimator.SetBool(boolName, value);
            }
            else
            {
                foreach (var animator in animators.Values)
                {
                    animator.SetBool(boolName, value);
                }
            }
        }
    }
}