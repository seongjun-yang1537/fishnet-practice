using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class WorldScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<WorldModel>(Lifetime.Scoped);

            builder.RegisterComponent(GetComponent<WorldController>());
        }
    }
}