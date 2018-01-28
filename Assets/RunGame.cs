using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;
using Cinemachine;

public class RunGame : MonoBehaviour {

	public event System.Action OnChangeRotate;
	public event System.Action OnBeats;

	public AudioClip palsSound;
	public AudioClip rotateSound;
	public AudioClip startSound;


	public AudioClip successSound;
	public AudioClip faildSound;


	public AudioSource palsSource;
	public AudioSource seSource;
	public AudioSource bgmSource
	{
		get
		{
			return field.bgmSource;
		}
	}

	static RunGame _instance;
	static public RunGame instance
	{
		get
		{
			return _instance;
		}
	}

	public UICombo comboEffect;

	public GameObject mainCharacter;

	public GameObject deathEffect;

	//現在居る位置
	public Vector2Int nowPosition;

	//前回に居た位置
	private Vector2Int prevPosition;

	//次に行く位置
	private Vector2Int nextPosition;

	[SerializeField]
	private Field _field;
	public Field field {
		get {
			if (_field == null) {
				if (StageSelectScene.fieldPrefab != null) {
					var instance = Instantiate (StageSelectScene.fieldPrefab);
					_field = instance.GetComponent<Field>();
				}
			}
			return _field;
		}
	}

	public GameObject prevStartUI;

	public echoLight echoLiteInstance;

	public GameObject goalObject;
	public PlayableDirector timeline;

	public CinemachineBrain cinemachineBrain;

	[HideInInspector]
	public Vector2Int speed;

	private float startTime;

	public Text remainTimeText;

	private float periodTime {
		get {
			return field.periodTime;
		}
	}

	//始まってから何秒経過したか
	private float durationTime
	{
		get {
			return bgmSourceTime - startTime;
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

	bool isGoal = false;
	bool isStart = false;
	bool isDeath = false;

	private float tapTime;
	private Vector3 tapPosition;

	[SerializeField]
	private float flickEnableWidth = 100f;

	[SerializeField]
	private float flickEnableTime = 0.11f;

	[SerializeField]
	private float flickThreshold = 1f;

	[SerializeField]
	private float successTimeDuration = 0.2f;


	private int lastFlickBeat = 0;

	void Awake()
	{
		_instance = this;
	}

	float bgmSourceTime
	{
		get
		{
			return bgmSource.time + field.beatOffset;
		}
	}

	// Use this for initialization
	void Start () {
		nowPosition = field.startPosition;
		mainCharacter.transform.position = new Vector3 (nowPosition.x, 1f, nowPosition.y);
		GetStartNextPosition ();

		echoLiteInstance.periodTime = periodTime;

		goalObject.transform.SetParent (field.goalObject.transform, false);
	}

	IEnumerator StartComboEffect(int combo) {
		comboEffect.gameObject.SetActive (true);
		comboEffect.comboText.text = string.Format ("{0}", combo);
		yield return new WaitForSeconds (0.3f);

		comboEffect.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isGoal) {
			return;
		}

		remainTimeText.text = string.Format ("{0:0.00}", field.stageTimeLimit - durationTime).Replace (".", ":");

		if (!isStart && Input.GetMouseButtonDown(0)) {
			prevStartUI.gameObject.SetActive (false);
			isStart = true;

			bgmSource.Play();
			startTime = 0;

			seSource.clip = startSound;
			seSource.Play ();
			return;
		}

		if (prevBeat != Mathf.FloorToInt (beatCount))
		{

			//次のビートに行っている
			prevBeat = Mathf.FloorToInt (beatCount);
			Vector2Int diff = nextPosition - nowPosition;
			prevPosition = nowPosition;
			nowPosition = nextPosition;
			nextPosition = nowPosition + diff;//そのまま進み続ける

			Debug.Log ("beet!");
			palsSource.clip = palsSound;
			palsSource.Play ();

			OnBeats ();

			var parts = field.GetParts (nowPosition.x, nowPosition.y);
			if (parts == null || !parts.enable) {
				StartCoroutine (ShowDeathEffect());
			}

			if (nowPosition == field.goalPosition) {
				StartCoroutine (GoalEffect ());
			}
		}

		if (!isDeath && field.stageTimeLimit <= durationTime) {
			StartCoroutine (ShowDeathEffect());
		}


		//タップした瞬間
		if (Input.GetMouseButtonDown (0)) {
			//タップした処理
			tapTime = bgmSourceTime;
			tapPosition = Input.mousePosition;
			Debug.Log ("tap");

			float modTime = durationTime % periodTime;
			if (!(successTimeDuration <= modTime && modTime <= periodTime - successTimeDuration)) {
				Debug.Log ("Success" + modTime);

				seSource.clip = successSound;
				seSource.Play ();

				echoLiteInstance.addCombo ();
				echoLiteInstance.Play ();

				StartCoroutine (StartComboEffect(echoLiteInstance.combo));
			} else {
				echoLiteInstance.comboReset ();

				seSource.clip = faildSound;
				seSource.Play ();
			}

		}

		//フリック処理
		if (Input.GetMouseButtonUp (0)) {
			Vector3 diff = Input.mousePosition - tapPosition;
			float timeDiff = bgmSourceTime - tapTime;
			if (flickEnableWidth > Mathf.Abs (diff.x) 
				&& timeDiff < flickEnableTime
				&& diff.magnitude > flickThreshold
				&& lastFlickBeat != Mathf.FloorToInt(beatCount)) {

				lastFlickBeat = Mathf.FloorToInt (beatCount);
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

						seSource.clip = rotateSound;
						seSource.Play ();
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

						seSource.clip = rotateSound;
						seSource.Play ();
					}
				}
			}
		}
	}

	void FixedUpdate() {
		if (isGoal) {
			return;
		}

		//キャラの位置を合わせる
		if (prevBeat == Mathf.FloorToInt (beatCount)) {
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

		if (diff.x == 1 && diff.y == 0) {
			mainCharacter.transform.Rotate (0f, 0f, 0f);

		} else if (diff.x == -1 && diff.y == 0) {
			mainCharacter.transform.Rotate (0f, 180f, 0f);

		} else if (diff.x == 0 && diff.y == -1) {
			mainCharacter.transform.Rotate (0f, 90f, 0f);

		} else if (diff.x == 0 && diff.y == 1) {
			mainCharacter.transform.Rotate (0f, -90f, 0f);			
		}

		if (OnChangeRotate != null) {
			OnChangeRotate ();
		}
	}

	IEnumerator ShowDeathEffect()
	{
		if (isGoal) {
			yield break;
		}
		
		isDeath = true;
		deathEffect.SetActive (true);

		field.bgmSource.Stop ();

		yield return new WaitForSeconds (2);

		SceneManager.LoadScene("StageSelect");
	}

	IEnumerator GoalEffect()
	{
		isGoal = true;

		cinemachineBrain.enabled = true;
		timeline.gameObject.SetActive (true);
		yield return new WaitForSeconds((float)timeline.duration);

		SceneManager.LoadScene("lotteryClear");
		
	}
}
