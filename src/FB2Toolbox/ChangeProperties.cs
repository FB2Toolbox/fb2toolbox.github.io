using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FB2Toolbox
{
    public partial class ChangeProperties : Form
    {
        public ChangeProperties()
        {
            InitializeComponent();
            List<GenreSubstitutionElement> genres = new List<GenreSubstitutionElement>();
            foreach(GenreSubstitutionElement el in FB2Config.Current.GenreSubstitutions)
            {
                genres.Add(el);
                genres.Sort();
            }
            foreach (GenreSubstitutionElement el in genres)
            {
                bookGenreText.Items.Add(el);
            }
        }
        public void LoadFileProperties(FB2File info, int filesCount)
        {
            if (info != null)
            {
                authorFirstNameText.Text = info.BookAuthorFirstName;
                authorMiddleNameText.Text = info.BookAuthorMiddleName;
                authorLastNameText.Text = info.BookAuthorLastName;
                bookSeriesText.Text = info.BookSequenceName;
                bookTitleText.Text = info.BookTitle;
                foreach (GenreSubstitutionElement el in bookGenreText.Items)
                {
                    if ((el.To == info.BookGenre) || (el.From == info.BookGenre))
                    {
                        bookGenreText.SelectedItem = el;
                        break;
                    }
                }
                bookNumberText.Value = info.BookSequenceNr.HasValue ? info.BookSequenceNr.Value : 0;
            }
            bookTitleCheck.Enabled = filesCount <= 1;
            Text = "Изменить метаданные " + (filesCount > 1 ? string.Format("({0} файлов)", filesCount) : "(один файл)");
        }
        public FileProperties GetFileProperties()
        {
            FileProperties fp = new FileProperties();
            fp.AuthorFirstNameChange = authorFirstNameCheck.Checked;
            if (fp.AuthorFirstNameChange)
                fp.AuthorFirstName = authorFirstNameText.Text.Trim();
            fp.AuthorMiddleNameChange = authorMiddleNameCheck.Checked;
            if (fp.AuthorMiddleNameChange)
                fp.AuthorMiddleName = authorMiddleNameText.Text.Trim();
            fp.AuthorLastNameChange = authorLastNameCheck.Checked;
            if (fp.AuthorLastNameChange)
                fp.AuthorLastName = authorLastNameText.Text.Trim();
            fp.SeriesChange = bookSeriesCheck.Checked;
            if (fp.SeriesChange)
                fp.Series = bookSeriesText.Text.Trim();
            fp.NumberChange = bookNumberCheck.Checked;
            if (fp.NumberChange)
                fp.Number = bookNumberText.Value.ToString();
            fp.TitleChange = bookTitleCheck.Checked;
            if (fp.TitleChange)
                fp.Title = bookTitleText.Text.Trim();
            if (bookGenreText.SelectedItem != null)
            {
                fp.GengeChange = bookGenreCheck.Checked;
                if (fp.GengeChange)
                    fp.Genre = (bookGenreText.SelectedItem as GenreSubstitutionElement).From;
            }
            return fp;
        }

        private void authorFirstNameCheck_CheckedChanged(object sender, EventArgs e)
        {
            authorFirstNameText.ReadOnly = !authorFirstNameCheck.Checked;
        }

        private void authorMiddleNameCheck_CheckedChanged(object sender, EventArgs e)
        {
            authorMiddleNameText.ReadOnly = !authorMiddleNameCheck.Checked;
        }

        private void authorLastNameCheck_CheckedChanged(object sender, EventArgs e)
        {
            authorLastNameText.ReadOnly = !authorLastNameCheck.Checked;
        }

        private void bookSeriesCheck_CheckedChanged(object sender, EventArgs e)
        {
            bookSeriesText.ReadOnly = !bookSeriesCheck.Checked;
        }

        private void bookNumberCheck_CheckedChanged(object sender, EventArgs e)
        {
            bookNumberText.ReadOnly = !bookNumberCheck.Checked;
        }

        private void bookGenreCheck_CheckedChanged(object sender, EventArgs e)
        {
            bookGenreText.Enabled = bookGenreCheck.Checked;
        }

        private void bookTitleCheck_CheckedChanged(object sender, EventArgs e)
        {
            bookTitleText.ReadOnly = !bookTitleCheck.Checked;
        }
    }
}
