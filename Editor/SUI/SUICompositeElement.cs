using System.Collections.Generic;

namespace Corelib.SUI
{
    public class SUICompositeElement : SUIElement
    {
        private readonly List<SUIElement> elements = new();
        public SUICompositeElement(params SUIElement[] elements)
        {
            this.elements.AddRange(elements);
        }

        public override void Render()
        {
            foreach (var element in elements)
                element.Render();
        }
    }
}