using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class PlayerScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<PlayerModel>(Lifetime.Scoped);

            builder.RegisterComponent(GetComponent<Rigidbody>());
            builder.RegisterComponent(GetComponent<PlayerView>());
            builder.RegisterComponent(GetComponent<PlayerController>());
        }
    }
}