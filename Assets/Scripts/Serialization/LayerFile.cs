using UnityEngine;

using System.Collections.Generic;

[System.Serializable]
public class LayerFile {
	[System.Serializable]
	public struct ColorPair {
		public Short2 k;
		public Color c;

		public ColorPair (Short2 k, Color c) {
			this.k = k;
			this.c = c;
		}
	}

	public int Index;
	public bool Hided;
	public bool Locked;

	public Dictionary<Short2, Color> Content;

	public LayerFile () {
	}
}