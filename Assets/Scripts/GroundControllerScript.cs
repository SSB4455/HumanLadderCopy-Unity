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
					nextHumanTrans = (Instantiate(humanPrefab) as GameObject).transform;
					nextHumanTrans.position = nextHumanPosition;
					Rigidbody2D rigidbody2D = nextHumanTrans.GetComponent<Rigidbody2D>();
					rigidbody2D.centerOfMass = new Vector2(0, 0.01f);
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
		}

		if (maxHeight > cameraPositionY)
		{
			//cameraMoveShift = 0;
			cameraPositionY = maxHeight + 1;
		} else if (maxHeight < cameraPositionY - 2) {
			//cameraMoveShift = 0;
			cameraPositionY = maxHeight + 1;
		}
		if (mainCamera.transform.position.y != cameraPositionY)
		{
			Debug.Log(Mathf.Sign(cameraPositionY - mainCamera.transform.position.y));
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

	/*void UpdateTouchs()
	{
		Touch[] currentTouches = Input.touches;
		for (int i = 0; i < currentTouches.Length; i++)
		{
			bool inFingerTouchList = false;
			for (int j = 0; j < fingerTouchList.Count; j++)
			{
				if (currentTouches[i].fingerId == fingerTouchList[j].touch.fingerId)
				{
					fingerTouchList[j].updated = true;
					fingerTouchList[j].touch = currentTouches[i];
					if (fingerTouchList[j].touch.tapCount > fingerTouchList[j].tapCount)
					{
						fingerTouchList[j].tapCount = fingerTouchList[j].touch.tapCount;
						AddTouch(fingerTouchList[j]);
					}
					//Debug.Log("touch.tapCount = " + fingerTouchList[j].touch.tapCount);
					inFingerTouchList = true;
					break;
				}
			}

			if (!inFingerTouchList)
			{
				if (fingerTouchPrepareList.Count < 1)
				{
					fingerTouchPrepareList.Add(new FingerTouch());
				}
				fingerTouchPrepareList[0].touch = currentTouches[i];
				fingerTouchPrepareList[0].tapCount = currentTouches[i].tapCount;
				fingerTouchPrepareList[0].updated = true;
				fingerTouchPrepareList[0].onScreen = true;
				FingerTouch fingerTouch = fingerTouchPrepareList[0];
				fingerTouchList.Add(fingerTouch);
				fingerTouchPrepareList.RemoveAt(0);
				TouchNumber++;
				AddTouch(fingerTouch);
			}
		}
		for (int i = 0; i < fingerTouchList.Count; i++)
		{
			if (!fingerTouchList[i].updated || fingerTouchList[i].touch.phase == TouchPhase.Ended || fingerTouchList[i].touch.phase == TouchPhase.Canceled)
			{
				fingerTouchList[0].updated = false;
				fingerTouchList[i].onScreen = false;
				fingerTouchPrepareList.Add(fingerTouchList[i]);
				fingerTouchList.RemoveAt(i);
				TouchNumber--;
				i--;
			}
			else
			{
				fingerTouchList[i].updated = false;
			}
		}

	}*/


	private GUIStyle fpsStyle;
	private void OnGUI()
	{
		GUILayout.Label("Count = " + humans.Count
			+ "Input.mousePosition = " + Input.mousePosition + "\nnextHumanPosition = " + nextHumanPosition
			//+ "\nScreen.currentResolution.height/2 = " + (Screen.currentResolution.height / 2)
			//+ "\nhalf screen Hight = " + mainCamera.ScreenPointToRay(new Vector3(0, Screen.currentResolution.height / 2, 0))
			+ "\nScreenPointToRay = " + mainCamera.ScreenPointToRay(Input.mousePosition), fpsStyle);
	}
}
