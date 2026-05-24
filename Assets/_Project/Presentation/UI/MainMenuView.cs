 #nullable enable
using UnityEngine;
using UnityEngine.SceneManagement;
using HalfEmpty.Application.Game;
using HalfEmpty.Presentation.Game;
namespace HalfEmpty.Presentation.UI {
/// <summary>
/// Main menu with Play, Quit actions.
/// </summary>
public class MainMenuView : MonoBehaviour
{
    /// <summary>Start a new game from Level_01.</summary>
    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
    /// <summary>Quit the application.</summary>
    public void OnQuitButton()
    {
        UnityEngine.Application.Quit();
    }
}
}
