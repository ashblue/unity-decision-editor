using UnityEngine;
using UnityEditor;

namespace Adnc.Decision {
	[CustomPropertyDrawer(typeof(EditDecisionDatabaseAttribute))]
	public class EditDecisionDatabaseDrawer : PropertyDrawer {
		public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label) {
			if (GUI.Button(position, prop.stringValue)) {
				Debug.Log("Setup database connection logic here");
			}
		}

	
	}
}
