using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FB2Toolbox;
using System.Text;

namespace FB2ToolboxUnitTest
{
    [TestClass]
    public class FB2FileTest
    {
        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Reading correct .FB2 file")]
        public void Read_Correct_FB2_File()
        {
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            Assert.AreEqual("Государь", file.BookTitle, "Book title must be 'Государь'");

            Assert.AreEqual("Николо", file.BookAuthorFirstName, "Author first name must be 'Николо'");
            Assert.AreEqual(string.Empty, file.BookAuthorMiddleName, "Author middle name must be empty string");
            Assert.AreEqual("Макиавелли", file.BookAuthorLastName, "Author last name must be 'Макиавелли'");

            Assert.AreEqual(string.Empty, file.BookSequenceName, "Sequence Name must be empty string");
            Assert.IsNull(file.BookSequenceNr, "Sequence Number must be null");

            Assert.AreEqual("Европейская старинная литература", file.BookGenre, "Genre must be 'Европейская старинная литература'");

            Assert.AreEqual("1.1", file.BookVersion, "Book version must be '1.1'");

            Assert.AreEqual("ru", file.BookLang, "Book language must be 'ru'");

            Encoding win1251 = Encoding.GetEncoding(1251);
            Assert.AreEqual(win1251.EncodingName, file.BookEncoding, string.Format("Book encoding must be '{0}'", win1251.EncodingName));

            Assert.AreEqual("352 Кб", file.BookSizeText, "Book size text must be '352 Кб'");
        }

        [TestMethod]
        [TestCategory("FB2.ZIP File")]
        [Description("Reading correct .FB2.ZIP file")]
        public void Read_Correct_FB2_ZIP_File()
        {
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2.zip");
            Assert.AreEqual("Государь", file.BookTitle, "Book title must be 'Государь'");

            Assert.AreEqual("Николо", file.BookAuthorFirstName, "Author first name must be 'Николо'");
            Assert.AreEqual(string.Empty, file.BookAuthorMiddleName, "Author middle name must be empty string");
            Assert.AreEqual("Макиавелли", file.BookAuthorLastName, "Author last name must be 'Макиавелли'");

            Assert.AreEqual(string.Empty, file.BookSequenceName, "Sequence Name must be empty string");
            Assert.IsNull(file.BookSequenceNr, "Sequence Number must be null");

            Assert.AreEqual("Европейская старинная литература", file.BookGenre, "Genre must be 'Европейская старинная литература'");

            Assert.AreEqual("1.1", file.BookVersion, "Book version must be '1.1'");

            Assert.AreEqual("ru", file.BookLang, "Book language must be 'ru'");

            Encoding win1251 = Encoding.GetEncoding(1251);
            Assert.AreEqual(win1251.EncodingName, file.BookEncoding, string.Format("Book encoding must be '{0}'", win1251.EncodingName));

            Assert.AreEqual("210 Кб", file.BookSizeText, "Book size text must be '210 Кб'");
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Renaming .FB2 file to the same name")]
        public void Rename_To_The_Same_Name()
        {
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            RenameProfileElement profile = null;
            foreach (RenameProfileElement rp in FB2Config.Current.RenameProfiles)
            {
                if (rp.Name == "Автор/Серия/Автор - Серия Номер - Название")
                {
                    profile = rp;
                    break;
                }
            }

            Assert.IsNotNull(profile, "Rename profile should not be null");

            if (profile != null)
            {
                var result = file.RenameTo(profile, false);
                Assert.AreEqual(true, result.Skipped, "File should not be renamed");
            }
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Validate the .FB2 file schema")]
        public void Validate_Correct_FB2_File()
        {
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            var errors = file.ValidateSchema();
            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        [TestCategory("FB2.ZIP File")]
        [Description("Validate the .FB2.ZIP file schema")]
        public void Validate_Correct_FB2_ZIP_File()
        {
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2.zip");

            var errors = file.ValidateSchema();
            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Count);
        }

    }
}
