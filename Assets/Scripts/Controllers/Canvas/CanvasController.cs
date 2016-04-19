﻿using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public RawImage CanvasImage;
	public RectTransform BackgroundRect;

	Material lineMaterial;

	void Awake () {
		// Unity has a built-in shader that is useful for drawing
		// simple colored things.
		var shader = Shader.Find("Hidden/Internal-Colored");
		lineMaterial = new Material(shader);
		lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		// Turn on alpha blending
		lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		// Turn backface culling off
		lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		// Turn off depth writes
		lineMaterial.SetInt("_ZWrite", 0);
	}

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

	void OnRenderObject () {
		// Apply the line material
		lineMaterial.SetPass(0);

		GL.PushMatrix();
		GL.MultMatrix(transform.localToWorldMatrix);
		GL.Begin(GL.LINES);

		GL.Color(Color.green);

//		GL.Vertex3(0, -300, 0);
//		GL.Vertex3(0, 300, 0);

		GL.End();
		GL.PopMatrix();
	}
}
	