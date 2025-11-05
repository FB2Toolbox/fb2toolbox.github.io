using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using Ionic.Zip;

namespace FB2Toolbox.Utilities
{
    public class FB2File : IComparable
    {
        #region Private
        private const string fb2xmlns = "http://www.gribuser.ru/xml/fictionbook/2.0";
        private string _errors = string.Empty;
        private readonly List<string> validationSchemaErrors = new List<string>();
        private static XmlSchema FictionBook { get; set; }
        private static XmlSchema FictionBookGenres { get; set; }
        private static XmlSchema FictionBookLang { get; set; }
        private static XmlSchema FictionBookLinks { get; set; }
        #endregion
        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (!(obj is FB2File fc))
            {
                throw new InvalidCastException();
            }

            int result = string.Compare(BookAuthorLastName, fc.BookAuthorLastName, StringComparison.InvariantCultureIgnoreCase);
            if (result == 0)
            {
                result = string.Compare(BookAuthorFirstName, fc.BookAuthorFirstName, StringComparison.InvariantCultureIgnoreCase);
            }

            if (result == 0)
            {
                result = string.Compare(BookSequenceName, fc.BookSequenceName, StringComparison.InvariantCultureIgnoreCase);
            }

            if (result == 0)
            {
                result = Comparer<int?>.Default.Compare(BookSequenceNr, fc.BookSequenceNr);
            }

