using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Kzrnm.EventHandlerHistory
{
    public class Status : IEquatable<Status>
    {
        public string Name { get; }
        public int Count { internal set; get; }
        public Status(string name, int defaultCount = 0)
        {
            Name = name;
            Count = defaultCount;
        }

        public override string ToString() => $"{Name}: {Count}";
        public override int GetHashCode()
            => Name.GetHashCode() ^ Count.GetHashCode();
        public override bool Equals(object? obj)
        {
            if (obj is Status other)
                return Equals(other);
            return false;
        }
        public bool Equals(Status? other)
        {
            if (other == null)
                return false;
            return Name == other.Name && Count == other.Count;
        }
    }
    public class PropertyChangedHistory : IDictionary<string, int>, IReadOnlyDictionary<string, int>, IDisposable
    {
        public PropertyChangedHistory(INotifyPropertyChanged notifyPropertyChanged)
        {
            history = new List<string>();
            History = new ReadOnlyCollection<string>(history);
            statuses = new Dictionary<string, Status>();
            NotifyPropertyChanged = notifyPropertyChanged;

            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            history.Add(e.PropertyName);
            if (statuses.TryGetValue(e.PropertyName, out var status))
                status.Count++;
            else
                statuses.Add(e.PropertyName, new Status(e.PropertyName, 1));
        }

        private INotifyPropertyChanged NotifyPropertyChanged { get; }

        private readonly Dictionary<string, Status> statuses;
        private readonly List<string> history;
        public ReadOnlyCollection<string> History { get; }

        public void Clear()
        {
            history.Clear();
            statuses.Clear();
        }
        int ICollection<KeyValuePair<string, int>>.Count => statuses.Count;
        int IReadOnlyCollection<KeyValuePair<string, int>>.Count => statuses.Count;

        IEnumerable<string> IReadOnlyDictionary<string, int>.Keys => statuses.Keys;
        IEnumerable<int> IReadOnlyDictionary<string, int>.Values => statuses.Values.Select(s => s.Count);
        ICollection<string> IDictionary<string, int>.Keys => statuses.Keys;
        ICollection<int> IDictionary<string, int>.Values => new ReadOnlyCollection<int>(statuses.Values.Select(s => s.Count).ToArray());


        public bool IsReadOnly => true;

        int IDictionary<string, int>.this[string key]
        {
            get => GetPropertyChangedCount(key);
            set => throw new NotImplementedException();
        }

        public int this[string key] => GetPropertyChangedCount(key);

        public Status GetPropertyChangedCountStatus(string propertyName)
        {
            if (statuses.TryGetValue(propertyName, out var status))
                return status;

            status = new Status(propertyName);
            statuses.Add(propertyName, status);
            return status;
        }

        public int GetPropertyChangedCount(string propertyName)
        {
            statuses.TryGetValue(propertyName, out var status);
            return status?.Count ?? 0;
        }

        public bool ContainsKey(string key) => GetPropertyChangedCount(key) != 0;
        public bool TryGetValue(string key, out int value)
        {
            value = GetPropertyChangedCount(key);
            return true;
        }
        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
            => statuses.Select(pair => new KeyValuePair<string, int>(pair.Key, pair.Value.Count)).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(KeyValuePair<string, int> item) => ((ICollection<KeyValuePair<string, int>>)statuses).Contains(item);

        void IDictionary<string, int>.Add(string key, int value) => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, int>>.Add(KeyValuePair<string, int> item) => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, int>>.CopyTo(KeyValuePair<string, int>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, int>>)statuses).CopyTo(array, arrayIndex);
        bool IDictionary<string, int>.Remove(string key) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<string, int>>.Remove(KeyValuePair<string, int> item) => throw new NotImplementedException();

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    NotifyPropertyChanged.PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
                    Clear();
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
