using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace KzLibraries.EventHandlerHistory
{
    public class Status : IEquatable<Status>
    {
        public string Name { get; }
        public int Count { internal set; get; }
        public Status(string name, int defaultCount = 0)
        {
            this.Name = name;
            this.Count = defaultCount;
        }

        public override string ToString() => $"{Name}: {Count}";
        public override int GetHashCode()
            => this.Name.GetHashCode() ^ this.Count.GetHashCode();
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
            return this.Name == other.Name && this.Count == other.Count;
        }
    }
    public class PropertyChangedHistory : IDictionary<string, int>, IReadOnlyDictionary<string, int>, IDisposable
    {
        public PropertyChangedHistory(INotifyPropertyChanged notifyPropertyChanged)
        {
            this.history = new Dictionary<string, Status>();
            this.notifyPropertyChanged = notifyPropertyChanged;

            notifyPropertyChanged.PropertyChanged += this.NotifyPropertyChanged_PropertyChanged;
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.history.TryGetValue(e.PropertyName, out var status))
                status.Count++;
            else
                this.history.Add(e.PropertyName, new Status(e.PropertyName, 1));
        }


        private readonly INotifyPropertyChanged notifyPropertyChanged;

        private readonly Dictionary<string, Status> history;


        int ICollection<KeyValuePair<string, int>>.Count => this.history.Count;
        int IReadOnlyCollection<KeyValuePair<string, int>>.Count => this.history.Count;

        IEnumerable<string> IReadOnlyDictionary<string, int>.Keys => this.history.Keys;
        IEnumerable<int> IReadOnlyDictionary<string, int>.Values => this.history.Values.Select(s => s.Count);
        ICollection<string> IDictionary<string, int>.Keys => this.history.Keys;
        ICollection<int> IDictionary<string, int>.Values => new ReadOnlyCollection<int>(this.history.Values.Select(s => s.Count).ToArray());


        public bool IsReadOnly => true;

        int IDictionary<string, int>.this[string key]
        {
            get => this.GetPropertyChangedCount(key);
            set => throw new NotImplementedException();
        }

        public int this[string key] => this.GetPropertyChangedCount(key);

        public Status GetPropertyChangedCountStatus(string propertyName)
        {
            if (this.history.TryGetValue(propertyName, out var status))
                return status;

            status = new Status(propertyName);
            this.history.Add(propertyName, status);
            return status;
        }

        public int GetPropertyChangedCount(string propertyName)
        {
            this.history.TryGetValue(propertyName, out var status);
            return status?.Count ?? 0;
        }

        public bool ContainsKey(string key) => this.GetPropertyChangedCount(key) != 0;
        public bool TryGetValue(string key, out int value)
        {
            value = this.GetPropertyChangedCount(key);
            return true;
        }
        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
            => this.history.Select(pair => new KeyValuePair<string, int>(pair.Key, pair.Value.Count)).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool Contains(KeyValuePair<string, int> item) => ((ICollection<KeyValuePair<string, int>>)history).Contains(item);

        void IDictionary<string, int>.Add(string key, int value) => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, int>>.Add(KeyValuePair<string, int> item) => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, int>>.Clear() => throw new NotImplementedException();
        void ICollection<KeyValuePair<string, int>>.CopyTo(KeyValuePair<string, int>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, int>>)this.history).CopyTo(array, arrayIndex);
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
                    this.notifyPropertyChanged.PropertyChanged -= this.NotifyPropertyChanged_PropertyChanged;
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
