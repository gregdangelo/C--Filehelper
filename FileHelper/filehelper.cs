using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    /*
     FileHelper class
     * currently deals with finding files only
     * future: read,write espcially for caching plus minification
     */
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
        /*
         * Future OPEN file function
         * @return bool false
        */
        public bool open()
        {
            return false;
        }
        /*
         * Future READ file function
         * @return bool false
        */
        public bool read()
        {
            return false;
        }
        /*
         * Future WRITE file function
         * @return bool false
        */
        public bool write()
        {
            return false;
        }
        /*
         * setPath
         * @param string set the initial Path to work from 
         * @return string current path
        */
        public string setPath(string path){
            if(path.Length > 0){
                if (this._validatepath(path))
                {
                    this.pathname = path;
                }
                else
                {
                    //If the path isn't valid let's handle that
                    throw new System.ArgumentException("Path set is not a valid path");
                }
            }else{
                //We need something passed
                throw new System.ArgumentException("Please specify a path","path");
            }
            return this.pathname;
        }
        /*
         * @return string current path
        */
        public string getPath()
        {
            return this.pathname;
        }
        /*
         * @param string path to validate
         * @return bool valid or not
        */
        public bool _validatepath(string path)
        {
            bool result = false;//Start of invalid
            //Simple length test
            if (path.Length > 0)
            {
                //Quick check if what we have is valid... TODO: also need to check for permissions
                DirectoryInfo dirTest = new DirectoryInfo(path);
                if (dirTest.Exists)
                {
                    result = true;
                }
            }
            return result;
        }
        
        /*
         * @param string extension to search for ex "*.mp3" current state only accepts one
         * @param bool true if we want to search subdirectories, is set to false by default
         * @return string[] array of filenames
        */
        public string[] findfiles(string[] extensions,bool verbose = false)
        {
            if (extensions == null)//Check for null
            {
                throw new System.ArgumentNullException("Please specify an array of extensions", "extensions");
            }
            else if (extensions.Length == 0)
            {
                throw new System.ArgumentException("Please specify an array of extensions", "extensions");
            }
            string[] result = {};//default restult
            int fcount = 0;//counter
            
            DirectoryInfo dir = new DirectoryInfo(this.pathname);
            FileInfo[] tmpfiles = dir.EnumerateFiles("*", verbose == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();

            //Let's make sure we have something to work with before do anything
            if (tmpfiles.Length > 0)
            {
                string[] files = new string[tmpfiles.Length];
                foreach (FileInfo file in tmpfiles)
                {
                    //Grab the full filename
                    string tmpname = file.FullName;
                    //remove the original path and extra \'s
                    tmpname = tmpname.Replace(this.pathname + "\\", "");
                    tmpname = tmpname.Replace("\\", ",");
                    string[] tmpfilearray = tmpname.Split(',');
                    //Grab the last item off of the array and save it in our array
                    Stack<string> myStack = new Stack<string>(tmpfilearray);
                    //Currently we're just returning the actual file but I will want to possible return the path as well
                    files[fcount++] = myStack.Pop();
                }
                if (files.Length > 0)
                {
                    result = files;
                }
            }
            return result;
        }
    }
/*
 *Test Class 
 * Just runs through a single test at the moment until this rest of the class is developed
 */
    class TestFileHelper
    {
        static void Main()
        {
            //Run through once to make sure the basics work
            FileHelper fh = new FileHelper();
            fh.setPath("D:\\Music");
            Console.WriteLine(fh.getPath());
            //Search sub directories
            //string[] extensions = new[] { ".jpg", ".tiff", ".bmp" };
            string[] extensions = new[] { ".mp3" };
            string[] files = fh.findfiles(extensions,true);
            Console.WriteLine("Number of Files: "+files.Length);
            Console.WriteLine("File Names:");
            foreach (string fname in files)
            {
                Console.WriteLine(fname);
            }

            /*******Now lets try and break this***********/
            //Try an invalid Path
            try
            {
                fh.setPath("Q:\\Music");//Q drive does not exist
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Crisis averted:" + e.Message);
            }
            //Try an empty string
            try
            {
                fh.setPath(""); //throwing over an empty path
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Crisis averted:" + e.Message);
            }
            //Try an empty extension
            try
            {
                fh.setPath("D:\\Music"); //use a normal path again
                files = fh.findfiles(null, true); //null the extensions
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Crisis averted:" + e.Message);
            }
        }
    }
