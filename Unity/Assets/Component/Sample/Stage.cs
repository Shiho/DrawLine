using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage : MonoBehaviour
{
	public Backgrounds Backgrounds;
	public Score Score;
	
	[SerializeField]
	private GameObject ballPrefab;
	private GameObject ballObject;
	
	[SerializeField]
	private GameObject startViewPrefab;
	private GameObject startViewObject;
	
	[SerializeField]
	private GameObject restartViewPrefab;
	private GameObject restartViewObject;

	[SerializeField]
	private GameObject linePrefab;
	private GameObject lineObject;
	private Line lineComponent;
	
	[SerializeField]
	private List<GameObject> waveList;
	private List<GameObject> waveObjList;
	private int currentWaveNum = 0;
	private bool isWaveGenerate = false;

	private bool isStart = false;

	private static Stage instance;
	public static Stage GetInstance ()
	{
		return instance;
	}

	void Awake ()
	{
		instance = this;
		waveObjList = new List<GameObject> ();
	}

	void Start ()
	{
		addBall ();
		addStartView ();
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (!isStart && ballObject) {
				ballObject.GetComponent<Rigidbody2D> ().WakeUp ();
				removeStartView ();
				removeRestartView ();
				isStart = true;
			}
			if (lineObject) {
				lineComponent.OnDestroy ();
			}
			lineObject = Instantiate (linePrefab, transform.position, transform.rotation) as GameObject;
			lineObject.transform.parent = transform;
			lineComponent = lineObject.GetComponent<Line> ();
		}
		if (!ballObject) {
			OnRestart ();
		}
	}

	private void addBall ()
	{
		if (!ballObject) {
			ballObject = Instantiate (ballPrefab, new Vector3 (0.0f, 5.0f, 0.0f), Quaternion.Euler (new Vector3 ())) as GameObject;
			ballObject.transform.parent = transform;
			ballObject.GetComponent<Rigidbody2D> ().Sleep ();
		}
	}

	private void addStartView ()
	{
		if (!startViewObject) {
			startViewObject = Instantiate (startViewPrefab, new Vector3 (), Quaternion.Euler (new Vector3 ())) as GameObject;
		}
	}

	private void removeStartView ()
	{
		if (startViewObject) {
			Destroy (startViewObject);
		}
	}

	private void addRestartView ()
	{
		if (!restartViewObject) {
			restartViewObject = Instantiate (restartViewPrefab, new Vector3 (), Quaternion.Euler (new Vector3 ())) as GameObject;
		}
	}

	private void removeRestartView ()
	{
		if (restartViewObject) {
			Destroy (restartViewObject);
		}
	}

	public void OnRestart ()
	{
		isStart = false;
		isWaveGenerate = false;
		currentWaveNum = 0;
		removeWave ();
		addBall ();
		addRestartView ();
		Score.Init ();
	}

	public void OnBlockGenerate ()
	{
		if (!isWaveGenerate) {
			isWaveGenerate = true;
			return;
		} else {
			isWaveGenerate = false;
		}
		int currentScore = Score.GetCurrentScore ();
		int num = (int)Mathf.Repeat (currentWaveNum, waveList.Count);
		if (num < 0) {
			return;
		}
		GameObject wave = Instantiate (waveList [num], new Vector3 (0.0f, 13.0f, 0.0f), Quaternion.Euler (new Vector3 ())) as GameObject;
		wave.transform.parent = transform;
		waveObjList.Add (wave);
		currentWaveNum++;
	}

	public void OnWaveMove ()
	{
		for (int i = 0; i < waveObjList.Count; i++) {
			if (waveObjList [i]) {
				waveObjList [i].GetComponent<Wave> ().OnMove ();
			}
		}
	}

	private void removeWave ()
	{
		for (int i = 0; i < waveObjList.Count; i++) {
			if (waveObjList [i]) {
				Destroy (waveObjList [i]);
			}
		}
		waveObjList = new List<GameObject> ();
	}
}
