using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;

namespace Core.Utilities.Helpers
{
    public class FileHelper
    {
        static string directory = Directory.GetCurrentDirectory() + @"\wwwroot\";
        static string path = @"Images/";



        public static string UploadImageFile(IFormFile imageFile)
        {
            string fileFormat = Path.GetExtension(imageFile.FileName).ToLower();
            string fileName = Guid.NewGuid().ToString();
            string imagePath = Path.Combine(directory + path, fileName + fileFormat);
            if (!Directory.Exists(directory + path))
            {
                Directory.CreateDirectory(directory + path);
            }
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
                fileStream.Flush();
            }
            return path.Replace(@"\", "/") + fileName + fileFormat;
            //return  (fileName + fileFormat);

        }
        public static void UpdateImageFile(IFormFile imageFile, string imagePath)
        {
            var path = Path.Combine(@"wwwroot\Images", imagePath);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
                fileStream.Flush();
            }
        }
        public static void DeleteImageFile(string fileName)
        {
            var path = Path.Combine(@"wwwroot\Images", fileName);
            if (File.Exists(path)) 
                File.Delete(path);

        }
    }
}
