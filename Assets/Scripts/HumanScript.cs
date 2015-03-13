/* 
 * SSB4455 2015.03.07
 * 
 */
using UnityEngine;

public class HumanScript : MonoBehaviour
{
	Transform trans;
	public HumanLadderController humanLadderController;
	bool falling;
	float fallingTime;
	bool added;

	Vector3 position;
	public Vector3 Position
	{
		get
		{
			return trans.position;
		}
		set
		{
			position = value;
		}
	}



	void Awake()
	{
		trans = transform;
	}

	internal void ReStart(Transform parent)
	{
		gameObject.SetActive(true);
		trans.parent = parent;
		position = Vector3.zero;
		trans.localPosition = position;
		trans.eulerAngles = Vector3.zero;
		falling = false;
		added = false;
	}

	internal void Fall()
	{
		falling = true;
		trans.parent = null;
		fallingTime = 0.5f;
	}
	
	void Update ()
	{
		if (!falling)
		{
			trans.localPosition = position;
		}

		if (!added && falling)
		{
			fallingTime -= Time.deltaTime;
			if (fallingTime < 0)
			{
				humanLadderController.AddLadderCount(this);
				added = true;
			}
		}

		if (trans.position.y < -6)
		{
			gameObject.SetActive(false);
			humanLadderController.ReduceLadderCount(this);
		}

	}

}
