using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public RawImage CanvasImage;

	public RectTransform CanvasRect;
	public RectTransform BackgroundRect;


	public void SetTexture (Texture texture) {
		CanvasImage.texture = texture;

		var aspect = (float)texture.width / texture.height;

		if (Camera.main.aspect >= aspect) {
			CanvasRect.sizeDelta = new Vector2(600 * aspect, 600);
		} else {
			var width = 600 * Camera.main.aspect;// * (600.0f / Screen.height);
			CanvasRect.sizeDelta = new Vector2(width, width / aspect);
		}

		BackgroundRect.sizeDelta = CanvasRect.sizeDelta;
	}
}
	