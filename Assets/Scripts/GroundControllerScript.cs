/* 
 * SSB4455 20150304
 * 
 */
using UnityEngine;
using System.Collections.Generic;

public class GroundControllerScript : MonoBehaviour
{
	Camera mainCamera;
	public GameObject humanPrefab;
	public float waitTime = 2;
	float waitShift;
	int status;
	int ladderCount;

	Transform nextHumanTrans;
	public List<Transform> prepareHumans;
	List<Transform> humans = new List<Transform>();
	Vector3 nextHumanPosition;
	float cameraPositionY;
	float cameraSpeed = 5;
	
	
	
	// Use this for initialization
	void Start ()
	{
		status = 0;
		waitShift = 0;
		nextHumanPosition = new Vector3(0, 3, 0);
		mainCamera = Camera.main;
		cameraPositionY = mainCamera.transform.position.y;




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
					nextHumanTrans.position = nextHumanPosition;
				}
				break;
			case 1:		// 调整摄像机高度
				waitShift += Time.deltaTime;

				nextHumanTrans.position = nextHumanPosition;
				break;
			case 2:		// 准备获取触摸或点击
				nextHumanTrans.position = nextHumanPosition;
				if (Input.GetButtonDown("Fire1"))
				{
					status++;
				}
				break;
			case 3:		// 持续触摸或点击 调整位置 
				nextHumanTrans.position = nextHumanPosition;
				//Debug.Log("Input.GetButton(\"Fire1\") = " + Input.GetButton("Fire1"));
				if (Input.GetButton("Fire1"))
				{
					nextHumanPosition.x = Camera.main.ScreenPointToRay(Input.mousePosition).origin.x;
				} else {
					status++;
				}
				break;
			case 4:		// 松手 放开 准备下一个human
				waitShift = waitTime;
				nextHumanPosition.x = 0;
				status = 0;
				break;
		}


		//float heightLimmmit = mainCamera.ScreenPointToRay(new Vector3(0, Screen.currentResolution.height / 2, 0)).origin.y;
		float maxHeight = 0;
		for (int i = 0; i < humans.Count; i++)
		{
			maxHeight = humans[i].position.y > maxHeight ? humans[i].position.y : maxHeight;
			if (humans[i].position.y < -4)
			{
				humans[i].gameObject.SetActive(false);
				prepareHumans.Add(humans[i]);
				humans.RemoveAt(i--);
				ladderCount--;
			}
		}

		if (maxHeight > cameraPositionY)
		{
			cameraPositionY = maxHeight + 1;
		} else if (maxHeight < cameraPositionY - 2) {
			cameraPositionY = maxHeight + 1;
		}
		if (mainCamera.transform.position.y != cameraPositionY)
		{
			float direction = Mathf.Sign(cameraPositionY - mainCamera.transform.position.y);
			mainCamera.transform.Translate(0, direction * Time.deltaTime * cameraSpeed, 0);
			if (direction * (cameraPositionY - mainCamera.transform.position.y) < 0)
			{
				mainCamera.transform.position = new Vector3(0, cameraPositionY, -100);
			}
			nextHumanPosition.y = mainCamera.transform.position.y + 3;
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
		humanTrans.gameObject.SetActive(true);
		humanTrans.eulerAngles = Vector3.zero;
		return humanTrans;
	}

	private GUIStyle fpsStyle;
	private void OnGUI()
	{
		GUILayout.Label("ladderCount = " + ladderCount
			//+ "\nInput.mousePosition = " + Input.mousePosition
			+ "\nnextHumanPosition = " + nextHumanPosition
			//+ "\nScreen.currentResolution.height/2 = " + (Screen.currentResolution.height / 2)
			//+ "\nhalf screen Hight = " + mainCamera.ScreenPointToRay(new Vector3(0, Screen.currentResolution.height / 2, 0))
			//+ "\nScreenPointToRay = " + mainCamera.ScreenPointToRay(Input.mousePosition)
			, fpsStyle);
	}
}
