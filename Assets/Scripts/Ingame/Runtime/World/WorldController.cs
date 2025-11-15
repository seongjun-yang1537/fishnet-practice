using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    [RequireComponent(typeof(WorldScope))]
    public class WorldController : MonoBehaviour
    {
        protected UnityEvent<WorldModel> onBuildModel = new();

        [Inject]
        private LifetimeScope scope;

        [Inject]
        [field: SerializeField]
        public WorldModel worldModel { get; private set; }

        [Required]
        public Transform trEntities;
        [Required]
        public GameObject prefabPlayer;

        protected virtual void Awake()
        {
            BuildModel();
        }

        public void BuildModel(WorldModel newWorldModel = null)
        {
            if (newWorldModel == null)
                newWorldModel = new WorldModel();

            worldModel = newWorldModel;
            onBuildModel.Invoke(worldModel);
        }

        private void OnBuildModel(WorldModel worldModel)
        {

        }

        protected virtual void OnEnable()
        {
            onBuildModel.AddListener(OnBuildModel);
        }

        protected virtual void OnDisable()
        {
            onBuildModel.RemoveListener(OnBuildModel);
        }

        public PlayerController CreatePlayer()
        {
            LifetimeScope playerScope = prefabPlayer.GetComponent<PlayerScope>();
            GameObject go = scope.CreateChildFromPrefab(playerScope).gameObject;

            PlayerController pc = go.GetComponent<PlayerController>();
            return pc;
        }

        public PlayerController SpawnPlayer(PlayerController pc = null)
        {
            if (pc == null) pc = CreatePlayer();

            PlayerModel playerModel = pc.playerModel;
            playerModel.uid = worldModel.wid++;

            worldModel.AddPlayer(playerModel);

            return pc;
        }

        public bool DespawnPlayer(uint uid)
        {
            return worldModel.RemovePlayer(uid);
        }
        public bool DespawnPlayer(PlayerController pc)
            => DespawnPlayer(pc.playerModel.uid);
    }
}
