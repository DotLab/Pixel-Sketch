using UnityEngine;

using UnityEngine.UI;

public class SelectionController : MonoBehaviour {
	public static Selection Selection { get { return selection; } }

	static readonly Selection selection = new Selection();

	public RawImage SelectionImage;
	Texture2D texture;

	void Update () {
		if (selection.UiDirtyFlag) RenderSelection();
	}

	public void RenderSelection () {
		var size = DrawingScheduler.DrawingSize;

		if (texture == null || texture.width != size.x || texture.height != size.y) {
			if (texture != null) Object.DestroyImmediate(texture);
			texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
			texture.filterMode = FilterMode.Point;
			texture.hideFlags = HideFlags.DontSave;

			SelectionImage.texture = texture;
		}

		var rotation = Quaternion.Euler(0, 0, -selection.Rotation);
		var pixels = new Color[size.x * size.y];
		int i = 0;
		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.x; x++) {
				var coordinate = new Vector3(x, y);
				coordinate -= selection.Position;
				coordinate = rotation * coordinate;
				coordinate /= selection.Scale;
				coordinate += selection.Pivotal;
				var originalCoordinate = new Short2(coordinate.x + 0.5f, coordinate.y + 0.5f);

				if (selection.Content.ContainsKey(originalCoordinate))
					pixels[i] = selection.Content[originalCoordinate];

				i++;
			}
		}

		texture.SetPixels(pixels);
		texture.Apply();
	}
}
