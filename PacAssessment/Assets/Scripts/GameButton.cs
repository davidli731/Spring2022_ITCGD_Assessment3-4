using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    public void exitButtonOnPress()
    {
        SceneManager.LoadScene((int)GameState.StartScene);
    }
}
