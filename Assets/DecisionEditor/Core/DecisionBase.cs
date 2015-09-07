using UnityEngine;
using System.Collections;

namespace Adnc.Decision {
	[System.Serializable]
	public class DecisionBase {
		public string displayName = "Untitled";
		public string id = "";
		public bool defaultValue = false;

		// Editor only values
		public string notes = "";
		public bool expanded = true;
	}
}
