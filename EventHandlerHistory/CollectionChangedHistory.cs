using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Kzrnm.EventHandlerHistory
{
    public class CollectionChangedHistory : IReadOnlyList<NotifyCollectionChangedEventArgs>, IDisposable
    {
        private readonly List<NotifyCollectionChangedEventArgs> history;
        public void Clear() => history.Clear();

        public int Count => history.Count;
        public NotifyCollectionChangedEventArgs this[int index] => history[index];
        public NotifyCollectionChangedEventArgs First => history[0];
        public NotifyCollectionChangedEventArgs Last => history[history.Count - 1];

        public CollectionChangedHistory(INotifyCollectionChanged notifyCollectionChanged)
        {
            history = new List<NotifyCollectionChangedEventArgs>();
            NotifyCollectionChanged = notifyCollectionChanged;
            NotifyCollectionChanged.CollectionChanged += NotifyCollectionChanged_CollectionChanged;
        }
        private INotifyCollectionChanged NotifyCollectionChanged { get; }

        private void NotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            history.Add(e);
        }

        public IEnumerator<NotifyCollectionChangedEventArgs> GetEnumerator() => history.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => history.GetEnumerator();

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    NotifyCollectionChanged.CollectionChanged -= NotifyCollectionChanged_CollectionChanged;
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
