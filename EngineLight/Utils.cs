using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineLight
{
    //Not a MonoBehaviour!
    class Utils
    {
        //As the name says, checks if the user is using the IVA camera
        //Added the OR just in case that other camera mode is also IVA!
        public static bool isIVA()
        {
             return CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.IVA || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal;
        }


    }
}
