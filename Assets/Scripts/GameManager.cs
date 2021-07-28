
using UnityEngine;
using UnityEngine.UI;
using HmsPlugin;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	#region Singleton class: GameManager

	public static GameManager Instance;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
	}

	#endregion

	Camera cam;

	public Rock rock;
	public Trajectory trajectory;
	[SerializeField] float pushForce = 4f;

	bool isDragging = false;

	Vector2 startPoint;
	Vector2 endPoint;
	Vector2 direction;
	Vector2 force;
	float distance;

	public Text scoreText;
	public static int score = 0;
	public static int rockCount = 5;
	Rock rockScript;
	public Text rockCountText;

	public Text finalScoreText;

	public bool isThrowing = false;
	public bool isRockLeft = true;

	void Start ()
	{
		cam = Camera.main;
		rock.DesactivateRb ();
		rockScript = GameObject.Find("Rock").GetComponent<Rock>();
		rockCountText.text = "Rocks Remaining " + rockCount;
		scoreText.text = "Score: " + score;
	}

	void Update ()
	{
		if(rock != null)
        {
			if (!isThrowing)
			{
				if (Input.GetMouseButtonDown(0))
				{
					isDragging = true;
					if (isRockLeft)
						OnDragStart();
				}
				if (Input.GetMouseButtonUp(0))
				{
					isDragging = false;
					OnDragEnd();
				}
			}

			if (isDragging)
			{
				OnDrag();
			}
		}
	}

	void OnDragStart ()
	{
		rock.DesactivateRb ();
		startPoint = cam.ScreenToWorldPoint (Input.mousePosition);

		trajectory.Show ();
	}

	void OnDrag ()
	{
		endPoint = cam.ScreenToWorldPoint (Input.mousePosition);
		distance = Vector2.Distance (startPoint, endPoint);
		direction = (startPoint - endPoint).normalized;
		force = direction * distance * pushForce;

		//just for debug
		Debug.DrawLine (startPoint, endPoint);

		if (rock != null)
        {
			trajectory.UpdateDots(rock.pos, force);
		}
		
	}

	void OnDragEnd ()
	{
		rockCount--;
		if (rockCount == 0)
			isRockLeft = false;
		rockCountText.text = "Rocks Remaining " + rockCount;

		//push the rock
		rock.ActivateRb ();

		rock.Push(force);

		trajectory.Hide ();

		isThrowing = true;
	}

}
