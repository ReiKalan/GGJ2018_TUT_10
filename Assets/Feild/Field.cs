using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {

	public GameObject blockPrefab;
	public GameObject goalPrefab;
	public GameObject nortsPrefab;

	public AudioSource bgmSource;

	public Vector2Int startPosition;

	public Vector2Int goalPosition;

	[SerializeField]
	public float beatOffset = 0.27f;

	//
	public List<Parts> dataList = new List<Parts>();

	[System.Serializable]
	public class Parts {
		public bool enable;
	}

	public int width = 10;
	public int height = 10;

	public void Refresh() {
		ClearBlock ();

		for (int s = dataList.Count;s < width * height;++s) {
			dataList.Add (new Parts());
		}
		for (int x = 0	;x < width;++x) {
			for (int z = 0;z < height;++z) {
				if (GetParts (x, z).enable) {
					InstatiateBlock (x, z);
					InstatiateNotrtsForPartsAround (x, z);
				}
			}
		}


		//ゴールをつけます
		Vector2Int prevPartsPos = new Vector2Int();
		AroundLoop (goalPosition, v2 => {
			var parts = GetParts (v2.x, v2.y);
			if (parts != null && parts.enable) {
				prevPartsPos = v2;
			}
		});
		var gorlDirection = goalPosition - prevPartsPos;
		if (gorlDirection.x == 1 && gorlDirection.y == 0) {
			InstantiateGorl (goalPosition.x, goalPosition.y, -90);

		} else if (gorlDirection.x == -1 && gorlDirection.y == 0) {
			InstantiateGorl (goalPosition.x, goalPosition.y, 90);
			
		} else if (gorlDirection.x == 0 && gorlDirection.y == 1) {
			InstantiateGorl (goalPosition.x, goalPosition.y, 180);
			
		} else if (gorlDirection.x == 0 && gorlDirection.y == -1) {
			InstantiateGorl (goalPosition.x, goalPosition.y, 0);
			
		}
	}

	void Start() {
		Refresh ();
	}

	private void InstantiateGorl(int x, int z, float rotateY) {
		var goal = Instantiate (goalPrefab);
		goal.transform.SetParent (transform, false);
		goal.transform.position = new Vector3 (x, 0f, z);
		goal.transform.Rotate (0, rotateY, 0);
	}

	private void InstatiateBlock(int x, int z) {
		GameObject gameObject =  Instantiate (blockPrefab);
		gameObject.transform.SetParent (transform, false);
		gameObject.transform.position = new Vector3 (x, 0, z);
	}

	private void InstatiateNotrtsForPartsAround(int x, int z) {

		foreach (var item in aroundLoopList) {
			var sidePartsPos = new Vector2Int (x, z) + item;
			var parts = GetParts (sidePartsPos.x, sidePartsPos.y);
			if (parts != null && parts.enable) {

				if (item.x == 1 && item.y == 0) {
					InstatiateNotrts (x + 0.5f, z, 0);

				} else if (item.x == -1 && item.y == 0) {
					InstatiateNotrts (x - 0.5f, z, 0);

				} else if (item.x == 0 && item.y == 1) {
					InstatiateNotrts (x, z + 0.5f, 90);

				} else if (item.x == 0 && item.y == -1) {
					InstatiateNotrts (x, z - 0.5f, 90);

				}
			}
		}
	}

	private void InstatiateNotrts(float x, float z, float rotateY) {
		GameObject gameObject =  Instantiate (nortsPrefab);
		gameObject.transform.SetParent (transform, false);
		gameObject.transform.position = new Vector3 (x, 0, z);
		gameObject.transform.Rotate (0, rotateY, 0);
	}

	private void ClearBlock() {
		while (transform.childCount != 0) {

			Transform t = transform.GetChild (0);
			DestroyImmediate (t.gameObject);
		}
	}

	public Parts GetParts(int x, int z) {
		if (!(0 <= x && x < width)) {
			return null;
		}
		if (!(0 <= z && z < height)) {
			return null;
		}

		Parts ret = dataList [x * height + z];
		if (ret == null) {
			dataList [x * height + z] = new Parts ();
			return dataList [x * height + z];
		}

		return ret;
	}

	private List<Vector2Int> aroundLoopList = new List<Vector2Int> {
		new Vector2Int(1, 0),
		new Vector2Int (0, 1),
		new Vector2Int (-1, 0),
		new Vector2Int (0, -1)
	};

	public void AroundLoop(Vector2Int pos, System.Action<Vector2Int> callBack)
	{
		foreach (var item in aroundLoopList) {
			callBack (item + pos);
		}
	}
}
