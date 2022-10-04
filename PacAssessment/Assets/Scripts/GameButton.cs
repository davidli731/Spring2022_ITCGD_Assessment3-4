using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    public void exitButtonOnPress()
    {
        SceneManager.LoadScene((int)GameState.StartScene);
    }
}
