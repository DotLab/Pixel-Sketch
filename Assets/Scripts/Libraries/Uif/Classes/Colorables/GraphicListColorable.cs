using UnityEngine;
using UnityEngine.UI;

namespace Uif {
	[AddComponentMenu("Uif/Colorable/Graphic List Colorable")]
	public class GraphicListColorable : Colorable {
		public Color Color = Color.white;
	
		public Graphic[] graphics;


		void Awake () {
			SetColor(Color);
		}

		public override Color GetColor () {
			return Color;
		}

		public override void SetColor (Color newColor) {
			foreach (var graphic in graphics) {
				graphic.color = newColor;
			}

			Color = newColor;
		}
	}
}