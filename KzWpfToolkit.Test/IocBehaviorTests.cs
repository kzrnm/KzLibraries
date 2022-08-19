using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.Wpf.Toolkit;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace KzWpfToolkit.Test
{
    public class IocBehaviorTests
    {
#pragma warning disable CS0659 // 型は Object.Equals(object o) をオーバーライドしますが、Object.GetHashCode() をオーバーライドしません
        class DefinedType { public override bool Equals(object? obj) => obj is DefinedType; }
        class UndefinedType { public override bool Equals(object? obj) => obj is UndefinedType; }
#pragma warning restore CS0659 // 型は Object.Equals(object o) をオーバーライドしますが、Object.GetHashCode() をオーバーライドしません

        public IocBehaviorTests()
        {
            var ioc = new Ioc();
            ioc.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<DefinedType>()
                .BuildServiceProvider());
            IocBehavior.Ioc = ioc;
        }

        [UIFact]
        public void FrameworkElementAutoViewModel()
        {
            var obj = new FrameworkElement();
            IocBehavior.SetAutoViewModel(obj, typeof(DefinedType));
            IocBehavior.GetAutoViewModel(obj).Should().Be(typeof(DefinedType));
            obj.DataContext.Should().Be(new DefinedType());
            IocBehavior.SetAutoViewModel(obj, typeof(UndefinedType));
            IocBehavior.GetAutoViewModel(obj).Should().Be(typeof(UndefinedType));
            obj.DataContext.Should().BeNull();
        }

        [UIFact]
        public void DependencyObjectAutoViewModel()
        {
            var obj = new DependencyObject();
            IocBehavior.SetAutoViewModel(obj, typeof(DefinedType));
            IocBehavior.GetAutoViewModel(obj).Should().Be(typeof(DefinedType));
            IocBehavior.SetAutoViewModel(obj, typeof(UndefinedType));
            IocBehavior.GetAutoViewModel(obj).Should().Be(typeof(UndefinedType));
        }
    }

}