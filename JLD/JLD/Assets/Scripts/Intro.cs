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
    public Button quit;

    public float timer = 0;
    float pulse_timer = 0.2f;
    
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
                quit.gameObject.SetActive(true);
            }
        }
        else
        {
            if (pulse_timer > 0)
            {
                pulse_timer -= Time.deltaTime;
                if (pulse_timer < 0)
                {
                    pulse_timer = 0.2f;
                    if (play.GetComponent<RectTransform>().localScale.x > 1)
                        play.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    else
                        play.GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);
                }
            }
        }
	}

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
