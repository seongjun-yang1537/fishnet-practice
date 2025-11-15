using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Corelib.Utils;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    [RequireComponent(typeof(WorldScope))]
    public class WorldController : Singleton<WorldController>
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

        private Dictionary<uint, PlayerController> playerControllers = new();

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
            foreach (var pc in playerControllers.Values)
                DestroyPlayer(pc);

            playerControllers.Clear();
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
            Transform tr = go.transform;
            tr.SetParent(trEntities);

            PlayerController pc = go.GetComponent<PlayerController>();
            return pc;
        }

        public PlayerController SpawnPlayer(PlayerController pc = null)
        {
            if (pc == null) pc = CreatePlayer();

            uint newUID = worldModel.wid++;
            PlayerModel playerModel = pc.playerModel;
            playerModel.uid = newUID;

            worldModel.AddPlayer(playerModel);
            playerControllers.Add(newUID, pc);

            return pc;
        }

        public bool DespawnPlayer(uint uid)
        {
            playerControllers.Remove(uid);
            return worldModel.RemovePlayer(uid);
        }
        public bool DespawnPlayer(PlayerController pc)
            => DespawnPlayer(pc.playerModel.uid);

        public bool DestroyPlayer(uint uid)
        {
            bool removed = worldModel.RemovePlayer(uid);
            if (!removed) return false;

            PlayerController pc = FindPlayerByUID(uid);
            if (pc == null) return true;

            LifetimeScope scope = pc.GetComponent<LifetimeScope>();
            if (scope != null)
                Destroy(scope.gameObject);
            else
                Destroy(pc.gameObject);

            return true;
        }

        public bool DestroyPlayer(PlayerController pc)
            => DestroyPlayer(pc.playerModel.uid);

        public PlayerController FindPlayerByUID(uint uid)
        {
            if (!playerControllers.ContainsKey(uid)) return null;
            return playerControllers[uid];
        }
    }
}
