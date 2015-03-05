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
	float cameraMoveTime = 1;
	float cameraMoveShift;
	int status;
	int ladderCount;

	Transform nextHumanTrans;
	public List<Transform> prepareHumans;
	List<Transform> humans = new List<Transform>();
	Vector3 nextHumanPosition;
	Vector3 cameraPosition;
	
	
	
	// Use this for initialization
	void Start ()
	{
		status = 0;
		waitShift = 0;
		nextHumanPosition = new Vector3(0, 3, 0);
		mainCamera = Camera.main;
		cameraPosition = mainCamera.transform.position;




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
			case 1:
				waitShift += Time.deltaTime;
				break;
			case 2:
				nextHumanTrans.position = nextHumanPosition;
				if (Input.GetButtonDown("Fire1"))
				{
					status++;
				}
				break;
			case 3:
				nextHumanTrans.position = nextHumanPosition;
				//Debug.Log("Input.GetButton(\"Fire1\") = " + Input.GetButton("Fire1"));
				if (Input.GetButton("Fire1"))
				{
					nextHumanPosition.x = Camera.main.ScreenPointToRay(Input.mousePosition).origin.x;
				} else {
					status++;
				}

				break;
			case 4:
				waitShift = waitTime;
				nextHumanPosition.x = 0;
				status = 0;
				break;
		}


		if (cameraMoveShift > cameraMoveTime)
		{
			float heightLimmmit = mainCamera.ScreenPointToRay(new Vector3(0, Screen.currentResolution.height / 2, 0)).origin.y;
			float maxHeight = 0;
			for (int i = 0; i < humans.Count; i++)
			{
				maxHeight = humans[i].position.y > maxHeight ? humans[i].position.y : maxHeight;
			}

			if (maxHeight > mainCamera.transform.position.y - 1f)
			{
				cameraMoveShift = 0;
				cameraPosition.y = maxHeight + 1;
			}
			else if (maxHeight < mainCamera.transform.position.y - 3)
			{
				cameraMoveShift = 0;
				cameraPosition.y = maxHeight + 1;
			}
		}
		cameraMoveShift += Time.deltaTime;
		float cameraMoveProgress = cameraMoveShift / cameraMoveTime;
		if (cameraMoveProgress < 1)
		{
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition, cameraMoveProgress);
			nextHumanPosition.y = cameraPosition.y + 3;
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
