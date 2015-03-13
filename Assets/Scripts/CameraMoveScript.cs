/* 
 * SSB4455 2015.03.13
 * 
 */
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
	Transform trans;
	public HumanLadderController humanLadderController;
	public Transform fallPoint;
	float cameraPositionY;
	float cameraSpeed = 5;
	
	
	
	// Use this for initialization
	void Awake ()
	{
		trans = transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float topHumanY = humanLadderController.TopHumanY;
		if (topHumanY > cameraPositionY)
		{
			cameraPositionY = topHumanY + 1.8f;
		} else if (topHumanY < cameraPositionY - 3) {
			cameraPositionY = topHumanY + 1f;
		}

		if (trans.position.y != cameraPositionY)
		{
			float direction = Mathf.Sign(cameraPositionY - trans.position.y);
			trans.Translate(0, direction * Time.deltaTime * cameraSpeed, 0);
			if (direction * (cameraPositionY - trans.position.y) < 0)
			{
				trans.position = new Vector3(0, cameraPositionY, -100);
			}
		}

		// exit
		if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private GUIStyle laberStyle;
	private void OnGUI()
	{
		//GUILayout.Label("ladderCount = " + ladderCount
			//+ "\nInput.mousePosition = " + Input.mousePosition
			//+ "\nScreen.currentResolution.height/2 = " + (Screen.currentResolution.height / 2)
			//+ "\nhalf screen Hight = " + mainCamera.ScreenPointToRay(new Vector3(0, Screen.currentResolution.height / 2, 0))
			//+ "\nScreenPointToRay = " + mainCamera.ScreenPointToRay(Input.mousePosition)
		//, laberStyle);
	}
}
