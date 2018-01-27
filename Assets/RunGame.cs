using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGame : MonoBehaviour {

	public event System.Action OnChangeRotate;
	public event System.Action OnBeats;

	static RunGame _instance;
	static public RunGame instance
	{
		get
		{
			return _instance;
		}
	}

	public GameObject mainCharacter;

	public GameObject deathEffect;

	//現在居る位置
	public Vector2Int nowPosition;

	//前回に居た位置
	private Vector2Int prevPosition;

	//次に行く位置
	private Vector2Int nextPosition;

	public Field field;

	[HideInInspector]
	public Vector2Int speed;

	private float startTime;
	private float gameTime
	{
		get {
			return Time.time - startTime;
		}
	}

	[SerializeField]
	private float periodTime = 1f; //周期[s]

	//始まってから何秒経過したか
	private float durationTime
	{
		get {
			return Time.time - startTime;
		}
	}

	//現在通過中の場所の何％移動しているか
	private float nowPeriodTimePar
	{
		get {
			float modTime = durationTime % periodTime;
			return modTime / periodTime;
		}
	}

	//始まってから何回脈打ったか
	private float beatCount
	{
		get {
			return durationTime / periodTime;
		}
	}

	int prevBeat;


	private float tapTime;
	private Vector3 tapPosition;

	[SerializeField]
	private float flickEnableWidth = 100f;

	[SerializeField]
	private float flickEnableTime = 0.11f;

	[SerializeField]
	private float flickThreshold = 1f;

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		mainCharacter.transform.position = new Vector3 (nowPosition.x, 1f, nowPosition.y);
		GetStartNextPosition ();
	}
	
	// Update is called once per frame
	void Update () {

		//タップした瞬間
		if (Input.GetMouseButtonDown (0)) {
			//タップした処理
			tapTime = Time.time;
			tapPosition = Input.mousePosition;
			Debug.Log ("tap");
		}

		//フリック処理
		if (Input.GetMouseButtonUp (0)) {
			Vector3 diff = Input.mousePosition - tapPosition;
			float timeDiff = Time.time - tapTime;
			if (flickEnableWidth > Mathf.Abs (diff.x) 
				&& timeDiff < flickEnableTime
				&& diff.magnitude > flickThreshold) {
				if (diff.y > 0) {
					Debug.Log ("上flic!");
					Vector2Int nowDirection = nextPosition - nowPosition;
					Vector2Int nextDirection = nowDirection;
					float rad = 90f * Mathf.Deg2Rad;
					nextDirection.x = nowDirection.x * (int)Mathf.Cos(rad) - nowDirection.y * (int)Mathf.Sin(rad);
					nextDirection.y = nowDirection.x * (int)Mathf.Sin(rad) + nowDirection.y * (int)Mathf.Cos(rad);

					var preNextPosition = nowPosition + nextDirection;
					var parts = field.GetParts (preNextPosition.x, preNextPosition.y);
					if (parts != null && parts.enable) {
						nextPosition = preNextPosition;
						mainCharacter.transform.Rotate (0, -90, 0);
						OnChangeRotate ();
					}
				} else {
					Debug.Log ("下flic!");
					Vector2Int nowDirection = nextPosition - nowPosition;
					Vector2Int nextDirection = nowDirection;
					float rad = -90f * Mathf.Deg2Rad;
					nextDirection.x = nowDirection.x * (int)Mathf.Cos(rad) - nowDirection.y * (int)Mathf.Sin(rad);
					nextDirection.y = nowDirection.x * (int)Mathf.Sin(rad) + nowDirection.y * (int)Mathf.Cos(rad);

					var preNextPosition = nowPosition + nextDirection;
					var parts = field.GetParts (preNextPosition.x, preNextPosition.y);
					if (parts != null && parts.enable) {
						nextPosition = preNextPosition;
						mainCharacter.transform.Rotate (0, 90, 0);
						OnChangeRotate ();
					}
				}
			}
		}
	}

	void FixedUpdate() {
		if (prevBeat != Mathf.FloorToInt (beatCount)) {

			//次のビートに行っている
			prevBeat = Mathf.FloorToInt (beatCount);
			Vector2Int diff = nextPosition - nowPosition;
			prevPosition = nowPosition;
			nowPosition = nextPosition;
			nextPosition = nowPosition + diff;//そのまま進み続ける

			Debug.Log ("beet!");
			OnBeats ();

			var parts = field.GetParts (nowPosition.x, nowPosition.y);
			if (parts == null || !parts.enable) {
				deathEffect.SetActive (true);
			}
		}

		//キャラの位置を合わせる
		var tempNowPeriodTimePar = nowPeriodTimePar;
		if (tempNowPeriodTimePar < 0.5f) {

			//前半
			var diff = prevPosition - nowPosition;
			mainCharacter.transform.position = new Vector3 (
				(float)nowPosition.x - (float)diff.x * tempNowPeriodTimePar + diff.x * 0.5f,
				1f,
				(float)nowPosition.y - (float)diff.y * tempNowPeriodTimePar + diff.y * 0.5f);
		} else {
			//後半
			var diff = nextPosition - nowPosition;
			mainCharacter.transform.position = new Vector3 (
				(float)nowPosition.x + (float)diff.x * (tempNowPeriodTimePar - 0.5f),
				1f,
				(float)nowPosition.y + (float)diff.y * (tempNowPeriodTimePar - 0.5f));
		}
	}

	void GetStartNextPosition() {
		Vector2Int ret = Vector2Int.zero;
		field.AroundLoop (nowPosition, (v2) => {
			var parts = field.GetParts(v2.x, v2.y);
			if (parts != null && parts.enable) {
				ret = v2;
			}
		});
		if (ret == Vector2Int.zero) {
			throw new System.Exception ();
		}
		nextPosition = ret;

		//反対向き
		var diff = nextPosition - nowPosition;
		prevPosition = nowPosition - diff;
	}
}
