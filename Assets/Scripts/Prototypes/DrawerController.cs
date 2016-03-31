using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawerController : MonoBehaviour, IPointerDownHandler, IDragHandler {
	public const int TextureSize = 64;

	public Vector2 Corner;

	RectTransform trans;
	RawImage rawImage;

	Texture2D texture;
	Vector2 size;

	Vector2 lastPixel;

	void Awake () {
		trans = GetComponent<RectTransform>();
		rawImage = GetComponent<RawImage>();
	}

	void Start () {
		texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBAFloat, false);
		texture.filterMode = FilterMode.Point;
		rawImage.texture = texture;

		size = trans.rect.size;
	}

	public void OnPointerDown (PointerEventData eventData) {
		var curPixel = 
			new Vector2(
				(eventData.position.x - Corner.x) / size.x * TextureSize,
				(eventData.position.y - Corner.y) / size.y * TextureSize);

		if (eventData.pointerId < 1) {
			Draw(curPixel.x, curPixel.y, Color.red);
			lastPixel = curPixel;
		} else {
			DrawLine(lastPixel.x, lastPixel.y, curPixel.x, curPixel.y, Color.red);
		}
	}

	public void OnDrag (PointerEventData eventData) {
		var curPixel = 
			new Vector2(
				(eventData.position.x - Corner.x) / size.x * TextureSize,
				(eventData.position.y - Corner.y) / size.y * TextureSize);

		DrawLine(lastPixel.x, lastPixel.y, curPixel.x, curPixel.y, Color.red);

		if (eventData.pointerId < 1)
			lastPixel = curPixel;
	}

	void Draw (float x, float y, Color color) {
		texture.SetPixel((int)x, (int)y, color);
		texture.Apply();
	}

	void DrawLine (float x1, float y1, float x2, float y2, Color col) {
		float dx, dy, s, xi, yi, x, y;

		dx = x2 - x1;
		dy = y2 - y1;

		s = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));

		xi = dx / s;
		yi = dy / s;

		x = x1;
		y = y1;

		texture.SetPixel((int)x1, (int)y1, col);

		for (var m = 0; m < s; m++) {
			x += xi;
			y += yi;
			texture.SetPixel((int)x, (int)y, col);
		}

		texture.Apply();
	}
}