            if (result == 0)
            {
                result = string.Compare(BookTitle, fc.BookTitle, StringComparison.InvariantCultureIgnoreCase);
            }

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
                catch (Exception)
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
                        Metadata = new FB2Metadata(description);
                        break;
                    }
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "body"))
                    {
                        break;
                    }
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
                _ = Encoding.GetEncoding(FB2Config.Current.Encodings.CompressionEncoding);
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
            _errors = string.Empty;
            IsValid = (!string.IsNullOrEmpty(BookTitle)) /*&& (!String.IsNullOrEmpty(BookAuthorLastName))*/ && (!string.IsNullOrEmpty(BookGenre));
            if (!IsValid)
            {
                if (string.IsNullOrEmpty(BookGenre))
                {
                    _errors += Properties.Resources.ParseFileErrorNoBookGenre + " ";
                }

                if (string.IsNullOrEmpty(BookTitle))
                {
                    _errors += Properties.Resources.ParseFileErrorNoBookTitle + " ";
                }
                /*if (String.IsNullOrEmpty(BookAuthorLastName))
   _errors += Properties.Resources.ParseFileErrorNoAuthorLastName + " ";*/
                _errors = _errors.Trim();
            }
        }
        private void ClearFields()
        {
            BookEncoding = string.Empty;
            BookInternalEncoding = string.Empty;
        }
        internal bool IsValid { get; private set; } = true;
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
            catch (Exception)
            {
            }
        }
        protected string SubstituteCharacters(CharacterSubstitutionCollection substitutionCollection, string value)
        {
            if (substitutionCollection == null)
            {
                return value;
            }

            if (substitutionCollection.Count == 0)
            {
                return value;
            }

            foreach (CharacterSubstitutionElement el in substitutionCollection)
            {
                for (int i = 0; i < el.Repeat; i++)
                {
                    value = value.Replace(el.From, el.To);
                }
            }
            return value;
        }
        protected string CalculateNewFileName(RenameProfileElement profile, bool useTranslit)
        {
            string fn = string.Empty;
            string extension = string.Empty;
            if (FileInformation.Name.ToLower().EndsWith(FB2Config.Current.FB2Extension))
            {
                extension = FB2Config.Current.FB2Extension;
            }
            else
                if (FileInformation.Name.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension))
            {
                extension = FB2Config.Current.FB2ZIPExtension;
            }

            foreach (string part in profile.FileName.Split(new char[] { '|' }))
            {
                fn += Metadata.SubstitutePart(part);
            }
            fn = fn.Replace("\\", string.Empty);
            fn = fn.Trim();
            fn = SubstituteCharacters(profile.CharacterSubstitution, fn);
            if (useTranslit)
            {
                fn = SubstituteCharacters(FB2Config.Current.RenameProfiles.GlobalTranslit, fn);
            }

            fn = SubstituteCharacters(FB2Config.Current.RenameProfiles.GlobalCharacterSubstitution, fn);
            if (!fn.ToLower().EndsWith(extension))
            {
                fn += extension;
            }

            return fn;
        }
        protected string CalculateNewPath(RenameProfileElement profile, bool useTranslit)
        {
            string fn = string.Empty;
            foreach (string part in profile.Path.Split(new char[] { '|' }))
            {
                fn += Metadata.SubstitutePart(part);
            }
            fn = SubstituteCharacters(profile.CharacterSubstitution, fn);
            if (useTranslit)
            {
                fn = SubstituteCharacters(FB2Config.Current.RenameProfiles.GlobalTranslit, fn);
            }

            fn = SubstituteCharacters(FB2Config.Current.RenameProfiles.GlobalCharacterSubstitution, fn);
            return fn;
        }
        public static void RemoveFolder(DirectoryInfo folder)
        {
            try
            {
                if (folder == null)
                {
                    return;
                }

                if (NativeMethods.CheckDirectoryEmpty_Fast(folder.FullName))
                {
                    folder.Delete();
                    if (folder.FullName != folder.Root.FullName)
                    {
                        RemoveFolder(folder.Parent);
                    }
                }
            }
            catch (Exception)
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
        public string BookSizeText => string.Format(Properties.Resources.FileSizeText, FileInformation.Length / 1024);
        public FileInfo FileInformation { get; private set; }
        public string BookAuthorFirstName => Metadata.GetMetadata(DescriptionElements.AuthorFirstName);
        public string BookAuthorLastName => Metadata.GetMetadata(DescriptionElements.AuthorLastName);
        public string BookAuthorMiddleName => Metadata.GetMetadata(DescriptionElements.AuthorMiddleName);
        public string BookGenre => Metadata.GetMetadata(DescriptionElements.Genre);
        public string BookEncoding { get; private set; }
        protected internal string BookInternalEncoding { get; private set; }
        public string BookTitle => Metadata.GetMetadata(DescriptionElements.Title);
        public string BookSequenceName => Metadata.GetMetadata(DescriptionElements.SequenceName);
        public string BookVersion => Metadata.GetMetadata(DescriptionElements.Version);
        public int? BookSequenceNr
        {
            get
            {
                string bsn = Metadata.GetMetadata(DescriptionElements.SequenceNr);
                return string.IsNullOrEmpty(bsn) ? null : (int?)int.Parse(bsn);
            }
        }
        public string BookLang => Metadata.GetMetadata(DescriptionElements.Lang);
        public FileMetadata Metadata { get; private set; } = new FileMetadata();
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
            _ = Directory.CreateDirectory(newPath);
            DirectoryInfo di = FileInformation.Directory;
            FileOperationResult result = new FileOperationResult
            {
                NewFileName = newFileName,
                NewFullName = newFullName,
                Skipped = FileInformation.FullName.ToLower() == newFullName.ToLower() || IsSkipFile(newFullName, newFileName)
            };

            if (!result.Skipped)
            {
                if (File.Exists(newFullName))
                {
                    File.Delete(newFullName);
                }

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
            _ = Directory.CreateDirectory(newPath);
            FileOperationResult result = new FileOperationResult
            {
                NewFileName = newFileName,
                NewFullName = newFullName,
                Skipped = IsSkipFile(newFullName, newFileName)
            };
            if (!result.Skipped)
            {
                _ = FileInformation.CopyTo(newFullName, true);
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
                _ = Encoding.GetEncoding(FB2Config.Current.Encodings.CompressionEncoding);
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
                if (!curr.HasChildren && !curr.Name.EndsWith("empty-line") && !curr.Name.EndsWith("image") && !curr.Name.EndsWith("sequence") && string.IsNullOrEmpty(curr.Value))
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
                string id = curr.GetAttribute("id", string.Empty);
                if (!idList.ContainsKey(id))
                {
                    idList.Add(id, string.Format("{0}|{1}", info.LineNumber, info.LinePosition));
                }
                else
                {
                    validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationWarning, info.LineNumber, info.LinePosition, string.Format(Properties.Resources.ValidationErrorDuplicateId, id)));
                }
            }

            XPathNodeIterator iterator = navigator.Select("//*[@xlink:href|@l:href]", nsm);
            while (iterator.MoveNext())
            {
                XPathNavigator curr = iterator.Current;
                string error = string.Empty;
                string href = curr.GetAttribute("href", defaultNamespace);
                string type = curr.GetAttribute("type", defaultNamespace);
                if (string.IsNullOrEmpty(href))
                {
                    error = string.Format(Properties.Resources.ValidationErrorEmptyLink, curr.Name);
                }
                else
                    if (!href.StartsWith("#"))
                {
                    error = curr.Name.EndsWith("}image")
                        ? string.Format(Properties.Resources.ValidationErrorExternalLink, href)
                        : type == "note"
                            ? string.Format(Properties.Resources.ValidationErrorExternalNote, href)
                            : !(href.StartsWith("http:") || href.StartsWith("https:") || href.StartsWith("ftp:") || href.StartsWith("mailto:"))
                                ? string.Format(Properties.Resources.ValidationErrorInvalidExternalLink, href)
                                : string.Format(Properties.Resources.ValidationErrorLocalExternalLink, href);
                }
                else
                {
                    string id = href.Remove(0, 1);
                    if (!hrefList.ContainsKey(id))
                    {
                        hrefList.Add(id, string.Empty);
                    }

                    if (!idList.ContainsKey(id))
                    {
                        error = string.Format(Properties.Resources.ValidationErrorReferenceToAnUnknown, href);
                    }
                }
                if (!string.IsNullOrEmpty(error))
                {
                    IXmlLineInfo info = curr as IXmlLineInfo;
                    validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationWarning, info.LineNumber, info.LinePosition, error));
                }
            }
            foreach (KeyValuePair<string, string> item in idList)
            {
                if (!hrefList.ContainsKey(item.Key))
                {
                    string[] parts = item.Value.Split('|');
                    validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationWarning, parts[0], parts[1], string.Format(Properties.Resources.ValidationErrorNoLinksToObject, item.Key)));
                }
            }
        }
        public List<string> ValidateSchema()
        {
            validationSchemaErrors.Clear();
            LoadSchemas();
            using (Stream stream = GetFileReadStream(FileInformation.FullName))
            {
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    CheckCharacters = true
                };
                settings.ValidationEventHandler += new ValidationEventHandler(SchemaValidation);
                settings.ValidationType = ValidationType.Schema;
                _ = settings.Schemas.Add(FictionBook);
                _ = settings.Schemas.Add(FictionBookGenres);
                _ = settings.Schemas.Add(FictionBookLang);
                _ = settings.Schemas.Add(FictionBookLinks);
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
                    _ = zip.AddFile(FileInformation.FullName, string.Empty);
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
                {
                    File.Delete(newFullName);
                }

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

            XmlNode author = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author", nsmgr);
            if (author == null)
            {
                author = doc.CreateElement("author", fb2xmlns);
                _ = titleInfo.AppendChild(author);
            }
            if (props.AuthorLastNameChange)
            {
                XmlNode lastName = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:last-name", nsmgr);
                if (lastName == null)
                {
                    lastName = doc.CreateElement("last-name", fb2xmlns);
                    _ = author.AppendChild(lastName);
                }
                lastName.InnerText = props.AuthorLastName;
            }
            if (props.AuthorFirstNameChange)
            {
                XmlNode firstName = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:first-name", nsmgr);
                if (firstName == null)
                {
                    firstName = doc.CreateElement("first-name", fb2xmlns);
                    _ = author.AppendChild(firstName);
                }
                firstName.InnerText = props.AuthorFirstName;
            }
            if (props.AuthorMiddleNameChange)
            {
                XmlNode middleName = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:middle-name", nsmgr);
                if (middleName == null)
                {
                    middleName = doc.CreateElement("middle-name", fb2xmlns);
                    _ = author.AppendChild(middleName);
                }
                middleName.InnerText = props.AuthorMiddleName;
            }
            // <sequence name="100 великих" number="0" />
            if (props.NumberChange || props.SeriesChange)
            {
                XmlNode sequence = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:sequence", nsmgr);
                if (sequence == null)
                {
                    sequence = doc.CreateElement("sequence", fb2xmlns);
                    _ = titleInfo.AppendChild(sequence);
                }
                XmlAttribute nameA = (sequence as XmlElement).Attributes["name"];
                XmlAttribute numberA = (sequence as XmlElement).Attributes["number"];
                if (nameA == null)
                {
                    nameA = doc.CreateAttribute("name");
                    _ = (sequence as XmlElement).Attributes.Append(nameA);
                }
                if (numberA == null)
                {
                    numberA = doc.CreateAttribute("number");
                    _ = (sequence as XmlElement).Attributes.Append(numberA);
                }

                if (props.SeriesChange)
                {
                    nameA.InnerText = props.Series;
                }
                //(sequence as XmlElement).SetAttribute("name", props.Series);
                if (props.NumberChange)
                {
                    numberA.InnerText = props.Number;
                }
                //(sequence as XmlElement).SetAttribute("number", props.Number);
            }
            // <genre>ref_encyc</genre>
            if (props.GengeChange)
            {
                XmlNode genre = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:genre", nsmgr);
                if (genre == null)
                {
                    genre = doc.CreateElement("genre", fb2xmlns);
                    _ = titleInfo.AppendChild(genre);
                }
                genre.InnerText = props.Genre;
            }
            if (props.TitleChange)
            {
                XmlNode title = doc.SelectSingleNode("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:book-title", nsmgr);
                if (title == null)
                {
                    title = doc.CreateElement("book-title", fb2xmlns);
                    _ = titleInfo.AppendChild(title);
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
                    {
                        writer.Formatting = Formatting.Indented;
                    }

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
                string inZipFileName = string.Empty;
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
                    {
                        writer.Formatting = Formatting.Indented;
                    }

                    doc.Save(writer);
                    writer.Flush();
                    memStream.Position = 0;
                    _ = zip.AddEntry(inZipFileName, memStream);
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
                using (Stream fileStream = new FileStream(fileName + ".tmp", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlTextWriter writer = new XmlTextWriter(fileStream, enc);
                    if (FB2Config.Current.Encodings.IndentFile)
                    {
                        writer.Formatting = Formatting.Indented;
                    }

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
                string inZipFileName = string.Empty;
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
                    {
                        writer.Formatting = Formatting.Indented;
                    }

                    doc.Save(writer);
                    writer.Flush();
                    memStream.Position = 0;
                    _ = zip.AddEntry(inZipFileName, memStream);
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
