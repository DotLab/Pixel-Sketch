using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CanvasController : MonoBehaviour {
	public RawImage Image;

	void OnValidate () {
		if (Image == null) Image = GetComponent<RawImage>();
	}

	public void SetTexture (Texture texture) {
		if (Image.texture != null) DestroyImmediate(Image.texture);
		Image.texture = texture;
	}
}
	