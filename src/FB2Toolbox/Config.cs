using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.IO;

namespace FB2Toolbox
{
    public class GenreSubstitutionElement : ConfigurationElement, IComparable
    {
        [ConfigurationProperty("from", IsRequired = true)]
        public string From
        {
            get
            {
                return (string)this["from"];
            }
            set
            {
                this["from"] = value;
            }
        }
        [ConfigurationProperty("to", DefaultValue = "Unknown")]
        public string To
        {
            get
            {
                return (string)this["to"];
            }
            set
            {
                this["to"] = value;
            }
        }
        public override string ToString()
        {
            return string.Format("{0} ({1})", To, From);
        }
        public int CompareTo(object obj)
        {
            if (obj is GenreSubstitutionElement)
                return this.ToString().CompareTo(obj.ToString());
            else
                return 0;
        }
    }
    public class EncodingElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
    public class RenameProfileElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
        [ConfigurationProperty("fileName", IsRequired = true)]
        public string FileName
        {
            get
            {
                return (string)this["fileName"];
            }
            set
            {
                this["fileName"] = value;
            }
        }
        [ConfigurationProperty("characterSubstitution", IsDefaultCollection = true, IsRequired = false)]
        public CharacterSubstitutionCollection CharacterSubstitution
        {
            get
            {
                return (CharacterSubstitutionCollection)this["characterSubstitution"];
            }
        }
    }
    public class CharacterSubstitutionElement : ConfigurationElement
    {
        [ConfigurationProperty("from", IsRequired=true)]
        [StringValidator(InvalidCharacters="\\")]
        public string From
        {
            get
            {
                return (string)this["from"];
            }
            set
            {
                this["from"] = value;
            }
        }
        [ConfigurationProperty("to", DefaultValue="")]
        public string To
        {
            get
            {
                return (string)this["to"];
            }
            set
            {
                this["to"] = value;
            }
        }
        [ConfigurationProperty("repeat", DefaultValue = 1)]
        [IntegerValidator(MinValue=1, MaxValue=50)]
        public int Repeat
        {
            get
            {
                return (int)this["repeat"];
            }
            set
            {
                this["repeat"] = value;
            }
        }
    }
    public class CommandTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("checkedFiles", IsDefaultCollection = true, IsRequired = true)]
        public CommandsCollection CheckedFilesCommands
        {
            get
            {
                return (CommandsCollection)this["checkedFiles"];
            }
        }
        [ConfigurationProperty("focusedFile", IsDefaultCollection = true, IsRequired = true)]
        public CommandsCollection FocusedFileCommand
        {
            get
            {
                return (CommandsCollection)this["focusedFile"];
            }
        }
    }
    public class CommandElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
        [ConfigurationProperty("fileName", IsRequired=true)]
        public string FileName
        {
            get
            {
                return (string)this["fileName"];
            }
            set
            {
                this["fileName"] = value;
            }
        }
        [ConfigurationProperty("arguments", IsRequired = false, DefaultValue="")]
        public string Arguments
        {
            get
            {
                return (string)this["arguments"];
            }
            set
            {
                this["arguments"] = value;
            }
        }
        [ConfigurationProperty("createNoWindow", IsRequired = false, DefaultValue = false)]
        public bool CreateNoWindow
        {
            get
            {
                return (bool)this["createNoWindow"];
            }
            set
            {
                this["createNoWindow"] = value;
            }
        }
        [ConfigurationProperty("onlyWithExtension", IsRequired = false, DefaultValue = "")]
        public string OnlyWithExtension
        {
            get
            {
                return (string)this["onlyWithExtension"];
            }
            set
            {
                this["onlyWithExtension"] = value;
            }
        }
        [ConfigurationProperty("waitAndReload", IsRequired = false, DefaultValue = true)]
        public bool WaitAndReload
        {
            get
            {
                return (bool)this["waitAndReload"];
            }
            set
            {
                this["waitAndReload"] = value;
            }
        }
    }
    [ConfigurationCollection(typeof(CommandElement), CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "command")]
    public class CommandsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CommandElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as CommandElement).Name;
        }
    }
    [ConfigurationCollection(typeof(CharacterSubstitutionElement), CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "char")]
    public class CharacterSubstitutionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CharacterSubstitutionElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as CharacterSubstitutionElement).From;
        }
    }
    [ConfigurationCollection(typeof(GenreSubstitutionElement), CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "genre")]
    public class GenresCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new GenreSubstitutionElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as GenreSubstitutionElement).From;
        }
        public string FindSubstitution(string genreName)
        {
            if (String.IsNullOrEmpty(genreName))
                return String.Empty;
            foreach (GenreSubstitutionElement gs in this)
                if (gs.From.ToUpperInvariant() == genreName.ToUpperInvariant())
                    return gs.To;
            return genreName;
        }
    }
    [ConfigurationCollection(typeof(EncodingElement), CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "encoding")]
    public class EncodingsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EncodingElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as EncodingElement).Name;
        }
        [ConfigurationProperty("translateEncodings", DefaultValue = true)]
        public bool TranslateEncodings
        {
            get
            {
                return (bool)this["translateEncodings"];
            }
            set
            {
                this["translateEncodings"] = value;
            }
        }
        [ConfigurationProperty("indentedFormatting", DefaultValue = true)]
        public bool IndentFile
        {
            get
            {
                return (bool)this["indentedFormatting"];
            }
            set
            {
                this["indentedFormatting"] = value;
            }
        }
        [ConfigurationProperty("compressionEncoding", IsRequired = false, DefaultValue = "utf-8")]
        public string CompressionEncoding
        {
            get
            {
                return (string)this["compressionEncoding"];
            }
            set
            {
                this["compressionEncoding"] = value;
            }
        }
    }
    [ConfigurationCollection(typeof(RenameProfileElement), CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "profile")]
    public class RenameProfilesCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("globalTranslit", IsDefaultCollection = true, IsRequired = false)]
        public CharacterSubstitutionCollection GlobalTranslit
        {
            get
            {
                return (CharacterSubstitutionCollection)this["globalTranslit"];
            }
        }
        [ConfigurationProperty("globalCharacterSubstitution", IsDefaultCollection = true, IsRequired = false)]
        public CharacterSubstitutionCollection GlobalCharacterSubstitution
        {
            get
            {
                return (CharacterSubstitutionCollection)this["globalCharacterSubstitution"];
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new RenameProfileElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as RenameProfileElement).Name;
        }
    }
    public class FB2ConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("normalizeNames", IsRequired = false, DefaultValue = true)]
        public bool NormalizeNames
        {
            get
            {
                return (bool)this["normalizeNames"];
            }
            set
            {
                this["normalizeNames"] = value;
            }
        }
        [ConfigurationProperty("fb2Extension", IsRequired = false, DefaultValue = ".fb2")]
        public string FB2Extension
        {
            get
            {
                return (string)this["fb2Extension"];
            }
            set
            {
                this["fb2Extension"] = value;
            }
        }
        [ConfigurationProperty("fb2zipExtension", IsRequired = false, DefaultValue = ".fb2.zip")]
        public string FB2ZIPExtension
        {
            get
            {
                return (string)this["fb2zipExtension"];
            }
            set
            {
                this["fb2zipExtension"] = value;
            }
        }
        [ConfigurationProperty("genreSubstitution", IsDefaultCollection = true, IsRequired = true)]
        public GenresCollection GenreSubstitutions
        {
            get
            {
                return (GenresCollection)this["genreSubstitution"];
            }
        }
        [ConfigurationProperty("encodings", IsDefaultCollection = true, IsRequired = true)]
        public EncodingsCollection Encodings
        {
            get
            {
                return (EncodingsCollection)this["encodings"];
            }
        }
        [ConfigurationProperty("renameProfiles", IsDefaultCollection = true, IsRequired = true)]
        public RenameProfilesCollection RenameProfiles
        {
            get
            {
                return (RenameProfilesCollection)this["renameProfiles"];
            }
        }
        [ConfigurationProperty("commands", IsRequired = true)]
        public CommandTypeElement Commands
        {
            get
            {
                return (CommandTypeElement)this["commands"];
            }
            set
            {
                this["commands"] = value;
            }
        }
    }
    public class FB2Config
    {
        private static FB2ConfigurationSection _fb2Configuration = null;
        public static FB2ConfigurationSection Current
        {
            get
            {
                if (_fb2Configuration == null)
                    _fb2Configuration = System.Configuration.ConfigurationManager.GetSection("FB2") as FB2ConfigurationSection;
                return _fb2Configuration;
            }
        }
    }
}