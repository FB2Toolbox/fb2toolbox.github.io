using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Ionic.Zip;
using System.Xml.Schema;
using System.Resources;
using System.Xml.XPath;

namespace FB2Toolbox
{
    public sealed class FB2EncoderFallbackBuffer : EncoderFallbackBuffer
    {
        #region FB2EncoderFallbackBuffer
        // Store our fallback string
        private String strFallback = String.Empty;
        int fallbackCount = -1;
        int fallbackIndex = -1;
        // Construction
        public FB2EncoderFallbackBuffer()
        {
        }
        // Fallback Methods
        public override bool Fallback(char charUnknown, int index)
        {
            // If we had a buffer already we're being recursive, throw, it's probably at the suspect
            // character in our array.
            if (this.fallbackCount >= 1)
                // Presumably you'd want a prettier exception:
                throw new Exception("Recursive Fallback Exception");

            // Go ahead and get our fallback
            this.strFallback = String.Format("&#{0};", (int)charUnknown);
            this.fallbackCount = strFallback.Length;
            this.fallbackIndex = -1;

            return this.fallbackCount != 0;
        }
        public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
        {
            // In this example, we didn't really expect surrogates.

            // If we had a buffer already we're being recursive, throw, it's probably at the suspect
            // character in our array.
            if (this.fallbackCount >= 1)
                // Presumably you'd want a prettier exception:
                throw new Exception("Recursive Fallback Exception");

            // Go ahead and get our fallback
            // Note that we're doing this 2X, once for each char.  That won't effect the
            // EncoderNumberFallback.MaxCharCount though because it is counting per char,
            // and although we're 2X that here, we also have 2x chars.
            this.strFallback = String.Format("&#{0};&#{1};", (int)charUnknownHigh, (int)charUnknownLow);
            this.fallbackCount = strFallback.Length;
            this.fallbackIndex = -1;

            return this.fallbackCount != 0;
        }
        public override char GetNextChar()
        {
            // We want it to get < 0 because == 0 means that the current/last character is a fallback
            // and we need to detect recursion.  We could have a flag but we already have this counter.
            this.fallbackCount--;
            this.fallbackIndex++;

            // Do we have anything left? 0 is now last fallback char, negative is nothing left
            if (this.fallbackCount < 0)
                return (char)0;

            // Need to get it out of the buffer.
            return this.strFallback[this.fallbackIndex];
        }
        public override bool MovePrevious()
        {
            // Back up one, only if we just processed the last character (or earlier)
            if (this.fallbackCount >= -1 && this.fallbackIndex >= 0)
            {
                this.fallbackIndex--;
                this.fallbackCount++;
                return true;
            }

            // Return false 'cause we couldn't do it.
            return false;
        }
        // How many characters left to output?
        public override int Remaining
        {
            get
            {
                // Our count is 0 for 1 character left.
                return (this.fallbackCount < 0) ? 0 : this.fallbackCount;
            }
        }
        #endregion
    }
    public class FBEncoderFallback : EncoderFallback
    {
        #region FBEncoderFallback
        public override EncoderFallbackBuffer CreateFallbackBuffer()
        {
            return new FB2EncoderFallbackBuffer();
        }
        public override int MaxCharCount
        {
            get { return 8; }
        }
        public FBEncoderFallback()
        {
        }
        #endregion
    }
    internal static class NativeMethods
    {
        #region Private
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ThrowOnUnmappableChar=true)]
        private static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32.dll")]
        private static extern bool FindClose(IntPtr hFindFile);
        #endregion
        public static bool CheckDirectoryEmpty_Fast(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(path);
            }

            if (Directory.Exists(path))
            {
                if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    path += "*";
                else
                    path += Path.DirectorySeparatorChar + "*";

                WIN32_FIND_DATA findData;
                var findHandle = FindFirstFile(path, out findData);

                if (findHandle != INVALID_HANDLE_VALUE)
                {
                    try
                    {
                        bool empty = true;
                        do
                        {
                            if (findData.cFileName != "." && findData.cFileName != "..")
                                empty = false;
                        } while (empty && FindNextFile(findHandle, out findData));

                        return empty;
                    }
                    finally
                    {
                        FindClose(findHandle);
                    }
                }

                throw new Exception("Failed to get directory first file");
            }

            throw new DirectoryNotFoundException();
        }
    }
    public class FileProperties
    {
        public bool AuthorFirstNameChange { get; set; }
        public string AuthorFirstName { get; set; }
        public bool AuthorLastNameChange { get; set; }
        public string AuthorLastName { get; set; }
        public bool AuthorMiddleNameChange { get; set; }
        public string AuthorMiddleName { get; set; }
        public bool GengeChange { get; set; }
        public string Genre { get; set; }
        public string GenreTitle { get; set; }
        public bool SeriesChange { get; set; }
        public string Series { get; set; }
        public bool NumberChange { get; set; }
        public string Number { get; set; }
        public bool TitleChange { get; set; }
        public string Title { get; set; }
    }
    public class FileOperationResult
    {
        public string NewFullName { get; set; }
        public string NewFileName { get; set; }
        public bool Skipped { get; set; }
    }
    public enum DescriptionElements
    {
        #region DescriptionElements elements
        Genre,
        Title,
        SequenceName,
        SequenceNr,
        Version,

        AuthorFirstName,
        AuthorMiddleName,
        AuthorLastName,
        AuthorFirstName1,
        AuthorMiddleName1,
        AuthorLastName1,

        TranslatorFirstName,
        TranslatorMiddleName,
        TranslatorLastName,
        TranslatorFirstName1,
        TranslatorMiddleName1,
        TranslatorLastName1,
        Lang
        #endregion
    }
    public class FileMetadata
    {
        #region Private
        private Dictionary<string, string> _metadataItems = new Dictionary<string, string>();
        private string _description = String.Empty;
        private bool _initialized = false;
        #endregion
        private void InternalAddMetadata(string key, string value)
        {
            var _key = key;
            var _value = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            if (Metadata.ContainsKey(_key))
                Metadata[_key] = _value;
            else
                Metadata.Add(_key, _value);
        }
        private Dictionary<string, string> Metadata
        {
            get
            {
                if (_metadataItems.Count == 0)
                    InternalInitialize();
                return _metadataItems;
            }
        }
        protected void SetDescription(string description)
        {
            _description = description;
        }
        protected virtual void InternalInitialize()
        {
            if (_initialized)
                return;
            _initialized = true;
            var elements = Enum.GetValues(typeof(DescriptionElements)) as DescriptionElements[];
            foreach (var element in elements)
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
            foreach (var item in Metadata)
            {
                if (part.Contains(string.Format("({0})", item.Key)) || part.Contains(string.Format("[{0}]", item.Key)))
                    if (string.IsNullOrEmpty(item.Value))
                        return false;
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
                InternalAddMetadata(name, value);
            name = name + Convert.ToString(index + 1);
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
            var _key = string.Format("{0}", name);
            return Metadata[_key];
        }
        public string SubstitutePart(string part)
        {
            if (CheckRequiredAttribute(part))
            {
                foreach (var item in Metadata)
                {
                    part = part.Replace(string.Format("({0})", item.Key), item.Value);
                    part = part.Replace(string.Format("[{0}]", item.Key), string.Empty);
                }
                return part;
            }
            return string.Empty;
        }
        public string Description
        {
            get
            {
                return _description;
            }
        }
        public FileMetadata()
        {
        }
    }
    public class FB2Metadata : FileMetadata
    {
        #region Private
        private int bookTitleAuthor = 0;
        private int bookTitleTranslator = 0;
        private int bookTitleGenre = 0;
        #endregion
        private void ParseBookTitleAuthor(string author)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(author)))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "first-name"))
                    {
                        var bookAuthorFirstName = reader.ReadString();
                        if (FB2Config.Current.NormalizeNames)
                            bookAuthorFirstName = NormalizeString(bookAuthorFirstName);
                        AddMetadata(DescriptionElements.AuthorFirstName, bookTitleAuthor, bookAuthorFirstName);
                        AddMetadata(DescriptionElements.AuthorFirstName1, bookTitleAuthor, string.IsNullOrEmpty(bookAuthorFirstName) ? string.Empty : bookAuthorFirstName[0].ToString());
                    }
                    else
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "last-name"))
                        {
                            var bookAuthorLastName = reader.ReadString();
                            if (FB2Config.Current.NormalizeNames)
                                bookAuthorLastName = NormalizeString(bookAuthorLastName);
                            AddMetadata(DescriptionElements.AuthorLastName, bookTitleAuthor, bookAuthorLastName);
                            AddMetadata(DescriptionElements.AuthorLastName1, bookTitleAuthor, string.IsNullOrEmpty(bookAuthorLastName) ? string.Empty : bookAuthorLastName[0].ToString());
                        }
                        else
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "middle-name"))
                            {
                                var bookAuthorMiddleName = reader.ReadString();
                                if (FB2Config.Current.NormalizeNames)
                                    bookAuthorMiddleName = NormalizeString(bookAuthorMiddleName);
                                AddMetadata(DescriptionElements.AuthorMiddleName, bookTitleAuthor, bookAuthorMiddleName);
                                AddMetadata(DescriptionElements.AuthorMiddleName1, bookTitleAuthor, string.IsNullOrEmpty(bookAuthorMiddleName) ? string.Empty : bookAuthorMiddleName[0].ToString());
                            }
                }
            }
        }
        private void ParseBookTitleTranslator(string translator)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(translator)))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "first-name"))
                    {
                        var bookTranslatorFirstName = reader.ReadString();
                        if (FB2Config.Current.NormalizeNames)
                            bookTranslatorFirstName = NormalizeString(bookTranslatorFirstName);
                        AddMetadata(DescriptionElements.TranslatorFirstName, bookTitleTranslator, bookTranslatorFirstName);
                        AddMetadata(DescriptionElements.TranslatorFirstName1, bookTitleTranslator, string.IsNullOrEmpty(bookTranslatorFirstName) ? string.Empty : bookTranslatorFirstName[0].ToString());
                    }
                    else
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "last-name"))
                        {
                            var bookTranslatorLastName = reader.ReadString();
                            if (FB2Config.Current.NormalizeNames)
                                bookTranslatorLastName = NormalizeString(bookTranslatorLastName);
                            AddMetadata(DescriptionElements.TranslatorLastName, bookTitleTranslator, bookTranslatorLastName);
                            AddMetadata(DescriptionElements.TranslatorLastName1, bookTitleTranslator, string.IsNullOrEmpty(bookTranslatorLastName) ? string.Empty : bookTranslatorLastName[0].ToString());
                        }
                        else
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "middle-name"))
                            {
                                var bookTranslatorMiddleName = reader.ReadString();
                                if (FB2Config.Current.NormalizeNames)
                                    bookTranslatorMiddleName = NormalizeString(bookTranslatorMiddleName);
                                AddMetadata(DescriptionElements.TranslatorMiddleName, bookTitleTranslator, bookTranslatorMiddleName);
                                AddMetadata(DescriptionElements.TranslatorMiddleName1, bookTitleTranslator, string.IsNullOrEmpty(bookTranslatorMiddleName) ? string.Empty : bookTranslatorMiddleName[0].ToString());
                            }
                }
            }

        }
        private void ParseTitleInfo(string titleInfo)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(titleInfo)))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "author"))
                    {
                        string author = reader.ReadOuterXml();
                        author = author.Trim();
                        ParseBookTitleAuthor(author);
                        bookTitleAuthor++;
                    }
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "translator"))
                    {
                        string translator = reader.ReadOuterXml();
                        translator = translator.Trim();
                        ParseBookTitleTranslator(translator);
                        bookTitleTranslator++;
                    }
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "book-title"))
                    {
                        var bookTitle = reader.ReadString();
                        AddMetadata(DescriptionElements.Title, bookTitle);
                    }
                    else
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "genre"))
                        {
                            var bookGenre = reader.ReadString();
                            bookGenre = FB2Config.Current.GenreSubstitutions.FindSubstitution(bookGenre);
                            if (bookTitleGenre == 0)
                                AddMetadata(DescriptionElements.Genre, bookGenre);
                            AddMetadata(DescriptionElements.Genre, bookTitleGenre, bookGenre);
                            bookTitleGenre++;
                        }
                        else
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "sequence"))
                            {
                                var bookSequenceName = reader.GetAttribute("name");
                                bookSequenceName = bookSequenceName.Trim();
                                AddMetadata(DescriptionElements.SequenceName, bookSequenceName);
                                string tmp = reader.GetAttribute("number");
                                try
                                {
                                    int tmpi = tmp != null && tmp.Trim() != string.Empty ? Int32.Parse(tmp) : 0;
                                    if ((tmpi > 0) && !string.IsNullOrEmpty(bookSequenceName))
                                        AddMetadata(DescriptionElements.SequenceNr, Convert.ToString(tmpi));
                                }
                                catch (Exception ex)
                        {
                                }
                            }
                            else
                                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "lang"))
                                {
                                    var bookLang = reader.ReadString();
                                    AddMetadata(DescriptionElements.Lang, bookLang);
                                }
                }
            }
        }
        private void ParseDocumentInfo(string documentInfo)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(documentInfo)))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "version"))
                    {
                        var bookVersion = reader.ReadString();
                        AddMetadata(DescriptionElements.Version, bookVersion);
                        break;
                    }
                }
            }
        }
        protected override void InternalParseDescription(string description)
        {
            base.InternalParseDescription(description);
            SetDescription(description);
            using (XmlReader reader = XmlReader.Create(new StringReader(description)))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "title-info"))
                    {
                        string titleInfo = reader.ReadOuterXml();
                        titleInfo = titleInfo.Trim();
                        ParseTitleInfo(titleInfo);
                    }
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "document-info"))
                    {
                        string documentInfo = reader.ReadOuterXml();
                        documentInfo = documentInfo.Trim();
                        ParseDocumentInfo(documentInfo);
                    }
                }
            }
            if (string.IsNullOrEmpty(GetMetadata(DescriptionElements.SequenceName)))
            {
                AddMetadata(DescriptionElements.SequenceNr, string.Empty);
            }
        }
        protected override void InternalInitialize()
        {
            bookTitleAuthor = 0;
            bookTitleTranslator = 0;
            bookTitleGenre = 0;
            base.InternalInitialize();
        }
        public FB2Metadata()
            : base()
        {
        }
        public FB2Metadata(string description)
            : this()
        {
            ParseDescription(description);
        }
    }
    public class FB2File : IComparable
    {
        #region Private
        private const string fb2xmlns = "http://www.gribuser.ru/xml/fictionbook/2.0";
        private bool _isValid = true;
        private FileMetadata _metadata = new FileMetadata();
        private string _errors = String.Empty;
        private List<string> validationSchemaErrors = new List<string>();
        private static XmlSchema FictionBook { get; set; }
        private static XmlSchema FictionBookGenres { get; set; }
        private static XmlSchema FictionBookLang { get; set; }
        private static XmlSchema FictionBookLinks { get; set; }
        #endregion
        #region IComparable Members
        public int CompareTo(object obj)
        {
            FB2File fc = obj as FB2File;
            if (fc == null)
                throw new InvalidCastException();
            int result = String.Compare(this.BookAuthorLastName, fc.BookAuthorLastName, StringComparison.InvariantCultureIgnoreCase);
            if (result == 0)
                result = String.Compare(this.BookAuthorFirstName, fc.BookAuthorFirstName, StringComparison.InvariantCultureIgnoreCase);
            if (result == 0)
                result = String.Compare(this.BookSequenceName, fc.BookSequenceName, StringComparison.InvariantCultureIgnoreCase);
            if (result == 0)
                result = Comparer<int?>.Default.Compare(BookSequenceNr, fc.BookSequenceNr);
            if (result == 0)
                result = String.Compare(this.BookTitle, fc.BookTitle, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }
        #endregion
        private static void LoadSchemas()
        {
            if (FictionBook == null)
            {
                FictionBook = GetEmbeddedSchema("FB2Toolbox.Validation.FictionBook2.1.xsd");
                FictionBookGenres = GetEmbeddedSchema("FB2Toolbox.Validation.FictionBookGenres.xsd");
                FictionBookLang = GetEmbeddedSchema("FB2Toolbox.Validation.FictionBookLang.xsd");
                FictionBookLinks = GetEmbeddedSchema("FB2Toolbox.Validation.FictionBookLinks.xsd");
            }
        }
        private void SchemaValidation(object sender, ValidationEventArgs e)
        {
            validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationError, e.Exception.LineNumber, e.Exception.LinePosition, e.Message));
        }
        private static XmlSchema GetEmbeddedSchema(string resourceName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                return XmlSchema.Read(stream, null);
            }
        }
        private void ParseEncoding(string encoding)
        {
            BookEncoding = encoding;
            BookInternalEncoding = encoding;
            if (FB2Config.Current.Encodings.TranslateEncodings)
            {
                try
                {
                    Encoding enc = Encoding.GetEncoding(encoding);
                    BookEncoding = enc.EncodingName;
                }
                catch (Exception ex)
                {
                }
            }
        }
        private void ParseStream(Stream stream)
        {
            stream.Position = 0;
            using (XmlReader reader = XmlReader.Create(stream))
            {
                ClearFields();
                // reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        ParseEncoding(reader.GetAttribute("encoding"));
                    }
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "description"))
                    {
                        string description = reader.ReadOuterXml();
                        description = description.Trim();
                        _metadata = new FB2Metadata(description);
                        break;
                    }
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "body"))
                        break;
                }
            }
        }
        private Stream GetFileReadStream(string fileName)
        {
            Stream stream = new MemoryStream();
            if (fileName.ToLower().EndsWith(FB2Config.Current.FB2Extension))
            {
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
                if (fileName.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension))
                {
                    Encoding zipEncoding = Encoding.GetEncoding(FB2Config.Current.Encodings.CompressionEncoding);
                    using (ZipFile zip = ZipFile.Read(fileName, new ReadOptions() { Encoding = Encoding.GetEncoding(866) }))
                    {
                        if (zip.Count <= 0)
                        {
                            throw new Exception(Properties.Resources.ZipErrorNoFiles);
                        }
                        if (zip.Count > 1)
                        {
                            throw new Exception(Properties.Resources.ZipErrorMoreThanOneFile);
                        }
                        if (!zip[0].FileName.ToLower().EndsWith(FB2Config.Current.FB2Extension))
                        {
                            throw new Exception(Properties.Resources.ZipErrorNoFB2);
                        }
                        foreach (ZipEntry entry in zip)
                        {
                            entry.Extract(stream);
                        }
                    }
                }
            stream.Position = 0;
            return stream;
        }
        private void ParseFile(string fileName)
        {
            Stream stream = GetFileReadStream(fileName);
            ParseStream(stream);
            stream.Close();
            Validate();
        }
        private void Validate()
        {
            _errors = String.Empty;
            _isValid = (!String.IsNullOrEmpty(BookTitle)) /*&& (!String.IsNullOrEmpty(BookAuthorLastName))*/ && (!String.IsNullOrEmpty(BookGenre));
            if (!IsValid)
            {
                if (String.IsNullOrEmpty(BookGenre))
                    _errors += Properties.Resources.ParseFileErrorNoBookGenre + " ";
                if (String.IsNullOrEmpty(BookTitle))
                    _errors += Properties.Resources.ParseFileErrorNoBookTitle + " ";
                /*if (String.IsNullOrEmpty(BookAuthorLastName))
                    _errors += Properties.Resources.ParseFileErrorNoAuthorLastName + " ";*/
                _errors = _errors.Trim();
            }
        }
        private void ClearFields()
        {
            BookEncoding = String.Empty;
            BookInternalEncoding = string.Empty;
        }
        internal bool IsValid
        {
            get
            {
                return _isValid;
            }
        }
        internal string Error()
        {
            return _errors;
        }
        internal void Reload()
        {
            try
            {
                UpdateFileInfo(FileInformation.FullName);
                ParseFile(FileInformation.FullName);
            }
            catch (Exception ex)
            {
            }
        }
        protected string SubstituteCharacters(CharacterSubstitutionCollection substitutionCollection, string value)
        {
            if (substitutionCollection == null)
                return value;
            if (substitutionCollection.Count == 0)
                return value;
            foreach (CharacterSubstitutionElement el in substitutionCollection)
            {
                for(int i = 0; i < el.Repeat; i++)
                    value = value.Replace(el.From, el.To);
            }
            return value;
        }
        protected string CalculateNewFileName(RenameProfileElement profile, bool useTranslit)
        {
            string fn = String.Empty;
            string extension = String.Empty;
            if (FileInformation.Name.ToLower().EndsWith(FB2Config.Current.FB2Extension))
                extension = FB2Config.Current.FB2Extension;
            else
                if (FileInformation.Name.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension))
                    extension = FB2Config.Current.FB2ZIPExtension;
            foreach (string part in profile.FileName.Split(new char[] {'|'}))
            {
                fn += Metadata.SubstitutePart(part);
            }
            fn = fn.Replace("\\", String.Empty);
            fn = fn.Trim();
            fn = SubstituteCharacters(profile.CharacterSubstitution, fn);
            if (useTranslit)
                fn = SubstituteCharacters(FB2Config.Current.RenameProfiles.GlobalTranslit, fn);
            fn = SubstituteCharacters(FB2Config.Current.RenameProfiles.GlobalCharacterSubstitution, fn);
            if (!fn.ToLower().EndsWith(extension))
                fn += extension;
            return fn;
        }
        protected string CalculateNewPath(RenameProfileElement profile, bool useTranslit)
        {
            string fn = String.Empty;
            foreach (string part in profile.Path.Split(new char[] { '|' }))
            {
                 fn += Metadata.SubstitutePart(part);
            }
            fn = SubstituteCharacters(profile.CharacterSubstitution, fn);
            if (useTranslit)
                fn = SubstituteCharacters(FB2Config.Current.RenameProfiles.GlobalTranslit, fn);
            fn = SubstituteCharacters(FB2Config.Current.RenameProfiles.GlobalCharacterSubstitution, fn);
            return fn;
        }
        public static void RemoveFolder(DirectoryInfo folder)
        {
            try
            {
                if (folder == null)
                    return;
                if (NativeMethods.CheckDirectoryEmpty_Fast(folder.FullName))
                {
                    folder.Delete();
                    if (folder.FullName != folder.Root.FullName)
                    {
                        RemoveFolder(folder.Parent);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public bool IsZIP()
        {
            return IsZIP(FileInformation.FullName);
        }
        public bool IsZIP(string fileName)
        {
            return fileName.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension);
        }
        public string BookSizeText
        {
            get
            {
                return String.Format(Properties.Resources.FileSizeText, FileInformation.Length / 1024);
            }
        }
        public FileInfo FileInformation { get; private set; }
        public string BookAuthorFirstName
        {
            get
            {
                return Metadata.GetMetadata(DescriptionElements.AuthorFirstName);
            }
        }
        public string BookAuthorLastName
        {
            get
            {
                return Metadata.GetMetadata(DescriptionElements.AuthorLastName);
            }
        }
        public string BookAuthorMiddleName
        {
            get
            {
                return Metadata.GetMetadata(DescriptionElements.AuthorMiddleName);
            }
        }
        public string BookGenre
        {
            get
            {
                return Metadata.GetMetadata(DescriptionElements.Genre);
            }
        }
        public string BookEncoding { get; private set; }
        protected internal string BookInternalEncoding { get; private set; }
        public string BookTitle
        {
            get
            {
                return Metadata.GetMetadata(DescriptionElements.Title);
            }
        }
        public string BookSequenceName
        {
            get
            {
                return Metadata.GetMetadata(DescriptionElements.SequenceName);
            }
        }
        public string BookVersion
        {
            get
            {
                return Metadata.GetMetadata(DescriptionElements.Version);
            }
        }
        public int? BookSequenceNr
        {
            get
            {
                string bsn = Metadata.GetMetadata(DescriptionElements.SequenceNr);
                if (string.IsNullOrEmpty(bsn))
                    return null;
                return Int32.Parse(bsn);
            }
        }
        public string BookLang
        {
            get
            {
                return Metadata.GetMetadata(DescriptionElements.Lang);
            }
        }
        public FileMetadata Metadata
        {
            get
            {
                return _metadata;
            }
        }
        public bool IsSkipFile(string newFullName, string newFileName)
        {
            bool skip = false;
            if (File.Exists(newFullName))
            {
                OverwriteDialog dialog = new OverwriteDialog(newFullName);
                skip = dialog.CheckSkip();
            }
            return skip;
        }
        public FileOperationResult MoveTo(string targetFolder, RenameProfileElement profile, bool useTranslit)
        {
            string newPath = CalculateNewPath(profile, useTranslit);
            newPath = Path.Combine(targetFolder, newPath);
            string newFileName = CalculateNewFileName(profile, useTranslit);
            string newFullName = Path.Combine(newPath, newFileName);
            Directory.CreateDirectory(newPath);
            DirectoryInfo di = FileInformation.Directory;
            FileOperationResult result = new FileOperationResult() { NewFileName = newFileName, NewFullName = newFullName };
            result.Skipped = FileInformation.FullName.ToLower() == newFullName.ToLower() || IsSkipFile(newFullName, newFileName);

            if (!result.Skipped)
            {
                if (File.Exists(newFullName))
                    File.Delete(newFullName);
                FileInformation.MoveTo(newFullName);
                UpdateFileInfo(newFullName);
                RemoveFolder(di);
            }
            return result;
        }
        public FileOperationResult CopyTo(string targetFolder, RenameProfileElement profile, bool useTranslit)
        {
            string newPath = CalculateNewPath(profile, useTranslit);
            newPath = Path.Combine(targetFolder, newPath);
            string newFileName = CalculateNewFileName(profile, useTranslit);
            string newFullName = Path.Combine(newPath, newFileName);
            Directory.CreateDirectory(newPath);
            FileOperationResult result = new FileOperationResult() { NewFileName = newFileName, NewFullName = newFullName };
            result.Skipped = IsSkipFile(newFullName, newFileName);
            if (!result.Skipped)
            {
                FileInformation.CopyTo(newFullName, true);
                UpdateFileInfo(newFullName);
            }
            return result;
        }
        public bool Extract()
        {
            string fileName = FileInformation.FullName;
            if (fileName.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension))
            {
                fileName = fileName.Substring(0, fileName.Length - FB2Config.Current.FB2ZIPExtension.Length) + FB2Config.Current.FB2Extension;
                Encoding zipEncoding = Encoding.GetEncoding(FB2Config.Current.Encodings.CompressionEncoding);
                using (ZipFile zip = ZipFile.Read(FileInformation.FullName, new ReadOptions() { Encoding = Encoding.GetEncoding(866) }))
                {
                    zip[0].Extract(FileInformation.Directory.FullName, ExtractExistingFileAction.Throw);
                    fileName = Path.Combine(FileInformation.Directory.FullName, zip[0].FileName);
                }
                FileInformation.Delete();
                UpdateFileInfo(fileName);
                Reload();
                return true;
            }
            return false;
        }
        private void ValidateEmptyTags(XPathDocument xmlDoc)
        {
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XPathNodeIterator iterator = navigator.Select("//*");
            while (iterator.MoveNext())
            {
                XPathNavigator curr = iterator.Current;
                if (!curr.HasChildren && !curr.Name.EndsWith("empty-line") && !curr.Name.EndsWith("image") && !curr.Name.EndsWith("sequence") && String.IsNullOrEmpty(curr.Value))
                {
                    IXmlLineInfo info = curr as IXmlLineInfo;
                    validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationWarning, info.LineNumber, info.LinePosition, string.Format(Properties.Resources.ValidationErrorEmptyTag, curr.Name)));
                }
            }
        }
        private void ValidateLinks(XPathDocument xmlDoc)
        {
            string defaultNamespace = "http://www.w3.org/1999/xlink";
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XmlNamespaceManager nsm = new XmlNamespaceManager(navigator.NameTable);
            nsm.AddNamespace("xlink", defaultNamespace);
            nsm.AddNamespace("l", defaultNamespace);


            Dictionary<string, string> idList = new Dictionary<string, string>();
            Dictionary<string, string> hrefList = new Dictionary<string, string>();

            XPathNodeIterator ids = navigator.Select("//*[@id]", nsm);
            while (ids.MoveNext())
            {
                XPathNavigator curr = ids.Current;
                IXmlLineInfo info = curr as IXmlLineInfo;
                string id = curr.GetAttribute("id", String.Empty);
                if (!idList.ContainsKey(id))
                {
                    idList.Add(id, String.Format("{0}|{1}", info.LineNumber, info.LinePosition));
                }
                else
                {
                    validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationWarning, info.LineNumber, info.LinePosition, String.Format(Properties.Resources.ValidationErrorDuplicateId, id)));
                }
            }

            XPathNodeIterator iterator = navigator.Select("//*[@xlink:href|@l:href]", nsm);
            while (iterator.MoveNext())
            {
                XPathNavigator curr = iterator.Current;
                string error = String.Empty;
                string href = curr.GetAttribute("href", defaultNamespace);
                string type = curr.GetAttribute("type", defaultNamespace);
                if (string.IsNullOrEmpty(href))
                    error = String.Format(Properties.Resources.ValidationErrorEmptyLink, curr.Name);
                else
                    if (!href.StartsWith("#"))
                    {
                        if (curr.Name.EndsWith("}image"))
                            error = String.Format(Properties.Resources.ValidationErrorExternalLink, href);
                        else
                            if (type == "note")
                                error = String.Format(Properties.Resources.ValidationErrorExternalNote, href);
                            else
                                if (!(href.StartsWith("http:") || href.StartsWith("https:") || href.StartsWith("ftp:") || href.StartsWith("mailto:")))
                                    error = String.Format(Properties.Resources.ValidationErrorInvalidExternalLink, href);
                                else
                                    error = String.Format(Properties.Resources.ValidationErrorLocalExternalLink, href);
                    }
                    else
                    {
                        string id = href.Remove(0, 1);
                        if (!hrefList.ContainsKey(id))
                            hrefList.Add(id, String.Empty);
                        if (!idList.ContainsKey(id))
                            error = String.Format(Properties.Resources.ValidationErrorReferenceToAnUnknown, href);
                    }
                if (!String.IsNullOrEmpty(error))
                {
                    IXmlLineInfo info = curr as IXmlLineInfo;
                    validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationWarning, info.LineNumber, info.LinePosition, error));
                }
            }
            foreach (var item in idList)
            {
                if (!hrefList.ContainsKey(item.Key))
                {
                    var parts = item.Value.Split('|');
                    validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationWarning, parts[0], parts[1], String.Format(Properties.Resources.ValidationErrorNoLinksToObject, item.Key)));
                }
            }
        }
        public List<string> ValidateSchema()
        {
            validationSchemaErrors.Clear();
            LoadSchemas();
            using (Stream stream = GetFileReadStream(FileInformation.FullName))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.CheckCharacters = true;
                settings.ValidationEventHandler += new ValidationEventHandler(SchemaValidation);
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(FictionBook);
                settings.Schemas.Add(FictionBookGenres);
                settings.Schemas.Add(FictionBookLang);
                settings.Schemas.Add(FictionBookLinks);
                XmlReader reader = XmlReader.Create(stream, settings);
                while (reader.Read()) { }
                //reader.Close();
            }

            using (Stream vstream = GetFileReadStream(FileInformation.FullName))
            {
                XPathDocument xmlDoc = new XPathDocument(vstream);

                ValidateEmptyTags(xmlDoc);
                ValidateLinks(xmlDoc);
            }
            return validationSchemaErrors;
        }
        public bool Compress()
        {
            string fileName = FileInformation.FullName;
            if (fileName.ToLower().EndsWith(FB2Config.Current.FB2Extension))
            {
                fileName = fileName.Substring(0, fileName.Length - FB2Config.Current.FB2Extension.Length) + FB2Config.Current.FB2ZIPExtension;
                Encoding zipEncoding = Encoding.GetEncoding(FB2Config.Current.Encodings.CompressionEncoding);
                using (ZipFile zip = new ZipFile(fileName, zipEncoding))
                {
                    zip.AddFile(FileInformation.FullName, String.Empty);
                    zip.Save();
                }
                FileInformation.Delete();
                UpdateFileInfo(fileName);
                Reload();
                return true;
            }
            return false;
        }
        public FileOperationResult RenameTo(RenameProfileElement profile, bool useTranslit)
        {
            string newFileName = CalculateNewFileName(profile, useTranslit);
            string newFullName = Path.Combine(FileInformation.Directory.FullName, newFileName);
            FileOperationResult result = new FileOperationResult() { NewFileName = newFileName, NewFullName = newFullName };
            // Skip renaming if it is the same file
            if (newFullName.ToLowerInvariant() == FileInformation.FullName.ToLowerInvariant())
            {
                result.Skipped = true;
                return result;
            }
            result.Skipped = IsSkipFile(newFullName, newFileName);
            if (!result.Skipped)
            {
                if (File.Exists(newFullName))
                    File.Delete(newFullName);
                FileInformation.MoveTo(newFullName);
                UpdateFileInfo(newFullName);
            }
            return result;
        }
        public void UpdateFileInfo(string fileName)
        {
            FileInformation = new FileInfo(fileName);
        }
        public void UpdateProperties(FileProperties props)
        {
            Stream stream = GetFileReadStream(FileInformation.FullName);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            stream.Close();

            // Add the namespace.
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("FB2", fb2xmlns);

            // Select and display the first node in which the author's 
            // last name is Kingsolver.
            XmlNode titleInfo = doc.SelectSingleNode(
                "/FB2:FictionBook/FB2:description/FB2:title-info", nsmgr);

            var author = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author", nsmgr);
            if (author == null)
            {
                author = doc.CreateElement("author", fb2xmlns);
                titleInfo.AppendChild(author);
            }
            if (props.AuthorLastNameChange)
            {
                var lastName = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:last-name", nsmgr);
                if (lastName == null)
                {
                    lastName = doc.CreateElement("last-name", fb2xmlns);
                    author.AppendChild(lastName);
                }
                lastName.InnerText = props.AuthorLastName;
            }
            if (props.AuthorFirstNameChange)
            {
                var firstName = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:first-name", nsmgr);
                if (firstName == null)
                {
                    firstName = doc.CreateElement("first-name", fb2xmlns);
                    author.AppendChild(firstName);
                }
                firstName.InnerText = props.AuthorFirstName;
            }
            if (props.AuthorMiddleNameChange)
            {
                var middleName = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:middle-name", nsmgr);
                if (middleName == null)
                {
                    middleName = doc.CreateElement("middle-name", fb2xmlns);
                    author.AppendChild(middleName);
                }
                middleName.InnerText = props.AuthorMiddleName;
            }
            // <sequence name="100 великих" number="0" />
            if (props.NumberChange || props.SeriesChange)
            {
                var sequence = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:sequence", nsmgr);
                if (sequence == null)
                {
                    sequence = doc.CreateElement("sequence", fb2xmlns);
                    titleInfo.AppendChild(sequence);
                }
                var  nameA = (sequence as XmlElement).Attributes["name"];
                var  numberA = (sequence as XmlElement).Attributes["number"];
                if (nameA == null)
                {
                    nameA = doc.CreateAttribute("name");
                    (sequence as XmlElement).Attributes.Append(nameA);
                }
                if (numberA == null)
                {
                    numberA = doc.CreateAttribute("number");
                    (sequence as XmlElement).Attributes.Append(numberA);
                }

                if (props.SeriesChange)
                    nameA.InnerText = props.Series;
                    //(sequence as XmlElement).SetAttribute("name", props.Series);
                if (props.NumberChange)
                    numberA.InnerText = props.Number;
                    //(sequence as XmlElement).SetAttribute("number", props.Number);
            }
            // <genre>ref_encyc</genre>
            if (props.GengeChange)
            {
                var genre = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:genre", nsmgr);
                if (genre == null)
                {
                    genre = doc.CreateElement("genre", fb2xmlns);
                    titleInfo.AppendChild(genre);
                }
                genre.InnerText = props.Genre;
            }
            if (props.TitleChange)
            {
                var title = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:book-title", nsmgr);
                if (title == null)
                {
                    title = doc.CreateElement("book-title", fb2xmlns);
                    titleInfo.AppendChild(title);
                }
                title.InnerText = props.Title;
            }


            if (FileInformation.FullName.ToLower().EndsWith(FB2Config.Current.FB2Extension))
            {
                string fileName = FileInformation.FullName;
                using (Stream fileStream = new FileStream(fileName + ".tmp", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlTextWriter writer = new XmlTextWriter(fileStream, Encoding.GetEncoding(BookInternalEncoding));
                    if (FB2Config.Current.Encodings.IndentFile)
                        writer.Formatting = Formatting.Indented;
                    doc.Save(writer);
                    writer.Flush();
                }
                FileInformation.Delete();
                FileInfo tmp = new FileInfo(fileName + ".tmp");
                tmp.MoveTo(fileName);
                FileInformation = new FileInfo(fileName);
            }
            else
                if (FileInformation.FullName.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension))
                {
                    string inZipFileName = String.Empty;
                    Encoding zipEncoding = Encoding.GetEncoding(FB2Config.Current.Encodings.CompressionEncoding);
                    using (ZipFile zip = ZipFile.Read(FileInformation.FullName, new ReadOptions() { Encoding = Encoding.GetEncoding(866) }))
                    {
                        inZipFileName = zip[0].FileName;
                    }
                    using (ZipFile zip = new ZipFile(zipEncoding))
                    {
                        MemoryStream memStream = new MemoryStream();
                        XmlTextWriter writer = new XmlTextWriter(memStream, Encoding.GetEncoding(BookInternalEncoding));
                        if (FB2Config.Current.Encodings.IndentFile)
                            writer.Formatting = Formatting.Indented;
                        doc.Save(writer);
                        writer.Flush();
                        memStream.Position = 0;
                        ZipEntry e = zip.AddEntry(inZipFileName, memStream);
                        zip.Save(FileInformation.FullName);
                        writer.Close();
                    }
                }
            Reload();
        }
        public void EncodeTo(Encoding enc)
        {
            Stream stream = GetFileReadStream(FileInformation.FullName);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            stream.Close();

            enc = Encoding.GetEncoding(enc.CodePage, new FBEncoderFallback(), Encoding.UTF8.DecoderFallback);

            if (FileInformation.FullName.ToLower().EndsWith(FB2Config.Current.FB2Extension))
            {
                string fileName = FileInformation.FullName;
                using (Stream fileStream = new FileStream(fileName +".tmp", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlTextWriter writer = new XmlTextWriter(fileStream, enc);
                    if (FB2Config.Current.Encodings.IndentFile)
                        writer.Formatting = Formatting.Indented;
                    doc.Save(writer);
                    writer.Flush();
                }
                FileInformation.Delete();
                FileInfo tmp = new FileInfo(fileName + ".tmp");
                tmp.MoveTo(fileName);
                FileInformation = new FileInfo(fileName);
            }
            else
                if (FileInformation.FullName.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension))
                {
                    string inZipFileName = String.Empty;
                    Encoding zipEncoding = Encoding.GetEncoding(FB2Config.Current.Encodings.CompressionEncoding);
                    using (ZipFile zip = ZipFile.Read(FileInformation.FullName, new ReadOptions() { Encoding = Encoding.GetEncoding(866) }))
                    {
                        inZipFileName = zip[0].FileName;
                    }
                    using (ZipFile zip = new ZipFile(zipEncoding))
                    {
                        MemoryStream memStream = new MemoryStream();
                        XmlTextWriter writer = new XmlTextWriter(memStream, enc);
                        if (FB2Config.Current.Encodings.IndentFile)
                            writer.Formatting = Formatting.Indented;
                        doc.Save(writer);
                        writer.Flush();
                        memStream.Position = 0;
                        ZipEntry e = zip.AddEntry(inZipFileName, memStream);
                        zip.Save(FileInformation.FullName);
                        writer.Close();
                    }
                }
            Reload();
        }
        public override string ToString()
        {
            return FileInformation.Name;
        }
        public FB2File(string fileName)
        {
            ClearFields();
            FileInformation = new FileInfo(fileName);
            ParseFile(fileName);
        }
    }
}
