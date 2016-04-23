using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections.Generic;

public class SelectionController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public RawImage SelectionImage;
	public SelectionGridController SelectionGrid;

	public float Scale;
	public float Rotation;
	public Vector3 Pivotal;
	public Vector3 Position;

	public readonly Dictionary<Short2, Color> Content = new Dictionary<Short2, Color>();

	public Short2 size;
	Texture2D texture;

	public int MainTouchIndex;
	public bool MainTouchPressed;
	public Vector2 MainTouchPosition;

	public int AsistTouchIndex;
	public bool AsistTouchPressed;
	public Vector2 AsistTouchPosition;

	public float StartDistance;
	public Vector3 StartDirection;
	public Vector2 StartMidPoint;

	public float StartScale;
	public float StartRotation;
	public Vector2 StartPosition;

	void Start () {
		for (int x = 2; x <= 19; x++) {
			for (int y = 4; y <= 28; y++) {
				Content[new Short2(x, y)] = new Color((float)x / size.x, (float)y / size.y, 1);
				SelectionGrid.Selection.SetSelection(new Short2(x, y), true);
			}
		}

		CalcPivotal();
		SelectionGrid.Pivotal = Pivotal;
		SelectionGrid.Position = Position;
		SelectionGrid.BuildGrid();
	}

	void CalcPivotal () {
		var max = new Vector2(float.MinValue, float.MinValue);
		var min = new Vector2(float.MaxValue, float.MaxValue);

		foreach (var key in Content.Keys) {
			var v = new Vector2(key.x, key.y);
			max = Vector2.Max(max, v);
			min = Vector2.Min(min, v);
		}

		Pivotal = (min + max) * 0.5f;
		Position = Pivotal;
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (!MainTouchPressed) {
			MainTouchPressed = true;
			MainTouchIndex = eventData.pointerId;
			MainTouchPosition = eventData.position;
		} else if (!AsistTouchPressed) {
			AsistTouchPressed = true;
			AsistTouchIndex = eventData.pointerId;
			AsistTouchPosition = eventData.position;

			StartScale = Scale;
			StartRotation = Rotation;
			StartPosition = Position;

			StartDistance = Vector2.Distance(MainTouchPosition, AsistTouchPosition);
			StartDirection = AsistTouchPosition - MainTouchPosition;
			StartMidPoint = (AsistTouchPosition + MainTouchPosition) * 0.5f;
		}
	}

	public void OnDrag (PointerEventData eventData) {
		if (MainTouchPressed && eventData.pointerId == MainTouchIndex) {
			MainTouchPosition = eventData.position;
		
			if (!AsistTouchPressed)
				Position += (Vector3)eventData.delta * (size.y / 600.0f);
			SelectionGrid.Position = Position;
		} else if (AsistTouchPressed && eventData.pointerId == AsistTouchIndex) {
			AsistTouchPosition = eventData.position;
		}

		if (MainTouchPressed && AsistTouchPressed) {
			var currentDistance = Vector2.Distance(MainTouchPosition, AsistTouchPosition);
			var currentDirection = AsistTouchPosition - MainTouchPosition;
			var currentMidPoint = (AsistTouchPosition + MainTouchPosition) * 0.5f;

			Scale = StartScale * currentDistance / StartDistance;
			Rotation = StartRotation + (
			    Mathf.Atan2(currentDirection.y, currentDirection.x) - Mathf.Atan2(StartDirection.y, StartDirection.x)
			) * Mathf.Rad2Deg;
			Position = StartPosition + (currentMidPoint - StartMidPoint) * (size.y / 600.0f);

			SelectionGrid.Scale = Scale;
			SelectionGrid.Rotation = Rotation;
			SelectionGrid.Position = Position;
		}
	}

	public void OnEndDrag (PointerEventData eventData) {
		if (eventData.pointerId == MainTouchIndex) {
			MainTouchPressed = false;
		} else if (eventData.pointerId == AsistTouchIndex) {
			AsistTouchPressed = false;
		}
	}

	void Update () {
		if (texture == null || texture.width != size.x || texture.height != size.y) {
			if (texture != null) Object.DestroyImmediate(texture);
			texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
			texture.filterMode = FilterMode.Point;
			texture.hideFlags = HideFlags.DontSave;
			SelectionImage.texture = texture;
		}

//		var rotation = Quaternion.Euler(0, 0, Rotation);
//		var pixels = new Color[size.x * size.y];
//		foreach (var key in Content.Keys) {
//			var coordinate = new Vector3(key.x, key.y);
//			coordinate -= Pivotal;
//			coordinate *= Scale;
//			coordinate = rotation * coordinate;
//			coordinate += Position;
//
//			var newKey = new Short2(coordinate.x + 0.5f, coordinate.y + 0.5f);
//			if (IsIllegal(newKey)) continue;
//
//			pixels[newKey.x + newKey.y * size.x] = Content[key];
//		}

		var rotation = Quaternion.Euler(0, 0, -Rotation);
		var pixels = new Color[size.x * size.y];
		int i = 0;
		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.x; x++) {
				var coordinate = new Vector3(x, y);
				coordinate -= Position;
				coordinate = rotation * coordinate;
				coordinate /= Scale;
				coordinate += Pivotal;
				var originalCoordinate = new Short2(coordinate.x + 0.5f, coordinate.y + 0.5f);

				if (Content.ContainsKey(originalCoordinate)) pixels[i] = Content[originalCoordinate];

				i++;
			}
		}

		texture.SetPixels(pixels);
		texture.Apply();
	}

	bool IsIllegal (Short2 c) {
		return c.x < 0 || c.x >= size.x || c.y < 0 || c.y >= size.y;
	}
}
