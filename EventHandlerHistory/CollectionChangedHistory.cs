using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace KzLibraries.EventHandlerHistory
{
    public class CollectionChangedHistory : IReadOnlyList<NotifyCollectionChangedEventArgs>, IDisposable
    {
        private readonly List<NotifyCollectionChangedEventArgs> history;
        public void Clear() => this.history.Clear();

        public int Count => this.history.Count;
        public NotifyCollectionChangedEventArgs this[int index] => this.history[index];
        public NotifyCollectionChangedEventArgs First => this.history[0];
        public NotifyCollectionChangedEventArgs Last => this.history[this.history.Count - 1];

        public CollectionChangedHistory(INotifyCollectionChanged notifyCollectionChanged)
        {
            this.history = new List<NotifyCollectionChangedEventArgs>();
            this.notifyCollectionChanged = notifyCollectionChanged;
            this.notifyCollectionChanged.CollectionChanged += this.NotifyCollectionChanged_CollectionChanged;
        }
        private readonly INotifyCollectionChanged notifyCollectionChanged;

        private void NotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.history.Add(e);
        }

        public IEnumerator<NotifyCollectionChangedEventArgs> GetEnumerator() => this.history.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.history.GetEnumerator();

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.notifyCollectionChanged.CollectionChanged -= this.NotifyCollectionChanged_CollectionChanged;
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
