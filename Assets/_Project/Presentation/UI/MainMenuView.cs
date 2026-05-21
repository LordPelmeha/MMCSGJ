#nullable enable
namespace HalfEmpty.Presentation.UI {
/// <summary>
/// Main menu with Play, Quit actions.
/// </summary>
public class MainMenuView
{
    /// <summary>Start a new game from Level_01.</summary>
    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
}