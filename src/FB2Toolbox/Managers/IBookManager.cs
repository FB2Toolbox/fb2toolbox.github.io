using System.Collections.Generic;
using System.Text;
using FB2Toolbox.Entities;
using FB2Toolbox.Utilities;

namespace FB2Toolbox.Managers
{
    public interface IBookManager
    {
        IBook LoadBook(string fileName);
        FileOperationResult CopyTo(IBook book, string targetFolder, RenameProfileElement profile, bool useTranslit, bool deleteOriginal = false);
        void EncodeTo(IBook book, Encoding enc);
        List<string> ValidateSchema(IBook book);
    }
}
