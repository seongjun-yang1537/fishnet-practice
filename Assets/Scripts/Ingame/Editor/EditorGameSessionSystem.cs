using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    public class EditorGameSessionSystem : Editor
    {
        GameSessionSystem script;
        protected virtual void OnEnable()
        {
            script = (GameSessionSystem)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUILayout.Vertical()
            .Content(
                RenderSessionController()
            )
            .Render();
        }

        private SUIElement RenderSessionController()
        {
            return SEditorGUILayout.Vertical("helpbox")
            .Content(
                SEditorGUILayout.Label($"Maybe Next Player UID: {GameSessionSystem.sessionUIDCounter + 1}")
                + SEditorGUILayout.Horizontal()
                .Content(
                    SEditorGUILayout.Button("Join")
                )
            );
        }
    }
}