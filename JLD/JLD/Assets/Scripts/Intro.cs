using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
* Intro menu
**/
public class Intro : MonoBehaviour
{
    public Image logo;
    public Image splash;
    public Button play;

    public float timer = 0;
    
	void Update ()
    {
	    if (timer < 3)
        {
            timer += Time.deltaTime;
            if (timer >= 3)
            {
                logo.gameObject.SetActive(false);
                splash.gameObject.SetActive(true);
                play.gameObject.SetActive(true);
            }
        }
	}

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
