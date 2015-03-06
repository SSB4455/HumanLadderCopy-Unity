/* 
 * SSB4455 20150304
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GroundControllerScript : MonoBehaviour
{
	Camera mainCamera;
	public GameObject humanPrefab;
	public Text countText;
	public Transform fallPoint;
	public AudioSource touchSound;
	public AudioSource fallSound;
	public AudioSource missSound;
	public float waitTime = 2;
	float waitShift;
	int status;
	int ladderCount = -1;
	float screenDown;

	Transform nextHumanTrans;
	public List<Transform> prepareHumans;
	List<Transform> humans = new List<Transform>();
	float cameraPositionY;
	float cameraSpeed = 5;
	
	
	
	// Use this for initialization
	void Start ()
	{
		status = 0;
		waitShift = 0;
		mainCamera = Camera.main;
		cameraPositionY = mainCamera.transform.position.y;
		screenDown = -6;

		


		fpsStyle = new GUIStyle();
		//fpsStyle.normal.background = null;
		fpsStyle.normal.textColor = new Color(0, 0, 0);
		//fpsStyle.fontSize = 40;
	}
	
	// Update is called once per frame
	void Update () {

		switch (status)
		{
			case 0:
				waitShift -= Time.deltaTime;
				if (waitShift < 0)
				{
					status = 2;
					if (nextHumanTrans != null)
					{
						humans.Add(nextHumanTrans);
					}
					nextHumanTrans = GetHuman();
					nextHumanTrans.localPosition = Vector3.zero;
				}
				break;
			case 1:		// 调整摄像机高度
				waitShift += Time.deltaTime;

				nextHumanTrans.localPosition = Vector3.zero;
				break;
			case 2:		// 准备获取触摸或点击
				nextHumanTrans.localPosition = Vector3.zero;
				if (Input.GetButtonDown("Fire1"))
				{
					status++;
					touchSound.Play();
				}
				break;
			case 3:		// 持续触摸或点击 调整位置
				//Debug.Log("Input.GetButton(\"Fire1\") = " + Input.GetButton("Fire1"));
				if (Input.GetButton("Fire1"))
				{
					nextHumanTrans.localPosition = new Vector3(Camera.main.ScreenPointToRay(Input.mousePosition).origin.x, 0, 0);
				} else {
					status++;
					fallSound.Play();
				}
				break;
			case 4:		// 松手 放开 准备下一个human
				waitShift = waitTime;
				nextHumanTrans.parent = null;
				status = 0;
				break;
		}


		//float heightLimmmit = mainCamera.ScreenPointToRay(new Vector3(0, Screen.currentResolution.height / 2, 0)).origin.y;
		float maxHeight = 0;
		for (int i = 0; i < humans.Count; i++)
		{
			maxHeight = humans[i].position.y > maxHeight ? humans[i].position.y : maxHeight;
			if (humans[i].position.y < -3 && humans[i].position.y < screenDown)
			{
				humans[i].gameObject.SetActive(false);
				prepareHumans.Add(humans[i]);
				humans.RemoveAt(i--);
				ladderCount--;
				countText.text = ladderCount.ToString();
				missSound.Play();
			}
		}

		if (maxHeight > cameraPositionY)
		{
			cameraPositionY = maxHeight + 1.8f;
		} else if (maxHeight < cameraPositionY - 2) {
			cameraPositionY = maxHeight + 1.8f;
		}
		if (mainCamera.transform.position.y != cameraPositionY)
		{
			float direction = Mathf.Sign(cameraPositionY - mainCamera.transform.position.y);
			mainCamera.transform.Translate(0, direction * Time.deltaTime * cameraSpeed, 0);
			if (direction * (cameraPositionY - mainCamera.transform.position.y) < 0)
			{
				mainCamera.transform.position = new Vector3(0, cameraPositionY, -100);
				screenDown = mainCamera.ScreenPointToRay(Vector3.zero).origin.y - 1;
			}
		}


		// exit
		if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	Transform GetHuman()
	{
		Transform humanTrans = null;
		if (prepareHumans.Count > 0)
		{
			humanTrans = prepareHumans[0];
			prepareHumans.RemoveAt(0);
		} else {
			humanTrans = (Instantiate(humanPrefab) as GameObject).transform;
			Rigidbody2D rigidbody2D = nextHumanTrans.GetComponent<Rigidbody2D>();
			rigidbody2D.centerOfMass = new Vector2(0, 0.01f);
		}

		ladderCount++;
		countText.text = ladderCount.ToString();
		humanTrans.gameObject.SetActive(true);
		humanTrans.parent = fallPoint;
		humanTrans.eulerAngles = Vector3.zero;
		return humanTrans;
	}

	private GUIStyle fpsStyle;
	private void OnGUI()
	{
		//GUILayout.Label("ladderCount = " + ladderCount
			//+ "\nInput.mousePosition = " + Input.mousePosition
			//+ "\nScreen.currentResolution.height/2 = " + (Screen.currentResolution.height / 2)
			//+ "\nhalf screen Hight = " + mainCamera.ScreenPointToRay(new Vector3(0, Screen.currentResolution.height / 2, 0))
			//+ "\nScreenPointToRay = " + mainCamera.ScreenPointToRay(Input.mousePosition)
			//, fpsStyle);
	}
}
