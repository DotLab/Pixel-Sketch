using UnityEngine;

namespace Uif {
	public interface IColorable {
		Color GetColor ();

		void SetColor (Color newColor);
	}
}