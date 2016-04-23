using UnityEngine;

using System.Collections;

using Uif;

public class CursorController : MonoBehaviour {
	[System.Serializable]
	public class CursorConfig {
		public CursorType CursorType;
		public Sprite Sprite;
	}

	public const float MaxClickInterval = 0.2f;

	public delegate void OnPositionChanged (Vector2 position);

	public event OnPositionChanged OnCursorMovedEvent;
	public event OnPositionChanged OnCursorDraggedEvent;
	public event OnPositionChanged OnCursorClickedEvent;

	public bool Active;

	[Space]
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public RectTransform CursorTransform;
	public SpriteSwapable CursorSprite;

	[Space]
	public ColorSwapable CursorColor;
	public Color NormalColor = Color.white;
	public Color PressedColor = Color.cyan;

	[Space]
	public Sprite TransparentSprite;
	public CursorConfig[] CursorConfigs;

	RectTransform trans;
	CursorConfig currentConfig;

	bool pressed;
	public bool Dragged;
	float pressStartTime;
	public Vector2 LastPosition;


	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	#region DrawingSceneScheduler

	public void HideCursor () {
		Active = false;

		currentConfig = null;
		CursorSprite.Swap(TransparentSprite);
	}

	public void SetCursorType (CursorType cursorType) {
		Active = true;

		var config = FindCursorConfig(cursorType);
		SetCursorConfig(config);
	}

	CursorConfig FindCursorConfig (CursorType cursorType) {
		foreach (var cursorConfig in CursorConfigs) {
			if (cursorConfig.CursorType == cursorType) return cursorConfig;
		}

		throw new System.NotImplementedException();
	}

	void SetCursorConfig (CursorConfig cursorConfig) {
		if (cursorConfig == currentConfig) return;
		currentConfig = cursorConfig;

		StopAllCoroutines();
		StartCoroutine(SetPivotHandler(CursorTransform.pivot, cursorConfig.Sprite.pivot / cursorConfig.Sprite.rect.height));
		CursorSprite.Swap(cursorConfig.Sprite);
	}

	IEnumerator SetPivotHandler (Vector2 srcPivot, Vector2 dstPivot) {
		float time = 0;

		while (time < TransitionDuration) {
			var easedStep = Easing.EaseInOut(time / TransitionDuration, TransitionEasingType);

			CursorTransform.pivot = Vector2.Lerp(srcPivot, dstPivot, easedStep);

			time += Time.deltaTime;
			yield return null;
		}

		CursorTransform.pivot = dstPivot;
	}

	#endregion

	#region CursorTouchHandler

	public void MoveCursor (Vector2 delta) {
		if (!Active) return;

		var targetPosition = CursorTransform.anchoredPosition + delta;

		targetPosition.x = Clump(trans.rect.xMin, trans.rect.xMax, targetPosition.x);
		targetPosition.y = Clump(trans.rect.yMin, trans.rect.yMax, targetPosition.y);

		CursorTransform.anchoredPosition = targetPosition;

		if (pressed) {
			if (OnCursorDraggedEvent != null) OnCursorDraggedEvent(GetCursorPosition());
			Dragged = true;
		} else {
			if (OnCursorMovedEvent != null) OnCursorMovedEvent(GetCursorPosition());
		}

		LastPosition = GetCursorPosition();
	}

	public void NudgeCursor (Vector2 delta) {
		MoveCursor(delta * 0.4f);
	}

	public void PressCursor () {
		if (!Active) return;

		CursorColor.Swap(PressedColor);

		pressed = true;
		Dragged = false;
		pressStartTime = Time.time;

		LastPosition = GetCursorPosition();
	}

	public void ReleaseCursor () {
		if (!Active) return;

		CursorColor.Swap(NormalColor);

		pressed = false;
		if (!Dragged && Time.time - pressStartTime < MaxClickInterval)
		if (OnCursorClickedEvent != null) OnCursorClickedEvent(GetCursorPosition());
	}

	public Vector2 GetCursorPosition () {
		return CursorTransform.anchoredPosition;
	}

	static float Clump (float min, float max, float value) {
		return value < min ? min : value > max ? max : value;
	}

	#endregion
}
