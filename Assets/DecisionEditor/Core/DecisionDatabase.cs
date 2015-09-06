using UnityEngine;
using System.Collections.Generic;

namespace Adnc.Decision {
	public class DecisionDatabase : ScriptableObject {
		public string title;

		[TextArea(3, 5)]
		public string description;

		[HideInInspector] public List<DecisionBase> decisions;

		[EditDecisionDatabase]
		public string editDatabase = "Edit Decision Database";
	}
}
