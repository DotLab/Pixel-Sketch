using UnityEngine;

using System.Collections;

using Uif;

public class CursorController : MonoBehaviour {
	[System.Serializable]
	public class CursorConfig {
		public CursorType CursorType;
		public Sprite Sprite;
	}

	public delegate void OnCursorStateChanged (Vector2 position);

	public delegate void OnCursorMoved (Vector2 deltaPosition);

	public event OnCursorStateChanged OnCursorClickedEvent;
	public event OnCursorStateChanged OnCursorPressedEvent;
	public event OnCursorMoved OnCursorMovedEvent;
	public event OnCursorStateChanged OnCursorReleasedEvent;

	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	[Space]
	public TouchHandler TouchHandler;
	public RectTransform CursorTransform;
	public SpriteSwapable CursorSprite;

	[Space]
	public ColorSwapable CursorColor;
	public Color NormalColor = Color.white;
	public Color PressedColor = Color.cyan;

	[Space]
	public CursorConfig[] CursorConfigs;


	void OnValidate () {
		if (CursorTransform == null) CursorTransform = GetComponent<RectTransform>();
		if (CursorSprite == null) CursorSprite = GetComponent<SpriteSwapable>();
	}

	void Awake () {
		TouchHandler.OnMainTouchTapedEvent += ClickCursor;
//		TouchHandler.OnMainTouchPressedEvent += OnMainTouchPressed;
		TouchHandler.OnMainTouchDragedEvent += MoveCursor;
//		TouchHandler.OnMainTouchReleasedEvent += OnMainTouchReleased;

		TouchHandler.OnAsistTouchTapedEvent += ClickCursor;
		TouchHandler.OnAsistTouchPressedEvent += PressCursor;
		TouchHandler.OnAsistTouchDragedEvent += NudgeCursor;
		TouchHandler.OnAsistTouchReleasedEvent += ReleaseCursor;
	}


	//	#region TouchHandler Event Listener
	//
	//	public void OnMainTouchTaped (Vector2 position) {
	//		if (OnCursorClickedEvent != null) OnCursorClickedEvent(position);
	//	}
	//
	//	public void OnMainTouchPressed (Vector2 position) {
	//
	//	}
	//
	//	public void OnMainTouchDraged (Vector2 deltaPosition) {
	//		MoveCursor(deltaPosition);
	//	}
	//
	//	public void OnMainTouchReleased (Vector2 position) {
	//
	//	}
	//
	//	public void OnAsistTouchTaped (Vector2 position) {
	//		if (OnCursorClickedEvent != null) OnCursorClickedEvent(position);
	//	}
	//
	//	public void OnAsistTouchPressed (Vector2 position) {
	//		if (OnCursorPressedEvent != null) OnCursorPressedEvent(position);
	//	}
	//
	//	public void OnAsistTouchDraged (Vector2 deltaPosition) {
	//		MoveCursor(deltaPosition / 10);
	//	}
	//
	//	public void OnAsistTouchReleased (Vector2 position) {
	//		if (OnCursorReleasedEvent != null) OnCursorReleasedEvent(position);
	//	}
	//
	//	#endregion

	void Start () {
		SetCursorType(CursorType.PencilActionCursor);
	}

	public void SetCursorType (CursorType cursorType) {
		var config = GetCursorConfig(cursorType);
		StopAllCoroutines();
		StartCoroutine(SetPivotHandler(CursorTransform.pivot, config.Sprite.pivot / config.Sprite.rect.height));
		CursorSprite.Swap(config.Sprite);
	}

	public Vector2 GetCursorPosition () {
		return CursorTransform.anchoredPosition;
	}

	public void ClickCursor (Vector2 position) {
		DebugConsole.Log("Click Cursor " + GetCursorPosition());
		if (OnCursorClickedEvent != null) OnCursorClickedEvent(GetCursorPosition());
	}

	public void PressCursor (Vector2 position) {
		CursorColor.Swap(PressedColor);

		DebugConsole.Log("Press Cursor " + GetCursorPosition());
		if (OnCursorPressedEvent != null) OnCursorPressedEvent(GetCursorPosition());
	}

	public void MoveCursor (Vector2 deltaPosition) {
		CursorTransform.anchoredPosition += deltaPosition;

		DebugConsole.Log("Move Cursor " + deltaPosition);
		if (OnCursorMovedEvent != null) OnCursorMovedEvent(deltaPosition);
	}

	public void NudgeCursor (Vector2 deltaPosition) {
		MoveCursor(deltaPosition / 10);
	}

	public void ReleaseCursor (Vector2 position) {
		CursorColor.Swap(NormalColor);

		DebugConsole.Log("Release Cursor " + GetCursorPosition());
		if (OnCursorReleasedEvent != null) OnCursorReleasedEvent(GetCursorPosition());
	}

	CursorConfig GetCursorConfig (CursorType cursorType) {
		foreach (var cursorConfig in CursorConfigs) {
			if (cursorConfig.CursorType == cursorType) return cursorConfig;
		}

		throw new System.NotImplementedException();
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
}
