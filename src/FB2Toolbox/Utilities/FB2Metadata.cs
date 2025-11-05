using System;
using System.IO;
using System.Xml;

namespace FB2Toolbox.Utilities
{
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
                        string bookAuthorFirstName = reader.ReadString();
                        if (FB2Config.Current.NormalizeNames)
                        {
                            bookAuthorFirstName = NormalizeString(bookAuthorFirstName);
                        }

                        AddMetadata(DescriptionElements.AuthorFirstName, bookTitleAuthor, bookAuthorFirstName);
                        AddMetadata(DescriptionElements.AuthorFirstName1, bookTitleAuthor, string.IsNullOrEmpty(bookAuthorFirstName) ? string.Empty : bookAuthorFirstName[0].ToString());
                    }
                    else
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "last-name"))
                    {
                        string bookAuthorLastName = reader.ReadString();
                        if (FB2Config.Current.NormalizeNames)
                        {
                            bookAuthorLastName = NormalizeString(bookAuthorLastName);
                        }

                        AddMetadata(DescriptionElements.AuthorLastName, bookTitleAuthor, bookAuthorLastName);
                        AddMetadata(DescriptionElements.AuthorLastName1, bookTitleAuthor, string.IsNullOrEmpty(bookAuthorLastName) ? string.Empty : bookAuthorLastName[0].ToString());
                    }
                    else
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "middle-name"))
                    {
                        string bookAuthorMiddleName = reader.ReadString();
                        if (FB2Config.Current.NormalizeNames)
                        {
                            bookAuthorMiddleName = NormalizeString(bookAuthorMiddleName);
                        }

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
                        string bookTranslatorFirstName = reader.ReadString();
                        if (FB2Config.Current.NormalizeNames)
                        {
                            bookTranslatorFirstName = NormalizeString(bookTranslatorFirstName);
                        }

                        AddMetadata(DescriptionElements.TranslatorFirstName, bookTitleTranslator, bookTranslatorFirstName);
                        AddMetadata(DescriptionElements.TranslatorFirstName1, bookTitleTranslator, string.IsNullOrEmpty(bookTranslatorFirstName) ? string.Empty : bookTranslatorFirstName[0].ToString());
                    }
                    else
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "last-name"))
                    {
                        string bookTranslatorLastName = reader.ReadString();
                        if (FB2Config.Current.NormalizeNames)
                        {
                            bookTranslatorLastName = NormalizeString(bookTranslatorLastName);
                        }

                        AddMetadata(DescriptionElements.TranslatorLastName, bookTitleTranslator, bookTranslatorLastName);
                        AddMetadata(DescriptionElements.TranslatorLastName1, bookTitleTranslator, string.IsNullOrEmpty(bookTranslatorLastName) ? string.Empty : bookTranslatorLastName[0].ToString());
                    }
                    else
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "middle-name"))
                    {
                        string bookTranslatorMiddleName = reader.ReadString();
                        if (FB2Config.Current.NormalizeNames)
                        {
                            bookTranslatorMiddleName = NormalizeString(bookTranslatorMiddleName);
                        }

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
                        string bookTitle = reader.ReadString();
                        AddMetadata(DescriptionElements.Title, bookTitle);
                    }
                    else
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "genre"))
                    {
                        string bookGenre = reader.ReadString();
                        bookGenre = FB2Config.Current.GenreSubstitutions.FindSubstitution(bookGenre);
                        if (bookTitleGenre == 0)
                        {
                            AddMetadata(DescriptionElements.Genre, bookGenre);
                        }

                        AddMetadata(DescriptionElements.Genre, bookTitleGenre, bookGenre);
                        bookTitleGenre++;
                    }
                    else
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "sequence"))
                    {
                        string bookSequenceName = reader.GetAttribute("name");
                        bookSequenceName = bookSequenceName.Trim();
                        AddMetadata(DescriptionElements.SequenceName, bookSequenceName);
                        string tmp = reader.GetAttribute("number");
                        try
                        {
                            int tmpi = tmp != null && tmp.Trim() != string.Empty ? int.Parse(tmp) : 0;
                            if ((tmpi > 0) && !string.IsNullOrEmpty(bookSequenceName))
                            {
                                AddMetadata(DescriptionElements.SequenceNr, Convert.ToString(tmpi));
                            }
                        }
                        catch
                        {
                        }
                    }
                    else
                                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "lang"))
                    {
                        string bookLang = reader.ReadString();
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
                        string bookVersion = reader.ReadString();
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
}
