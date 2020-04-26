using System;
using UnityEngine;

namespace Managers.InputManagers {
    public static class Inputs {
        private static string cameraPanControl = "Camera Pan Control";
        private static string mouseScrollWheel = "Mouse ScrollWheel";
        private static string mouseX = "Mouse X";
        private static string mouseY = "Mouse Y";

        public static bool IsCameraPanControl => Input.GetAxis(cameraPanControl) > 0f;
        public static bool IsMouseScroll => Math.Abs(Input.GetAxis(mouseScrollWheel)) > 0f;

        //Mouse inputs
        public static float MouseScroll => Input.GetAxis(mouseScrollWheel);
        public static float MouseX => Input.GetAxis(mouseX);
        public static float MouseY => Input.GetAxis(mouseY);


    }
}
