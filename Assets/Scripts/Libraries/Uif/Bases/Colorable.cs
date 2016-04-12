using UnityEngine;

namespace Uif {
	public abstract class Colorable : MonoBehaviour, IColorable {
		public abstract Color GetColor ();

		public abstract void SetColor (Color newColor);
	}
}
