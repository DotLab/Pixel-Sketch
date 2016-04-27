using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public Rect Rect { get { return CanvasRect.rect; } }

	public RawImage CanvasImage;
	public RectTransform CanvasRect;


	public void SetTexture (Texture texture) {
		CanvasImage.texture = texture;

		var aspect = (float)texture.width / texture.height;

		if (DrawingScheduler.AspectRatio >= aspect)
			CanvasRect.sizeDelta = new Vector2(
				DrawingScheduler.UiHeight * aspect,
				DrawingScheduler.UiHeight);
		else
			CanvasRect.sizeDelta = new Vector2(
				DrawingScheduler.UiWidth,
				DrawingScheduler.UiWidth / aspect);
	}
}
	