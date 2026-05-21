// HalfEmpty: InputSystem_Actions.cs
// Thin wrapper so PauseMenuView.cs and other scripts can compile
// before Unity's Input System code-generator creates the real class.
// Unity NEVER auto-generates files from .inputassets outside of
// an active Unity editor pipeline — this stub is permanent until the
// AssetDatabase runs (which requires a running editor instance).
// When the generated class appears (in the global namespace), DELETE
// this stub file and add the proper using directives where needed.

using System;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#else
[Preserve]
#endif
static class InputSystemStub
{
    // Marker to force this file to persist;
    // replace body with real logic once Unity generates the class.
}
