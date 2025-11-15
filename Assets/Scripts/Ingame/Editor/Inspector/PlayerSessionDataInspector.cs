using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PlayerSessionDataInspector
    {
        public static SUIElement Render(PlayerSessionData playerSessionData)
        {
            if (playerSessionData == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Text("세션 ID", playerSessionData.SessionIdentifier ?? string.Empty)
                        .OnValueChanged(value => playerSessionData.SessionIdentifier = value ?? string.Empty)
                    + SEditorGUILayout.Text("플레이어 이름", playerSessionData.PlayerName ?? string.Empty)
                        .OnValueChanged(value => playerSessionData.PlayerName = value ?? string.Empty)
                    + SEditorGUILayout.Float("플레이 시간(초)", playerSessionData.AccumulatedPlayTime)
                        .Min(0f)
                        .OnValueChanged(value => playerSessionData.AccumulatedPlayTime = value)
                    + SEditorGUILayout.Toggle("접속 중", playerSessionData.IsConnected)
                        .OnValueChanged(value => playerSessionData.IsConnected = value)
                );
        }

        public static SUIElement RenderGroup(PlayerSessionData playerSessionData, bool fold, UnityAction<bool> setter)
        {
            if (playerSessionData == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("플레이어 세션 데이터", fold)
                        .OnValueChanged(setter)
                        .Content(
                            Render(playerSessionData)
                        )
                );
        }
    }
}
