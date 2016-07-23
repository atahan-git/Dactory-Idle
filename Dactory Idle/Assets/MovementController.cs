﻿using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	Vector2?[] oldTouchPositions = {
		null,
		null
	};
	Vector2 oldTouchVector;
	float oldTouchDistance;

	ItemPlacer it;
	Transform myCam;

	public float minZoom = 6f;
	public float maxZoom = 15f;

	// Use this for initialization
	void Start () {
		it = GetComponent<ItemPlacer> ();
		myCam = Camera.main.gameObject.transform;
	}

	int defZero = 0;
	
	// Update is called once per frame
	void Update () {
		if (!it.isMovementEnabled)
			return;
		if (it.isPlacingItem) {
			defZero = 1;
		} else {
			defZero = 0;
		}


		if (Input.touchCount == defZero) {
			oldTouchPositions[0] = null;
			oldTouchPositions[1] = null;
		}
		else if (Input.touchCount == defZero + 1) {
			if (oldTouchPositions[0] == null || oldTouchPositions[1] != null) {
				oldTouchPositions[0] = Input.GetTouch(defZero).position;
				oldTouchPositions[1] = null;
			}
			else {
				Vector2 newTouchPosition = Input.GetTouch(defZero).position;

				myCam.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] - newTouchPosition) * myCam.GetComponent<Camera>().orthographicSize / myCam.GetComponent<Camera>().pixelHeight * 2f));

				oldTouchPositions[0] = newTouchPosition;
			}
		}
		else {
			if (oldTouchPositions[1] == null) {
				oldTouchPositions[0] = Input.GetTouch(defZero).position;
				oldTouchPositions[1] = Input.GetTouch(defZero + 1).position;
				oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
				oldTouchDistance = oldTouchVector.magnitude;
			}
			else {
				Vector2 screen = new Vector2(myCam.GetComponent<Camera>().pixelWidth, myCam.GetComponent<Camera>().pixelHeight);

				Vector2[] newTouchPositions = {
					Input.GetTouch(defZero).position,
					Input.GetTouch(defZero + 1).position
				};
				Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
				float newTouchDistance = newTouchVector.magnitude;

				myCam.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * myCam.GetComponent<Camera>().orthographicSize / screen.y));
				//myCam.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
				myCam.GetComponent<Camera>().orthographicSize *= oldTouchDistance / newTouchDistance;
				myCam.GetComponent<Camera> ().orthographicSize = Mathf.Clamp (myCam.GetComponent<Camera> ().orthographicSize, minZoom, maxZoom);
				myCam.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * myCam.GetComponent<Camera>().orthographicSize / screen.y);

				oldTouchPositions[0] = newTouchPositions[0];
				oldTouchPositions[1] = newTouchPositions[1];
				oldTouchVector = newTouchVector;
				oldTouchDistance = newTouchDistance;
			}
		}
	}
}
