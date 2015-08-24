using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour
{
	private LineRenderer lineRenderer; //対象のLineRendererコンポーネント

	private bool drawEnable = true; //描画可能状態
	private bool isButtonDown = true; //左クリック押されてるかどうか
	private bool hitEnable = true; //当たり判定の状態

	private float maxDistance = 2.5f; //線の最高の長さ
	private float currentDistance = 0.0f; //現在の線の長さ

	private float deltaTime = 0.0f; //経過時間
	private float drawInterval = 0.0333f; //描画のインターバル

	public int ForceFactor = 30; //ボールに与える力

	private List<Vector3> positions;

	//ゲームスタート時に実行
	void Start ()
	{
		positions = new List<Vector3> ();
		
		Vector3 drawPoint = getDrawPoint ();

		lineRenderer = gameObject.GetComponent<LineRenderer> ();
		lineRenderer.SetVertexCount (1);
		lineRenderer.SetPosition (0, drawPoint);
		positions.Add (drawPoint);
	}
	
	//毎フレーム実行
	void Update ()
	{
		if (Input.GetMouseButton (0) && drawEnable && isButtonDown) {
			drawEnable = false;
			setDrawPosition ();
			onDrawLine ();
		}
		if (Input.GetMouseButtonUp (0)) {
			isButtonDown = false;
		}
		
		deltaTime += Time.deltaTime;
		if (deltaTime > drawInterval) {
			deltaTime = 0.0f;
			drawEnable = true;
		}else if (Vector3.Distance (positions [positions.Count - 1], getDrawPoint ()) > maxDistance / 20) {
			deltaTime = 0.0f;
			drawEnable = true;
		}

		if (positions.Count >= 2 && hitEnable) {
			collisionDetection ();
		}
	}

	//線を結ぶ点の位置をpositionsに追加
	private void setDrawPosition ()
	{
		if (maxDistance > currentDistance + Vector3.Distance (positions [positions.Count - 1], getDrawPoint ())) {
			Vector3 drawPoint = getDrawPoint ();
			positions.Add (drawPoint);
		} else {
			float diff = currentDistance + Vector3.Distance (positions [positions.Count - 1], getDrawPoint ()) - maxDistance;
			float temp = 0.0f;
			int count = 0;
			for (int i = 0; i < positions.Count - 1; i++) {
				temp += Vector3.Distance (positions [i], positions [i + 1]);
				if (diff <= temp) {
					break;
				}
				count++;
			}
			positions.RemoveRange (0, count);
			Vector3 drawPoint = getDrawPoint ();
			positions.Add (drawPoint);
		}
	}

	//線を描画
	private void onDrawLine ()
	{
		lineRenderer.SetVertexCount (positions.Count);
		for (int i = 0; i < positions.Count; i++) {
			lineRenderer.SetPosition (i, positions [i]);
		}
		currentDistance = 0.0f;
		for (int i = 0; i < positions.Count - 1; i++) {
			currentDistance += Vector3.Distance (positions [i], positions [i + 1]);
		}
	}

	//スクリーン座標（マウス座標）をワールド座標に変換
	private Vector3 getDrawPoint ()
	{
		Vector3 screenPoint = Input.mousePosition;
		screenPoint.z = 10.0f;
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint (screenPoint);
		return worldPoint;
	}

	//当たり判定処理
	private void collisionDetection ()
	{
		Vector2 startPoint = new Vector2 (positions [0].x, positions [0].y);
		Vector2 endPoint = new Vector2 (positions [positions.Count - 1].x, positions [positions.Count - 1].y);
		RaycastHit2D hit = Physics2D.Linecast (startPoint, endPoint);

		Vector2 vec = Vector2.Max (endPoint, startPoint) - Vector2.Min (endPoint, startPoint);
		Vector2 force = new Vector2 (-vec.y, vec.x);

		if (hit) {
			string layerName = LayerMask.LayerToName (hit.collider.gameObject.layer);
			if (layerName == "Ball") {
				Rigidbody2D rigidBody = hit.collider.gameObject.GetComponent<Rigidbody2D> ();
				rigidBody.AddForce ((force.normalized * ForceFactor) * rigidBody.mass / Time.fixedDeltaTime);
				hitEnable = false;
				Stage.GetInstance ().Backgrounds.OnUpMove ();
				OnDestroy ();
			}
		}
	}

	//自身を削除
	public void OnDestroy ()
	{
		Destroy (gameObject);
	}
}
