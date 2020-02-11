using FluentAssertions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xunit;

namespace KzLibraries.EventHandlerHistory
{
    public class CollectionChangedHistoryTest
    {
        [Fact]
        public void CollectionChangedTest()
        {
            var observableCollection = new ObservableCollection<int>();
            var history = new CollectionChangedHistory(observableCollection);

            var count = 0;
            history.Count.Should().Be(count);

            observableCollection.Add(5);
            history.Last.Action.Should().Be(NotifyCollectionChangedAction.Add);
            history.Last.OldItems.Should().BeNull();
            history.Last.OldStartingIndex.Should().Be(-1);
            history.Last.NewItems.Should().BeEquivalentTo(new object[] { 5 });
            history.Last.NewStartingIndex.Should().Be(0);
            history.Count.Should().Be(++count);

            observableCollection.Add(0);
            history.Last.Action.Should().Be(NotifyCollectionChangedAction.Add);
            history.Last.OldItems.Should().BeNull();
            history.Last.OldStartingIndex.Should().Be(-1);
            history.Last.NewItems.Should().BeEquivalentTo(new object[] { 0 });
            history.Last.NewStartingIndex.Should().Be(1);
            history.Count.Should().Be(++count);

            observableCollection.Move(0, 1);
            history.Last.Action.Should().Be(NotifyCollectionChangedAction.Move);
            history.Last.OldItems.Should().BeEquivalentTo(new object[] { 5 });
            history.Last.OldStartingIndex.Should().Be(0);
            history.Last.NewItems.Should().BeEquivalentTo(new object[] { 5 });
            history.Last.NewStartingIndex.Should().Be(1);
            history.Count.Should().Be(++count);

            observableCollection.RemoveAt(1);
            history.Last.Action.Should().Be(NotifyCollectionChangedAction.Remove);
            history.Last.OldItems.Should().BeEquivalentTo(new object[] { 5 });
            history.Last.OldStartingIndex.Should().Be(1);
            history.Last.NewItems.Should().BeNull();
            history.Last.NewStartingIndex.Should().Be(-1);
            history.Count.Should().Be(++count);

            observableCollection[0] = 1;
            history.Last.Action.Should().Be(NotifyCollectionChangedAction.Replace);
            history.Last.OldItems.Should().BeEquivalentTo(new object[] { 0 });
            history.Last.OldStartingIndex.Should().Be(0);
            history.Last.NewItems.Should().BeEquivalentTo(new object[] { 1 });
            history.Last.NewStartingIndex.Should().Be(0);
            history.Count.Should().Be(++count);

            observableCollection.Clear();
            history.Last.Action.Should().Be(NotifyCollectionChangedAction.Reset);
            history.Last.OldItems.Should().BeNull();
            history.Last.OldStartingIndex.Should().Be(-1);
            history.Last.NewItems.Should().BeNull();
            history.Last.NewStartingIndex.Should().Be(-1);
            history.Count.Should().Be(++count);
        }

        [Fact]
        public void ClearTest()
        {
            var observableCollection = new ObservableCollection<int>();
            var history = new CollectionChangedHistory(observableCollection);

            history.Should().BeEmpty();
            observableCollection.Add(0);
            history.Should().ContainSingle();
            history.Clear();
            history.Should().BeEmpty();
        }
    }
}
