using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace X10Card.Droid
{
    public class PdfViewer : IPdfViewer
    {
        public bool SaveAndOpenPDF(string fileName, byte[] data)
        {
            //Context context = MainActivity.Instance;
            Context context= Android.App.Application.Context;
            try
            {
                string filepath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                string fullName = System.IO.Path.Combine(filepath, fileName);

                // If the file exist on device delete it  
                if (File.Exists(fullName))
                {
                    File.Delete(fullName);
                }

                File.WriteAllBytes(fullName, data);

                Android.Net.Uri file = FileProvider.GetUriForFile(context, context.PackageName + ".fileprovider", new Java.IO.File(fileName));
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(file, "application/pdf");
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask);
                context.StartActivity(intent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message + ex.StackTrace);
                return false;
            }
        }
    }
}