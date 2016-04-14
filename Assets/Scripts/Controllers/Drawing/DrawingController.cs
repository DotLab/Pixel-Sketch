using UnityEngine;

using System.Collections.Generic;

public class DrawingController : MonoBehaviour {
	public CursorController Cursor;

	public ToolBarController ToolBar;

	public ColorTabController ColorTab;
	public LayerTabController LayerTab;

	public CanvasController Canvas;


	void Awake () {
		Cursor.OnCursorMovedEvent += position => DebugConsole.Log("OnCursorMovedEvent " + position);
		Cursor.OnCursorPressedEvent += position => DebugConsole.Log("OnCursorPressedEvent " + position);
		Cursor.OnCursorReleasedEvent += position => DebugConsole.Log("OnCursorReleasedEvent " + position);

		ToolBar.OnToolChangedEvent += newToolType => DebugConsole.Log("OnToolChangedEvent " + newToolType);

		ColorTab.OnColorChangedEvent += newColor => DebugConsole.Log("OnColorChangedEvent " + newColor);

		LayerTab.OnLayerAddEvent += layer => DebugConsole.Log("OnLayerAddEvent " + layer.Index);
		LayerTab.OnLayerDeleteEvent += layer => DebugConsole.Log("OnLayerDeleteEvent " + layer.Index);
		LayerTab.OnLayerSelectedEvent += layer => DebugConsole.Log("OnLayerSelectedEvent " + layer.Index);
		LayerTab.OnLayerOrderChangedEvent += layers => DebugConsole.Log("OnLayerOrderChangedEvent " + layers.Length);
		LayerTab.OnLayerHideStateChangedEvent += layer => DebugConsole.Log("OnLayerHideStateChangedEvent " + layer.Index);
		LayerTab.OnLayerLockStateChangedEvent += layer => DebugConsole.Log("OnLayerLockStateChangedEvent " + layer.Index);
	
		LayerTab.OnLayerAddEvent += Canvas.OnLayerAdded;
		LayerTab.OnLayerDeleteEvent += Canvas.OnLayerDeleted;
		LayerTab.OnLayerSelectedEvent += Canvas.OnLayerSelected;
		LayerTab.OnLayerOrderChangedEvent += Canvas.OnLayerOrderChanged;
		LayerTab.OnLayerHideStateChangedEvent += Canvas.OnLayerHideStateChanged;

		Cursor.OnCursorMovedEvent += Canvas.Draw;
		ColorTab.OnColorChangedEvent += Canvas.SetColor;
	}
}
