using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(GameSessionSystem))]
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
                    .OnClick(OnButtonJoin)
                )
            );
        }

        private void OnButtonJoin()
        {
            PlayerSessionData sessionData = new();
            script.JoinPlayer(sessionData);
        }
    }
}