using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BackgroundAudio.Instance.PlaySound("click");
        BackgroundAudio.Instance.swapGame.TransitionTo(10f);
        SceneManager.LoadScene("SwapGame");
    }

}
