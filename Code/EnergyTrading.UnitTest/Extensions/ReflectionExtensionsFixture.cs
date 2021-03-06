﻿namespace EnergyTrading.UnitTest.Extensions
{
    using EnergyTrading.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class ReflectionExtensionsFixture
    {
        public class GrandChild
        {
        }

        private class Child
        {
// ReSharper disable UnusedAutoPropertyAccessor.Local
            public GrandChild GrandChild { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private class Parent
        {
            public Child Child { get; set; }
        }

        [Test]
        public void IfNullCreateReturnsNullIfSourceIsNull()
        {
            var candidate = ReflectionExtension.IfNullCreate<Parent, Child>(null, t => t.Child, () => new Child());
            Assert.IsNull(candidate);
        }

        [Test]
        public void IfNullCreateReturnsNullIfFuncIsNull()
        {
            var candidate = new Parent().IfNullCreate(null, () => new Child());
            Assert.IsNull(candidate);
        }

        [Test]
        public void IfNullCreateReturnsNullIfCreateFunctionIsNull()
        {
            var candidate = new Parent().IfNullCreate(t => t.Child, null);
            Assert.IsNull(candidate);
        }

        [Test]
        public void IfNullCreateReturnsChildIfExists()
        {
            var parent = new Parent { Child = new Child() };
            var candidate = parent.IfNullCreate(t => t.Child, () => new Child());
            Assert.AreSame(candidate, parent.Child);
        }

        [Test]
        public void IfNullCreateSetsPropertyAndReturnsCorrectValueIfChildIsNull()
        {
            var parent = new Parent();
            var candidate = parent.IfNullCreate(t => t.Child, () => new Child());
            Assert.IsNotNull(candidate);
            Assert.AreSame(candidate, parent.Child);
        }

        [Test]
        public void ChainingIfNullCreate()
        {
            var parent = new Parent();
            var grandChild = parent.IfNullCreate(x => x.Child, () => new Child()).IfNullCreate(y => y.GrandChild, () => new GrandChild());
            Assert.IsNotNull(grandChild);
            Assert.IsNotNull(parent.Child);
            Assert.IsNotNull(parent.Child.GrandChild);
            Assert.AreSame(grandChild, parent.Child.GrandChild);
        }

        [Test]
        public void ChainingWithDefaultConstructorVersion()
        {
            var parent = new Parent();
            var grandChild = parent.IfNullCreate(x => x.Child).IfNullCreate(y => y.GrandChild);
            Assert.IsNotNull(grandChild);
            Assert.IsNotNull(parent.Child);
            Assert.IsNotNull(parent.Child.GrandChild);
            Assert.AreSame(grandChild, parent.Child.GrandChild);
        }
    }
}