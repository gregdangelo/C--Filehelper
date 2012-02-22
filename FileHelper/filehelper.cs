using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public static class DirectoryHelper{
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException("extensions");
            }
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }
    }

    public class FileHelper
    {
        private string pathname;
        //Basic no arguments constructor
        public FileHelper()
        {
            pathname = "/";
        }
        //Basic one argument constructor
        public FileHelper(string path)
        {
            this.setPath(path);
        }
        public string setPath(string path){
            if(path.Length > 0){
                if (this._validatepath(path))
                {
                    this.pathname = path;
                }
                else
                {
                    throw new System.InvalidOperationException("Path set is not a valid path");
                }
            }else{
                throw new System.ArgumentException("Please specify a path","path");
            }
            return this.pathname;
        }
        public string getPath()
        {
            return this.pathname;
        }
        public bool _validatepath(string path)
        {
            bool result = false;
            if (path.Length > 0)
            {
                DirectoryInfo dirTest = new DirectoryInfo(path);
                
                if (dirTest.Exists)
                {
                    result = true;
                }
            }
            return result;
        }
        

        
        public string[] findfiles(string extension)
        {
            string[] result = {};
            int fcount = 0;
            
            DirectoryInfo dir = new DirectoryInfo(this.pathname);
            FileInfo[] tmpfiles = dir.GetFiles(extension, SearchOption.AllDirectories);
            //Need to update the Extension above to allow this code to work as intended
            //FileInfo[] tmpfiles = dir.GetFilesByExtensions(".jpg", ".png", ".gif");
            Console.WriteLine(tmpfiles.Length);
            string[] files = new string[tmpfiles.Length];
            foreach (FileInfo file in tmpfiles)
            {
                //Grab the full filename
                string tmpname = file.FullName;
                //remove the original path and extra \'s
                tmpname = tmpname.Replace(this.pathname+"\\", "");
                tmpname = tmpname.Replace("\\", ",");
                string[] tmpfilearray = tmpname.Split(',');
                //Grab the last item off of the array and save it in our array
                Stack<string> myStack = new Stack<string>(tmpfilearray);
                files[fcount++] = myStack.Pop();
            }
            if (files.Length > 0)
            {
                result = files;
            }
            return result;
        }
    }
    class TestFileHelper
    {
        static void Main()
        {
            FileHelper fh = new FileHelper();
            fh.setPath("D:\\Music");
            Console.WriteLine(fh.getPath());
            string[] files = fh.findfiles("*.mp3");
            foreach (string fname in files)
            {
                   Console.WriteLine(fname);
            }
        }
    }
