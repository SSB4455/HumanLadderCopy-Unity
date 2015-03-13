/* 
 * SSB4455 2015.03.13
 * 
 */
using UnityEngine;
using UnityEngine.UI;

public class Mode60sScript : MonoBehaviour
{
	public HumanLadderController humanLadderController;
	public AudioSource gameSound;
	public Text timeText;
	public float time = 60;

	
	
	void Start ()
	{
		//time = 60;
	}
	
	void Update ()
	{
		time -= Time.deltaTime;
		if (time < 0)
		{
			time = 0;
			humanLadderController.playingOn = false;
			gameSound.Stop();
		}
		timeText.text = "Time: " + ((int)time).ToString();

	}
}
