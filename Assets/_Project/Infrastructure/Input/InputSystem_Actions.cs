// HalfEmpty: InputSystem_Actions.cs
// Minimal stand-in so PauseMenuView.cs (and any other script that references
// InputSystem_Actions) can compile without Unity's code-generator having run.
// Unity generates this class in the ROOT (unnamed) namespace from
// InputSystem_Actions.inputactions; once generated, DELETE this file.
// There can only be ONE class with a given name in the root namespace so
// Unity's generator and this file are mutually exclusive.

using System;
using UnityEngine.InputSystem;

namespace InputSystem_ActionsNamespace
{
    public class InputSystem_Actions : InputActionAsset
    {
        public InputActionMap Player { get; }
        public InputSystem_Actions()
        {
            Player = new InputActionMap();
        }
    }

    public class InputActionMap
    {
        public InputAction Pause { get; }
        public InputActionMap() { Pause = new InputAction(); }
    }

    public class InputAction
    {
        public bool triggered => false;
    }
}
