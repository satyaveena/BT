using System;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Policy;
using FakeItEasy;


namespace BT.Auth.UnitTest
{
    public class AutoFakeExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new AutoFakeBuilderStrategy(),UnityBuildStage.PreCreation);
        }

        private class AutoFakeBuilderStrategy : BuilderStrategy
        {
            private static readonly System.Reflection.MethodInfo _fakeGenericDefinition;

            static AutoFakeBuilderStrategy()
            {
                _fakeGenericDefinition = typeof(A).GetMethod("Fake", Type.EmptyTypes);
            }

            public override void PreBuildUp(IBuilderContext context)
            {
                if (context.Existing == null)
                {
                    var type = context.BuildKey.Type;
                    if (type.IsInterface || type.IsAbstract)
                    {
                        var fakeMethod = _fakeGenericDefinition.MakeGenericMethod(type);
                        var fake = fakeMethod.Invoke(null, new object[0]);
                        context.PersistentPolicies.Set<ILifetimePolicy>(new ContainerControlledLifetimeManager(), context.BuildKey);
                        context.Existing = fake;
                        context.BuildComplete = true;
                    }
                }
                base.PreBuildUp(context);
            }
        }
    }
}
