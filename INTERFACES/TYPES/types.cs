using Hyland.Unity;
using Hyland.Unity.EnterpriseIntegrationServer;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bases.Types
{
    public class OnBaseConnection : IDisposable
    {
        public OnBaseConnection(string username, string password, string appserver, string datasource)
        {
            this.UserName = username;
            this.Password = password;
            this.AppServer = appserver;
            this.DataSource = datasource;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AppServer { get; set; }
        public string DataSource { get; set; }
        public Application Application { get; private set; }

        public void Connect()
        {
            Hyland.Unity.AuthenticationProperties props = Hyland.Unity.Application.CreateOnBaseAuthenticationProperties(this.AppServer, this.UserName, this.Password, this.DataSource);
            this.Application = Hyland.Unity.Application.Connect(props);
            Console.WriteLine($"|------- ...Conected with session id: {this.Application.SessionID}");
        }
        public void Dispose()
        {
            if (this.Application != null)
            {
                if (this.IsConnected)
                {
                    this.Application.Disconnect();
                }
                this.Application?.Dispose();
            }
        }
        public bool IsConnected
        {
            get
            {
                if (this.Application == null)
                    return false;
                else
                    return this.Application.IsConnected;
            }
        }
        public static OnBaseConnection Create(string username, string password, string appserver, string datasource)
        {
            return new OnBaseConnection(username, password, appserver, datasource);
        }
    }
    public sealed class WorkflowEventArgs : BaseContextualEventArgs
    {
        public long BatchDocumentsRemaining { get; }

        public EISMessageItem EISMessageItem { get; }

        public PropertyBag PersistentPropertyBag { get; }

        public PropertyBag PropertyBag { get; set; }

        public Hyland.Unity.Workflow.Queue Queue { get; }

        public bool ScriptResult { get; set; }

        public PropertyBag SessionPropertyBag { get; set; }

        public WorkflowEventArgs(Hyland.Unity.Document document, Hyland.Unity.Workflow.Queue queue, Bases.Types.PropertyBag sessionPropertyBag, Bases.Types.PropertyBag scopedPropertyBag, Bases.Types.PropertyBag persistentPropertyBag, long batchDocumentsRemaining, bool scriptResult)
            : base(document)
        {
            Queue = queue;
            SessionPropertyBag = sessionPropertyBag;
            PropertyBag = scopedPropertyBag;
            PersistentPropertyBag = persistentPropertyBag;
            BatchDocumentsRemaining = batchDocumentsRemaining;
            ScriptResult = scriptResult;
        }

        public WorkflowEventArgs(Folder folder, Hyland.Unity.Workflow.Queue queue, Bases.Types.PropertyBag sessionPropertyBag, Bases.Types.PropertyBag scopedPropertyBag, Bases.Types.PropertyBag persistentPropertyBag, long batchDocumentsRemaining, bool scriptResult)
            : base(folder)
        {
            Queue = queue;
            SessionPropertyBag = sessionPropertyBag;
            PropertyBag = scopedPropertyBag;
            PersistentPropertyBag = persistentPropertyBag;
            BatchDocumentsRemaining = batchDocumentsRemaining;
            ScriptResult = scriptResult;
        }

        public WorkflowEventArgs(Hyland.Unity.WorkView.Object wvObject, Hyland.Unity.Workflow.Queue queue, Bases.Types.PropertyBag sessionPropertyBag, PropertyBag scopedPropertyBag, PropertyBag persistentPropertyBag, long batchDocumentsRemaining, bool scriptResult)
            : base(wvObject)
        {
            Queue = queue;
            SessionPropertyBag = sessionPropertyBag;
            PropertyBag = scopedPropertyBag;
            PersistentPropertyBag = persistentPropertyBag;
            BatchDocumentsRemaining = batchDocumentsRemaining;
            ScriptResult = scriptResult;
        }

        public WorkflowEventArgs(EISMessageItem eisMessageItem, Hyland.Unity.Workflow.Queue queue, Bases.Types.PropertyBag sessionPropertyBag, Bases.Types.PropertyBag scopedPropertyBag, Bases.Types.PropertyBag persistentPropertyBag, long batchDocumentsRemaining, bool scriptResult)
        {
            EISMessageItem = eisMessageItem;
            Queue = queue;
            SessionPropertyBag = sessionPropertyBag;
            PropertyBag = scopedPropertyBag;
            PersistentPropertyBag = persistentPropertyBag;
            BatchDocumentsRemaining = batchDocumentsRemaining;
            ScriptResult = scriptResult;
        }
    }
    public abstract class BaseContextualEventArgs : DocumentEventArgs
    {
        public Hyland.Unity.Folder Folder { get; }

        public Hyland.Unity.WorkView.Object WorkViewObject { get; }

        internal BaseContextualEventArgs()
            : base(null)
        {
        }

        internal BaseContextualEventArgs(Hyland.Unity.Document document)
            : base(document)
        {
        }

        internal BaseContextualEventArgs(Hyland.Unity.Folder folder)
            : base(null)
        {
            Folder = folder;
        }

        internal BaseContextualEventArgs(Hyland.Unity.WorkView.Object workViewObject)
            : base(null)
        {
            WorkViewObject = workViewObject;
        }
    }

    public abstract class DocumentEventArgs : EventArgs
    {
        public Hyland.Unity.Document Document { get; internal set; }

        internal DocumentEventArgs(Hyland.Unity.Document document)
        {
            Document = document;
        }
    }
    public class PropertyBag : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, ICollection
    {
        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public int Count => Values.Count;

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => Values.IsReadOnly;

        ICollection<string> IDictionary<string, object>.Keys => Values.Keys;

        ICollection<object> IDictionary<string, object>.Values => Values.Values;

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                return Values[key];
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (!IsSupportedValue(value))
                {
                    throw new ArgumentException("Value Type is not supported: " + value.GetType().FullName, "value");
                }

                Values[key] = value;
            }
        }

        private IDictionary<string, object> Values { get; }

        public PropertyBag(IDictionary<string, object> values)
        {
            Values = values;
        }

        public bool ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return Values.ContainsKey(key);
        }

        public void Set(string key, bool value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, byte value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, char value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, DateTime value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, decimal value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, double value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, short value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, int value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, long value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, sbyte value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, float value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, ushort value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, uint value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, ulong value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, bool[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, byte[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, char[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, DateTime[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, decimal[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, double[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, short[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, int[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, long[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, sbyte[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, float[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, string[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, ushort[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, uint[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public void Set(string key, ulong[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Values[key] = value;
        }

        public bool TryGetValue(string key, out bool value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<bool>(key, out value);
        }

        public bool TryGetValue(string key, out byte value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<byte>(key, out value);
        }

        public bool TryGetValue(string key, out char value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<char>(key, out value);
        }

        public bool TryGetValue(string key, out DateTime value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<DateTime>(key, out value);
        }

        public bool TryGetValue(string key, out decimal value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<decimal>(key, out value);
        }

        public bool TryGetValue(string key, out double value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<double>(key, out value);
        }

        public bool TryGetValue(string key, out short value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<short>(key, out value);
        }

        public bool TryGetValue(string key, out int value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<int>(key, out value);
        }

        public bool TryGetValue(string key, out long value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<long>(key, out value);
        }

        public bool TryGetValue(string key, out sbyte value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<sbyte>(key, out value);
        }

        public bool TryGetValue(string key, out float value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<float>(key, out value);
        }

        public bool TryGetValue(string key, out string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<string>(key, out value);
        }

        public bool TryGetValue(string key, out ushort value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<ushort>(key, out value);
        }

        public bool TryGetValue(string key, out uint value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<uint>(key, out value);
        }

        public bool TryGetValue(string key, out ulong value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<ulong>(key, out value);
        }

        public bool TryGetValue(string key, out bool[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<bool>(key, out value);
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<byte>(key, out value);
        }

        public bool TryGetValue(string key, out char[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<char>(key, out value);
        }

        public bool TryGetValue(string key, out DateTime[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<DateTime>(key, out value);
        }

        public bool TryGetValue(string key, out decimal[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<decimal>(key, out value);
        }

        public bool TryGetValue(string key, out double[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<double>(key, out value);
        }

        public bool TryGetValue(string key, out short[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<short>(key, out value);
        }

        public bool TryGetValue(string key, out int[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<int>(key, out value);
        }

        public bool TryGetValue(string key, out long[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<long>(key, out value);
        }

        public bool TryGetValue(string key, out sbyte[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<sbyte>(key, out value);
        }

        public bool TryGetValue(string key, out float[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<float>(key, out value);
        }

        public bool TryGetValue(string key, out string[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<string>(key, out value);
        }

        public bool TryGetValue(string key, out ushort[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<ushort>(key, out value);
        }

        public bool TryGetValue(string key, out uint[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<uint>(key, out value);
        }

        public bool TryGetValue(string key, out ulong[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return TryGetValue<ulong>(key, out value);
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("Array cannot be multidimensional.", "array");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, null);
            }

            checked
            {
                if (array.Length - index < Count)
                {
                    throw new ArgumentException("Array does not have enough capacity.", "array");
                }

                foreach (KeyValuePair<string, object> item in (IEnumerable<KeyValuePair<string, object>>)this)
                {
                    array.SetValue(item.Value, index++);
                }
            }
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentException("Key cannot be null.", "item");
            }

            if (!IsSupportedValue(item.Value))
            {
                throw new ArgumentException("Value Type is not supported: " + item.Value.GetType().FullName, "item");
            }

            Values.Add(item);
        }

        public virtual void Clear()
        {
            Values.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentException("Key cannot be null.", "item");
            }

            if (!IsSupportedValue(item.Value))
            {
                throw new ArgumentException("Value Type is not supported: " + item.Value.GetType().FullName, "item");
            }

            return Values.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, null);
            }

            if (checked(array.Length - arrayIndex) < Count)
            {
                throw new ArgumentException("Array does not have enough capacity.", "array");
            }

            Values.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentException("Key cannot be null.", "item");
            }

            if (!IsSupportedValue(item.Value))
            {
                throw new ArgumentException("Value Type is not supported: " + item.Value.GetType().FullName, "item");
            }

            return Values.Remove(item);
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (!IsSupportedValue(value))
            {
                throw new ArgumentException("Value Type is not supported: " + value.GetType().FullName, "value");
            }

            Values.Add(key, value);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return Values.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return Values.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return Values.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Values).GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        private static bool IsSupportedValue(object value)
        {
            if (value != null)
            {
                return IsSupportedValueType(value.GetType());
            }

            return true;
        }

        private static bool IsSupportedValueType(Type valueType)
        {
            TypeCode typeCode = Type.GetTypeCode(valueType);
            if (typeCode == TypeCode.Object)
            {
                typeCode = Type.GetTypeCode(valueType.GetElementType());
            }

            if ((uint)(typeCode - 3) <= 13u || typeCode == TypeCode.String)
            {
                return true;
            }

            return false;
        }

        private bool TryGetValue<T>(string key, out T[] value) where T : IConvertible
        {
            value = null;
            if (!Values.TryGetValue(key, out var value2))
            {
                return false;
            }

            value = value2 as T[];
            if (value == null)
            {
                return value2 == null;
            }

            return true;
        }

        private bool TryGetValue<T>(string key, out T value) where T : IConvertible
        {
            value = default(T);
            if (!Values.TryGetValue(key, out var value2))
            {
                return false;
            }

            TypeCode typeCode = Type.GetTypeCode(typeof(T));
            try
            {
                value2 = Convert.ChangeType(value2, typeCode, null);
                value = (T)value2;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
