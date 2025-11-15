using Corelib.SUI;
using UnityEngine.Events;

namespace Ingame
{
    public static class WorldModelInspector
    {
        public static SUIElement Render(WorldModel worldModel)
        {
            if (worldModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Text("월드 ID", worldModel.WorldIdentifier ?? string.Empty)
                        .OnValueChanged(value => worldModel.WorldIdentifier = value ?? string.Empty)
                    + SEditorGUILayout.Int("활성 엔티티 수", worldModel.ActiveEntityCount)
                        .Min(0)
                        .OnValueChanged(value => worldModel.ActiveEntityCount = value)
                    + SEditorGUILayout.Float("타임 스케일", worldModel.TimeScale)
                        .Min(0f)
                        .OnValueChanged(value => worldModel.TimeScale = value)
                    + SEditorGUILayout.Toggle("일시 정지", worldModel.IsPaused)
                        .OnValueChanged(value => worldModel.IsPaused = value)
                );
        }

        public static SUIElement RenderGroup(WorldModel worldModel, bool fold, UnityAction<bool> setter)
        {
            if (worldModel == null) return SUIElement.Empty();

            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.FoldGroup("월드 모델", fold)
                        .OnValueChanged(setter)
                        .Content(
                            Render(worldModel)
                        )
                );
        }
    }
}
