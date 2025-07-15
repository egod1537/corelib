using UnityEngine;

namespace Corelib.Utils
{
    public static class ExCharacterController
    {
        public static void SetPosition(this CharacterController controller, Vector3 destination)
        {
            controller.enabled = false;
            controller.transform.position = destination;
            controller.enabled = true;
        }
    }
}