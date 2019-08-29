using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FB2Toolbox.Entities
{
    public class FB2Book: IBook
    {
        #region IComparable Members
        public int CompareTo(object obj)
        {
            IBook fc = obj as IBook;
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
        public string BookAuthorFirstName { get; set; }
        public string BookAuthorLastName { get; set; }
        public string BookAuthorMiddleName { get; set; }
        public string BookGenre { get; set; }
        public string BookEncoding { get; set; }
        public string BookTitle { get; set; }
        public string BookSequenceName { get; set; }
        public string BookVersion { get; set; }
        public int? BookSequenceNr { get; set; }
        public string BookLang { get; set; }
        public string BookSizeText { get; set; }
        public string BookFile { get; set; }
        public FB2Book()
        {
            BookFile = string.Empty;
            BookAuthorFirstName = string.Empty;
            BookAuthorLastName = string.Empty;
            BookAuthorMiddleName = string.Empty;
            BookEncoding = string.Empty;
            BookTitle = string.Empty;
            BookSequenceName = string.Empty;
            BookSequenceNr = null;
            BookLang = string.Empty;
        }
    }
}
