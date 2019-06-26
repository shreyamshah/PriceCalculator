using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using PCLStorage;
using Plugin.CurrentActivity;
using Plugin.FilePicker.Abstractions;
using PriceCalculator.Droid.Helpers;
using PriceCalculator.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace PriceCalculator.Droid.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetFile(string fileName)
        {
            string fileContent = null;
            using (StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open(fileName)))
            {
                fileContent = sr.ReadToEnd();
            }
            return fileContent;
        }

        public bool DeleteFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RenameFile(string oldFilename, string newFileName)
        {
            try
            {
                File.Move(newFileName, oldFilename);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SaveZipToFolder()
        {
            try
            {
                var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "priceCalculator.sqlite");
                if (File.Exists(path))
                {
                    if (App.Connection != null)
                        await App.Connection.CloseAsync();
                    FileStream fsOut = File.Create(Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path, "price_backup.zip"));
                    ZipOutputStream zipStream = new ZipOutputStream(fsOut);
                    zipStream.SetLevel(6);
                    zipStream.Password = "backup";
                    FileInfo db = new FileInfo(path);
                    ZipEntry dbEntry = new ZipEntry(ZipEntry.CleanName(path.Split("/").LastOrDefault()));
                    zipStream.PutNextEntry(dbEntry);
                    byte[] dbBuffer = new byte[4096];
                    using (FileStream streamReader = File.OpenRead(path))
                    {
                        StreamUtils.Copy(streamReader, zipStream, dbBuffer);
                    }
                    zipStream.CloseEntry();
                    string imgDir = Path.Combine(CrossCurrentActivity.Current.AppContext.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).Path);
                    if (Directory.Exists(imgDir))
                    {
                        string[] files = Directory.GetFiles(imgDir);
                        foreach (var item in files)
                        {
                            FileInfo info = new FileInfo(item);
                            ZipEntry entry = new ZipEntry(ZipEntry.CleanName(item.Split("/").LastOrDefault()));
                            zipStream.PutNextEntry(entry);
                            byte[] buffer = new byte[4096];
                            using (FileStream streamReader = File.OpenRead(item))
                            {
                                StreamUtils.Copy(streamReader, zipStream, buffer);
                            }
                            zipStream.CloseEntry();
                        }
                    }
                    zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
                    zipStream.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UnzipDb(byte[] zipFile)
        {

            //Java.IO.File file = new Java.IO.File(zipFile.FilePath);
            //Android.Net.Uri zip = Android.Net.Uri.Parse(Android.Net.Uri.Decode(zipFile.FilePath));
            //if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Android.Manifest.Permission.ManageDocuments) == (int)Permission.Granted)
            //{
            byte[] data = zipFile;
            //resolvedPath = Android.App.Application.Context.ApplicationContext.ContentResolver.OpenOutputStream(Android.Net.Uri.Parse(zipFile.FilePath));
            Stream fileStream = new MemoryStream(data);
            ZipFile zf = new ZipFile(fileStream);
                zf.Password = "backup";
                if (!zf.GetEntry("priceCalculator.sqlite").Name.Equals("priceCalculator.sqlite"))
                    return false;
                if (App.Connection != null)
                    await App.Connection.CloseAsync();
                List<string> files = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)).ToList();
                List<string> pics = Directory.GetFiles(CrossCurrentActivity.Current.AppContext.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).Path).ToList();
                files.AddRange(pics);
                foreach (var item in files)
                {
                    File.Delete(item);
                }
                foreach (ZipEntry item in zf)
                {
                    string fileName = item.Name;
                    byte[] buffer = new byte[4096];
                    Stream stream = zf.GetInputStream(item);
                    string fullZipToPath = null;
                    if (item.Name.Contains(".sqlite"))
                    {
                        fullZipToPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
                    }
                    else if (item.Name.Contains(".jpg"))
                    {
                        fullZipToPath = Path.Combine(CrossCurrentActivity.Current.AppContext.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).Path, fileName);
                    }
                    else
                        return false;
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(stream, streamWriter, buffer);
                    }
                }
                return true;
            //}
            //else
           // {
            //}
            //await Task.Delay(5000);
            //return false;
        }
    }
}