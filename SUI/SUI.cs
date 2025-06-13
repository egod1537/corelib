using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SUI
    {
        public static readonly UnityAction<SUIElement> Render = (element) => element.Render();
    }
}