using System;
using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SGizmos : SUIElement
    {
        protected SGizmos(UnityAction onRender) : base(onRender)
        {
        }

        public static SGizmosScope Scope(SUIElement content = null)
            => new SGizmosScope(content);
        public static SUIElement Cube(Vector3 origin, Vector3 size)
            => new SGizmos(() => Gizmos.DrawCube(origin, size));
        public static SUIElement WireCube(Vector3 origin, Vector3 size)
            => new SGizmos(() => Gizmos.DrawWireCube(origin, size));
        public static SUIElement Sphere(Vector3 origin, float radius)
            => new SGizmos(() => Gizmos.DrawSphere(origin, radius));

        public static SUIElement Line(Vector3 from, Vector3 to)
            => new SGizmos(() => Gizmos.DrawLine(from, to));

        public static SUIElement Action(UnityAction action)
            => new SGizmos(() => action?.Invoke());
    }
}