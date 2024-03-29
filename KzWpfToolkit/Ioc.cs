using System;
using System.ComponentModel;
using System.Windows;
using CommunityToolkitIoc = CommunityToolkit.Mvvm.DependencyInjection.Ioc;

namespace Kzrnm.Wpf.Toolkit
{
    public static class Ioc
    {
        public static CommunityToolkitIoc DefaultIoc { set; get; } = CommunityToolkitIoc.Default;
        public static Type GetAutoViewModel(DependencyObject obj) => (Type)obj.GetValue(AutoViewModelProperty);
        public static void SetAutoViewModel(DependencyObject obj, Type value) => obj.SetValue(AutoViewModelProperty, value);
        public static readonly DependencyProperty AutoViewModelProperty =
            DependencyProperty.RegisterAttached(
                "AutoViewModel",
                typeof(Type),
                typeof(Ioc),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.NotDataBindable,
                    AutoViewModelChanged));

        private static void AutoViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;
            if (d is FrameworkElement elm)
            {
                elm.DataContext = e.NewValue is Type type ? DefaultIoc.GetService(type) : null;
            }
        }
    }

}
