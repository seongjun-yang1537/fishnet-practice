using System;
using System.Collections.Generic;
using Corelib.SUI;
using UnityEditor;

namespace Ingame
{
    [CustomEditor(typeof(GameSessionSystem))]
    public class EditorGameSessionSystem : Editor
    {
        private GameSessionSystem targetGameSessionSystem;
        protected virtual void OnEnable()
        {
            targetGameSessionSystem = (GameSessionSystem)target;
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
                SEditorGUILayout.Label($"현재 세션 수: {targetGameSessionSystem.sessionDatas.Count}")
                + SEditorGUILayout.Label($"다음 플레이어 UID: {GameSessionSystem.sessionUIDCounter + 1}")
                + SEditorGUILayout.Space(6f)
                + SEditorGUILayout.Horizontal()
                .Content(
                    SGUILayout.FlexibleSpace()
                    + SEditorGUILayout.Button("Join")
                    .Width(80f)
                    .OnClick(OnButtonJoin)
                )
                + SEditorGUILayout.Space(6f)
                + RenderSessionList()
            );
        }

        private void OnButtonJoin()
        {
            PlayerSessionData sessionData = new();
            targetGameSessionSystem.JoinPlayer(sessionData);
        }

        private SUIElement RenderSessionList()
        {
            if (targetGameSessionSystem.sessionDatas.Count == 0)
            {
                return SEditorGUILayout.Label("활성화된 세션이 없습니다.");
            }

            SUIElement accumulatedSessions = SUIElement.Empty();
            foreach (KeyValuePair<uint, PlayerSessionData> sessionEntry in targetGameSessionSystem.sessionDatas)
            {
                accumulatedSessions = accumulatedSessions + RenderSessionItem(sessionEntry);
            }

            return SEditorGUILayout.Vertical()
            .Content(
                accumulatedSessions
            );
        }

        private SUIElement RenderSessionItem(KeyValuePair<uint, PlayerSessionData> sessionEntry)
        {
            PlayerSessionData playerSessionData = sessionEntry.Value;
            uint sessionUid = sessionEntry.Key;
            string joinedTimeText = playerSessionData.joinedTime == default
                ? "-"
                : playerSessionData.joinedTime.ToString("yyyy-MM-dd HH:mm:ss", null);

            return SEditorGUILayout.Horizontal("box")
            .Content(
                SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Label($"세션 UID: {sessionUid}")
                    + SEditorGUILayout.Label($"플레이어 UID: {playerSessionData.worldPlayerUID}")
                    + SEditorGUILayout.Label($"호스트 여부: {playerSessionData.isHost}")
                    + SEditorGUILayout.Label($"참여 시각: {joinedTimeText}")
                )
                + SGUILayout.FlexibleSpace()
                + SEditorGUILayout.Button("Leave")
                .Width(80f)
                .OnClick(() => OnClickLeavePlayer(sessionUid))
            );
        }

        private void OnClickLeavePlayer(uint sessionUid)
        {
            targetGameSessionSystem.LeavePlayer(sessionUid);
        }
    }
}
