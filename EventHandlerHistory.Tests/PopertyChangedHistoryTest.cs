using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xunit;

namespace KzLibraries.EventHandlerHistory
{
    public class PopertyChangedHistoryTest
    {
        private class NotifyPropertyChanged : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null!)
            {
                if (!EqualityComparer<T>.Default.Equals(field, value))
                {
                    field = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            private int _Number;
            public int Number
            {
                set => this.SetProperty(ref _Number, value);
                get => this._Number;
            }

            private string _Text = "";
            public string Text
            {
                set => this.SetProperty(ref _Text, value);
                get => this._Text;
            }
        }

        [Fact]
        public void PopertyChangedTest()
        {
            var notifyPropertyChanged = new NotifyPropertyChanged();
            var history = new PropertyChangedHistory(notifyPropertyChanged);

            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Number)).Should().Be(0);
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Text)).Should().Be(0);
            history.History.Should().BeEmpty();
            notifyPropertyChanged.Number = 2;
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Number)).Should().Be(1);
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Text)).Should().Be(0);
            history.History.Should().Equal(
                nameof(NotifyPropertyChanged.Number));
            notifyPropertyChanged.Text = "foo";
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Number)).Should().Be(1);
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Text)).Should().Be(1);
            history.History.Should().Equal(
                nameof(NotifyPropertyChanged.Number),
                nameof(NotifyPropertyChanged.Text));
            notifyPropertyChanged.Number = 2;
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Number)).Should().Be(1);
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Text)).Should().Be(1);
            history.History.Should().Equal(
                nameof(NotifyPropertyChanged.Number),
                nameof(NotifyPropertyChanged.Text));
            notifyPropertyChanged.Number = 0;
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Number)).Should().Be(2);
            history.GetPropertyChangedCount(nameof(NotifyPropertyChanged.Text)).Should().Be(1);
            history.History.Should().Equal(
                nameof(NotifyPropertyChanged.Number),
                nameof(NotifyPropertyChanged.Text),
                nameof(NotifyPropertyChanged.Number));
        }

        [Fact]
        public void PopertyChangedStatusTest()
        {
            var notifyPropertyChanged = new NotifyPropertyChanged();
            var history = new PropertyChangedHistory(notifyPropertyChanged);
            var numberStatus = history.GetPropertyChangedCountStatus(nameof(NotifyPropertyChanged.Number));
            var textStatus = history.GetPropertyChangedCountStatus(nameof(NotifyPropertyChanged.Text));

            numberStatus.Count.Should().Be(0);
            textStatus.Count.Should().Be(0);
            notifyPropertyChanged.Number = 2;
            numberStatus.Count.Should().Be(1);
            textStatus.Count.Should().Be(0);
            notifyPropertyChanged.Text = "foo";
            numberStatus.Count.Should().Be(1);
            textStatus.Count.Should().Be(1);
            notifyPropertyChanged.Number = 2;
            numberStatus.Count.Should().Be(1);
            textStatus.Count.Should().Be(1);
            notifyPropertyChanged.Number = 0;
            numberStatus.Count.Should().Be(2);
            textStatus.Count.Should().Be(1);
        }

        [Fact]
        public void PopertyChangedStatusDefaultTest()
        {
            var notifyPropertyChanged = new NotifyPropertyChanged();
            var history = new PropertyChangedHistory(notifyPropertyChanged);
            var numberStatus = history.GetPropertyChangedCountStatus(nameof(NotifyPropertyChanged.Number));
            var textStatus = history.GetPropertyChangedCountStatus(nameof(NotifyPropertyChanged.Text));

            notifyPropertyChanged.Number = 2;
            notifyPropertyChanged.Text = "foo";
            numberStatus.Count.Should().Be(1);
            textStatus.Count.Should().Be(1);
            notifyPropertyChanged.Number = 2;
            numberStatus.Count.Should().Be(1);
            textStatus.Count.Should().Be(1);
            notifyPropertyChanged.Number = 0;
            numberStatus.Count.Should().Be(2);
            textStatus.Count.Should().Be(1);
        }
    }

}
