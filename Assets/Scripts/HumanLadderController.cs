/* 
 * SSB4455 2015.03.04
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HumanLadderController : MonoBehaviour
{
	public AudioSource touchSound;
	public AudioSource fallSound;
	public AudioSource missSound;
	public Transform fallPoint;
	public Text countText;
	public int ladderCount = 0;
	public bool playingOn;
	float waitTime = 2;
	float waitShift;
	int status;

	public GameObject humanPrefab;
	HumanScript nextHuman;
	public List<HumanScript> prepareHumans;
	List<HumanScript> humans = new List<HumanScript>();
	
	
	
	// Use this for initialization
	void Start ()
	{
		status = 0;
		waitShift = 0;

		playingOn = true;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!playingOn)
		{
			return;
		}
		switch (status)
		{
			case 0:
				waitShift -= Time.deltaTime;
				if (waitShift < 0)
				{
					nextHuman = GetHuman();
					nextHuman.ReStart(fallPoint);
					status = 2;
				}
				break;
			case 1:
				status++;
				break;
			case 2:		// 准备获取触摸或点击
				if (Input.GetButtonDown("Fire1"))
				{
					touchSound.Play();
					status++;
				}
				break;
			case 3:		// 持续触摸或点击 调整位置
				//Debug.Log("Input.GetButton(\"Fire1\") = " + Input.GetButton("Fire1"));
				if (Input.GetButton("Fire1"))
				{
					nextHuman.Position = new Vector3(Camera.main.ScreenPointToRay(Input.mousePosition).origin.x, 0, 0);
				} else {
					fallSound.Play();
					nextHuman.Fall();
					status++;
				}
				break;
			case 4:		// 松手 放开 准备下一个human
				waitShift = waitTime;
				status = 0;
				break;
		}
	}

	internal void AddLadderCount(HumanScript human)
	{
		ladderCount++;
		countText.text = ladderCount.ToString();
		humans.Add(human);
	}

	internal void ReduceLadderCount(HumanScript human)
	{
		//Application.CaptureScreenshot("Screenshot.png");
		missSound.Play();
		ladderCount--;
		countText.text = ladderCount.ToString();
		humans.Remove(human);
		prepareHumans.Add(human);
	}

	HumanScript GetHuman()
	{
		HumanScript human = null;
		if (prepareHumans.Count > 0)
		{
			human = prepareHumans[0];
			prepareHumans.RemoveAt(0);
		} else {
			human = (Instantiate(humanPrefab) as GameObject).GetComponent<HumanScript>();
			human.humanLadderController = this;
			Rigidbody2D rigidbody2D = human.GetComponent<Rigidbody2D>();
			rigidbody2D.centerOfMass = new Vector2(0, 0.01f);
		}
		return human;
	}

	public float TopHumanY
	{
		get
		{
			float maxHeight = 0;
			for (int i = 0; i < humans.Count; i++)
			{
				maxHeight = humans[i].Position.y > maxHeight ? humans[i].Position.y : maxHeight;
			}
			return maxHeight;
		}
	}
}
