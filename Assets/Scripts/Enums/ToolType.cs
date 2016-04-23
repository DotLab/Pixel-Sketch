public enum ToolType {
	MoveTool = 0x00,

	SelectTool = 0x10,
	RectSelectTool = 0x11,
	EllipSelectTool = 0x12,
	PolySelectTool = 0x13,

	PaintTool = 0x20,
	PencilPaintTool = 0x21,
	BrushPaintTool = 0x22,

	EraserTool = 0x30,

	ShapeTool = 0x40,
	RectShapeTool = 0x41,
	EllipShapeTool = 0x42,
	PolyShapeTool = 0x43,
	LineShapeTool = 0x44,

	MagicTool = 0x50,
	MagicNewTool = 0x51,
	MagicAddTool = 0x52,
	MagicSubTool = 0x53,

	TransTool = 0x60,

	ToolFamilyMask = 0xF0,

	None = 0xFF
}