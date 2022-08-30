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
            var ioc = new CommunityToolkit.Mvvm.DependencyInjection.Ioc();
            ioc.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<DefinedType>()
                .BuildServiceProvider());
            Kzrnm.Wpf.Toolkit.Ioc.DefaultIoc = ioc;
        }

        [UIFact]
        public void FrameworkElementAutoViewModel()
        {
            var obj = new FrameworkElement();
            Kzrnm.Wpf.Toolkit.Ioc.SetAutoViewModel(obj, typeof(DefinedType));
            Ioc.GetAutoViewModel(obj).Should().Be(typeof(DefinedType));
            obj.DataContext.Should().Be(new DefinedType());
            Kzrnm.Wpf.Toolkit.Ioc.SetAutoViewModel(obj, typeof(UndefinedType));
            Ioc.GetAutoViewModel(obj).Should().Be(typeof(UndefinedType));
            obj.DataContext.Should().BeNull();
        }

        [UIFact]
        public void DependencyObjectAutoViewModel()
        {
            var obj = new DependencyObject();
            Kzrnm.Wpf.Toolkit.Ioc.SetAutoViewModel(obj, typeof(DefinedType));
            Ioc.GetAutoViewModel(obj).Should().Be(typeof(DefinedType));
            Kzrnm.Wpf.Toolkit.Ioc.SetAutoViewModel(obj, typeof(UndefinedType));
            Ioc.GetAutoViewModel(obj).Should().Be(typeof(UndefinedType));
        }
    }

}