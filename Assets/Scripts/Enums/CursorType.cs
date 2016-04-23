public enum CursorType {
	MoveCursor = 0x00,
	MoveObjectCursor = 0x01,
	MoveSelectionCursor = 0x02,
	MoveCutCursor = 0x03,

	CrossCursor = 0x10,

	ActionCursor = 0x20,
	DropperActionCursor = 0x21,
	PencilActionCursor = 0x22,
	EraserActionCursor = 0x23,
	BrushActionCursor = 0x24,

	MagicCursor = 0x30,
	MagicNewCursor = 0x31,
	MagicAddCursor = 0x32,
	MagicSubCursor = 0x33,

	Transparent = 0x40,

	CursorFamilyMask = 0xF0,
}