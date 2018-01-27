using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {

	public GameObject blockPrefab;

	public AudioSource bgmSource;

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
				if (GetParts(x, z).enable) {
					InstatiateBlock (x, z);
				}
			}
		}
	}

	void Start() {
		Refresh ();
	}

	private void InstatiateBlock(int x, int z) {
		GameObject gameObject =  Instantiate (blockPrefab);
		gameObject.transform.SetParent (transform, false);
		gameObject.transform.position = new Vector3 (x, 0, z);
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
