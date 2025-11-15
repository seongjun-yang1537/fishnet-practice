using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class PlayerModelInspector
    {
        public static SUIElement Render(PlayerModel playerModel)
        {
            if (playerModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Text("표시 이름", playerModel.DisplayName ?? string.Empty)
                        .OnValueChanged(value => playerModel.DisplayName = value ?? string.Empty)
                    + SEditorGUILayout.Int("레벨", playerModel.Level)
                        .Min(0)
                        .OnValueChanged(value => playerModel.Level = value)
                    + SEditorGUILayout.Float("체력", playerModel.Health)
                        .Min(0f)
                        .OnValueChanged(value => playerModel.Health = value)
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.Header("세션")
                    + PlayerSessionDataInspector.Render(playerModel.SessionData)
                    + SEditorGUILayout.Separator()
                    + SEditorGUILayout.Header("월드")
                    + WorldModelInspector.Render(playerModel.World)
                );
        }

        public static SUIElement RenderGroup(PlayerModel playerModel, bool fold, UnityAction<bool> setter)
        {
            if (playerModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("플레이어 모델", fold)
                        .OnValueChanged(setter)
                        .Content(
                            Render(playerModel)
                        )
                );
        }
    }
}
