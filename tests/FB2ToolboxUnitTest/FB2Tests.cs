using System.ComponentModel;
using System.Text;
using AwesomeAssertions;
using FB2Toolbox;
using FB2Toolbox.Utilities;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class FB2FileTest
    {
        [Fact]
        [Trait("Category", "FB2 File")]
        [Description("Reading correct .FB2 file")]
        public void Read_Correct_FB2_File()
        {
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            file.BookTitle.Should().Be("Государь", "Book title must be 'Государь'");

            file.BookAuthorFirstName.Should().Be("Николо", "Author first name must be 'Николо'");
            file.BookAuthorMiddleName.Should().Be(string.Empty, "Author middle name must be empty string");
            file.BookAuthorLastName.Should().Be("Макиавелли", "Author last name must be 'Макиавелли'");

            file.BookSequenceName.Should().Be(string.Empty, "Sequence Name must be empty string");
            file.BookSequenceNr.Should().BeNull("Sequence Number must be null");

            file.BookGenre.Should().Be("Европейская старинная литература", "Genre must be 'Европейская старинная литература'");

            file.BookVersion.Should().Be("1.1", "Book version must be '1.1'");

            file.BookLang.Should().Be("ru", "Book language must be 'ru'");

            var win1251 = Encoding.GetEncoding(1251);
            file.BookEncoding.Should().Be(win1251.EncodingName, string.Format("Book encoding must be '{0}'", win1251.EncodingName));

            file.BookSizeText.Should().Be("352 Кб", "Book size text must be '352 Кб'");
        }

        [Fact]
        [Trait("Category", "FB2 File")]
        [Description("Reading correct .FB2.ZIP file")]
        public void Read_Correct_FB2_ZIP_File()
        {
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2.zip");
            file.BookTitle.Should().Be("Государь", "Book title must be 'Государь'");

            file.BookAuthorFirstName.Should().Be("Николо", "Author first name must be 'Николо'");
            file.BookAuthorMiddleName.Should().Be(string.Empty, "Author middle name must be empty string");
            file.BookAuthorLastName.Should().Be("Макиавелли", "Author last name must be 'Макиавелли'");

            file.BookSequenceName.Should().Be(string.Empty, "Sequence Name must be empty string");
            file.BookSequenceNr.Should().BeNull("Sequence Number must be null");

            file.BookGenre.Should().Be("Европейская старинная литература", "Genre must be 'Европейская старинная литература'");

            file.BookVersion.Should().Be("1.1", "Book version must be '1.1'");

            file.BookLang.Should().Be("ru", "Book language must be 'ru'");

            var win1251 = Encoding.GetEncoding(1251);
            file.BookEncoding.Should().Be(win1251.EncodingName, string.Format("Book encoding must be '{0}'", win1251.EncodingName));

            file.BookSizeText.Should().Be("210 Кб", "Book size text must be '210 Кб'");
        }

        [Fact]
        [Trait("Category", "FB2 File")]
        [Description("Renaming .FB2 file to the same name")]
        public void Rename_To_The_Same_Name()
        {
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            RenameProfileElement profile = null;
            foreach (RenameProfileElement rp in FB2Config.Current.RenameProfiles)
            {
                if (rp.Name == "Автор/Серия/Автор - Серия Номер - Название")
                {
                    profile = rp;
                    break;
                }
            }

            profile.Should().NotBeNull("Rename profile should not be null");

            if (profile != null)
            {
                var result = file.RenameTo(profile, false);
                result.Skipped.Should().Be(true, "File should not be renamed");
            }
        }

        [Fact]
        [Trait("Category", "FB2 File")]
        [Description("Validate the .FB2 file schema")]
        public void Validate_Correct_FB2_File()
        {
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            var errors = file.ValidateSchema();
            errors.Should().NotBeNull();
            errors.Should().BeEmpty();
        }

        [Fact]
        [Trait("Category", "FB2 File")]
        [Description("Validate the .FB2.ZIP file schema")]
        public void Validate_Correct_FB2_ZIP_File()
        {
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2.zip");

            var errors = file.ValidateSchema();
            errors.Should().NotBeNull();
            errors.Should().BeEmpty();
        }

    }
}
