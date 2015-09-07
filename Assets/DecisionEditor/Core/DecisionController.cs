using UnityEngine;
using System.Collections.Generic;

namespace Adnc.Decision {
	public abstract class DecisionControllerBase : MonoBehaviour {
		[SerializeField] DecisionDatabase database;
		public Dictionary<string, DecisionBase> decisionDef = new Dictionary<string, DecisionBase>(); // Defenitions used in retrieving decision data
		public Dictionary<string, bool> decisions = new Dictionary<string, bool>(); // Actively made decisions

		public virtual void Awake () {
			Debug.Assert(database != null, "You must include a decision database for the DecisionController to fully load");
			foreach (DecisionBase d in database.decisions) {
				decisionDef[d.id] = d;
			}
		}

		public bool GetDecision (string id) {
			DecisionCheck(id);

			bool result;
			if (decisions.TryGetValue(id, out result)) {
				return result;
			} else {
				return decisionDef[id].defaultValue;
			}
		}

		public void SetDecision (string id, bool val) {
			DecisionCheck(id);
			decisions[id] = val;
		}

		void DecisionCheck (string id) {
			Debug.Assert(decisionDef.ContainsKey(id), 
			             string.Format("Decision ID '{0}' does not exist. Please fix.", id));
		}
	}
}
