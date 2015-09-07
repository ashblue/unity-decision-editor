using UnityEngine;
using UnityEngine.UI;

namespace Adnc.Decision {
	public class DecisionTest : MonoBehaviour {
		[SerializeField] DecisionControllerBase ctrl;

		[Header("Inputs")]
		[SerializeField] InputField id;
		[SerializeField] Toggle newVal;

		public void SetDecision () {
			ctrl.SetDecision(id.text, newVal.isOn);
		}

		public void GetDecision () {
			Debug.LogFormat("ID {0}, Result {1}", id.text, ctrl.GetDecision(id.text));
		}
	}
}
