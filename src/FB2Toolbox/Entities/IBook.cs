using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FB2Toolbox.Entities
{
    public interface IBook: IComparable
    {
        string BookFile { get; set; }
        string BookAuthorFirstName { get; set; }
        string BookAuthorLastName { get; set; }
        string BookAuthorMiddleName { get; set; }
        string BookGenre { get; set; }
        string BookEncoding { get; set; }
        string BookTitle { get; set; }
        string BookSequenceName { get; set; }
        string BookVersion { get; set; }
        int? BookSequenceNr { get; set; }
        string BookLang { get; set; }
        string BookSizeText { get; set; }
    }
}
