using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker.Abstractions;

namespace PriceCalculator.Helper
{
    public interface IFileHelper
    {
        string GetFile(string fileName);
        bool DeleteFile(string fileName);
        bool RenameFile(string oldFilename, string newFileName);
        Task<bool> SaveZipToFolder();
        Task<bool> UnzipDb(byte[] zipFile);
        //Task AskPermission();
    }
}
