using System;
using System.Collections.Generic;

namespace FB2Toolbox.Utilities
{
    public class FileMetadata
    {
        #region Private
        private readonly Dictionary<string, string> _metadataItems = new Dictionary<string, string>();
        private bool _initialized = false;
        #endregion
        private void InternalAddMetadata(string key, string value)
        {
            string _key = key;
            string _value = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            if (Metadata.ContainsKey(_key))
            {
                Metadata[_key] = _value;
            }
            else
            {
                Metadata.Add(_key, _value);
            }
        }
        private Dictionary<string, string> Metadata
        {
            get
            {
                if (_metadataItems.Count == 0)
                {
                    InternalInitialize();
                }

                return _metadataItems;
            }
        }
        protected void SetDescription(string description)
        {
            Description = description;
        }
        protected virtual void InternalInitialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            DescriptionElements[] elements = Enum.GetValues(typeof(DescriptionElements)) as DescriptionElements[];
            foreach (DescriptionElements element in elements)
            {
                AddMetadata(element, string.Empty);
            }
        }
        protected virtual void InternalParseDescription(string description)
        {
        }
        protected void ParseDescription(string description)
        {
            InternalParseDescription(description);
        }
        protected virtual bool CheckRequiredAttribute(string part)
        {
            foreach (KeyValuePair<string, string> item in Metadata)
            {
                // Only check required attributes (in parentheses), not optional ones (in square brackets)
                if (part.Contains(string.Format("({0})", item.Key)))
                {
                    if (string.IsNullOrEmpty(item.Value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        protected string NormalizeString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToLower().ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        public void AddMetadata(DescriptionElements key, int index, string value)
        {
            string name = Enum.GetName(typeof(DescriptionElements), key);
            if (index == 0)
            {
                InternalAddMetadata(name, value);
            }

            name += Convert.ToString(index + 1);
            InternalAddMetadata(name, value);
        }
        public void AddMetadata(DescriptionElements key, string value)
        {
            string name = Enum.GetName(typeof(DescriptionElements), key);
            InternalAddMetadata(name, value);
        }
        public string GetMetadata(DescriptionElements key)
        {
            string name = Enum.GetName(typeof(DescriptionElements), key);
            string _key = string.Format("{0}", name);
            return Metadata[_key];
        }
        public string SubstitutePart(string part)
        {
            if (CheckRequiredAttribute(part))
            {
                foreach (KeyValuePair<string, string> item in Metadata)
                {
                    // Replace required attributes (in parentheses) with their values
                    part = part.Replace(string.Format("({0})", item.Key), item.Value);
                    
                    // Replace optional attributes (in square brackets)
                    // If value is not empty, use the value; otherwise remove the entire placeholder
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        part = part.Replace(string.Format("[{0}]", item.Key), item.Value);
                    }
                    else
                    {
                        part = part.Replace(string.Format("[{0}]", item.Key), string.Empty);
                    }
                }
                return part;
            }
            return string.Empty;
        }
        public string Description { get; private set; } = string.Empty;
        public FileMetadata()
        {
        }
    }
}
