using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public RawImage CanvasImage;
	public RectTransform BackgroundRect;


	public void SetTexture (Texture texture) {
		CanvasImage.texture = texture;

		var aspect = (float)texture.width / texture.height;

		if (Camera.main.aspect >= aspect) {
			BackgroundRect.sizeDelta = new Vector2(600 * aspect, 600);
		} else {
			var width = 600 * Camera.main.aspect;// * (600.0f / Screen.height);
			BackgroundRect.sizeDelta = new Vector2(width, width / aspect);
		}
	}
}
	