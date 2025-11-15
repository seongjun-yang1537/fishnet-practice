using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Ingame
{
    [RequireComponent(typeof(PlayerScope))]
    [RequireComponent(typeof(PlayerView))]
    public class PlayerController : MonoBehaviour
    {
        protected UnityEvent<PlayerModel> onBuildModel = new();

        [Inject]
        [field: SerializeField]
        public PlayerModel playerModel { get; private set; }
        [Inject]
        public PlayerView playerView { get; private set; }

        [Inject]
        private Rigidbody rigidbody;

        protected virtual void Awake()
        {
            BuildModel();
        }

        public void BuildModel(PlayerModel newPlayerModel = null)
        {
            if (newPlayerModel == null)
                newPlayerModel = new PlayerModel();

            playerModel = newPlayerModel;
            onBuildModel.Invoke(playerModel);
        }

        private void OnBuildModel(PlayerModel playerModel)
        {
            playerView = GetComponent<PlayerView>();
            playerView.onChangedColor.Invoke(playerModel.uid);
        }

        protected virtual void OnEnable()
        {
            onBuildModel.AddListener(OnBuildModel);
        }

        protected virtual void OnDisable()
        {
            onBuildModel.RemoveListener(OnBuildModel);
        }

        protected virtual void Start()
        {
            playerView.onChangedColor.Invoke(playerModel.uid);
        }

        public void Move(Vector3 direction)
        {
            if (direction.sqrMagnitude > 1f)
                direction.Normalize();

            Vector3 next = rigidbody.position + direction * playerModel.moveSpeed * Time.deltaTime;
            rigidbody.MovePosition(next);
        }

        public void Jump()
        {
            Debug.Log("Jump");
        }
    }
}
