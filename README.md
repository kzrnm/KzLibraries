My WPF libraries

## Install

```
dotnet add package KzWpfControl
```
```
dotnet add package KzWpfToolkit
```
```
dotnet add package EventHandlerHistory
```

# Usage

See [sandbox](sandbox)

## KzWpfControl
WPF Controls. Set `urn:kzrnm-wpf/controls` to `xmlns`.

```xml
<Window xmlns:kz="urn:kzrnm-wpf/controls">
```

### ComboBoxBehavior

`ComboBoxBehavior.IsFirstEmpty`: Insert empty item befor first item.

```xml
<ComboBox kz:ComboBoxBehavior.IsFirstEmpty="True">
    <ComboBoxItem>1</ComboBoxItem>
    <ComboBoxItem>2</ComboBoxItem>
    <ComboBoxItem>3</ComboBoxItem>
</ComboBox>
```

### DoubleTextBox

Even if `UpdateSourceTrigger=PropertyChanged`, text will follow input.

```xml
<kz:DoubleTextBox DoubleText="{Binding DoubleProperty, UpdateSourceTrigger=PropertyChanged}" />
```

#### note: `TextBox` bind to `double`

```xml
<TextBox Text="{Binding DoubleProperty, UpdateSourceTrigger=PropertyChanged}" />
```

If your xaml is above, text may not follow input.

|Input|Text|
|---|---|
|1|1|
|1.|1|
|1.0|1|
|1.1|1.1|

### TextBoxBehavior

`TextBoxBehavior.SelectAllOnFocus`: Select text on focus the TextBox.

```xml
<TextBox kz:TextBoxBehavior.SelectAllOnFocus="True" Text="foo" />
```


## KzWpfToolkit
Library for [.NET Community Toolkit/MVVM toolkit](https://github.com/CommunityToolkit/dotnet/).
Set `urn:kzrnm-wpf/toolkit` to `xmlns`.

```xml
<Window xmlns:kz="urn:kzrnm-wpf/toolkit">
```

### Ioc
For `Ioc`.

`Ioc.AutoViewModel`: Auto set view model

```xml
<Window
    x:Class="sandbox.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:ktk="urn:kzrnm-wpf/toolkit"
    xmlns:local="clr-namespace:sandbox"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    ktk:IocBehavior.AutoViewModel="{x:Type local:MainViewModel}" >
</Window>

```

## EventHandlerHistory

For testing.
