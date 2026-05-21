#nullable enable
using UnityEngine;
namespace HalfEmpty.Presentation.UI {
/// <summary>
/// Game Over screen shown when the player dies. Offers Restart and Main Menu options.
/// </summary>
public class GameOverView : MonoBehaviour
{
    /// <summary>
    /// Called when the player chooses to restart.
    /// </summary>
    public void OnRestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
}