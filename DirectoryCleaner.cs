using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace DirCleaner
{
    public class DirectoryCleaner
    {
        private List<string> EmptyDirPathList;
        private int EmptyDirCount;
        private string RootDir;

        public DirectoryCleaner()
        {
            this.EmptyDirPathList = new List<string>();
            this.EmptyDirCount = 0;
            this.RootDir = "";
        }

        public void Analyze(string dirPath)
        {
            this.RootDir = dirPath;
            var dirs = new List<string>();
            var files = new List<string>();

            try
            {
                dirs = Directory.GetDirectories(dirPath).ToList();
                files = Directory.GetFiles(dirPath).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (dirs.Count == 0 && files.Count == 0)
            {
                this.EmptyDirPathList.Add(dirPath);
                this.EmptyDirCount++;
                Console.WriteLine($"\"{dirPath}\" is empty.");
            }
            else
            {
                dirs.ForEach(dir => this.Analyze(dir));
            }
        }

        private void ListOptimization()
        {
            var deletingPlanDirPath = new List<string>();
            var addingPlanDirPath = new List<string>();

            foreach (var dirPath in this.EmptyDirPathList)
            {
                if (IsDeletableParentDirectoryPath(dirPath))
                {
                    var parentDirPath = Directory.GetParent(dirPath).FullName;

                    if(!addingPlanDirPath.Contains(parentDirPath))
                    {
                        addingPlanDirPath.Add(parentDirPath);
                    }

                    deletingPlanDirPath.Add(dirPath);
                }
            }

            deletingPlanDirPath.ForEach(path => this.EmptyDirPathList.Remove(path));
            addingPlanDirPath.ForEach(path => this.EmptyDirPathList.Add(path));

            if (deletingPlanDirPath.Count > 0 && addingPlanDirPath.Count > 0)
            {
                ListOptimization();
            }
        }

        private bool IsDeletableParentDirectoryPath(string dirPath)
        {
            var parentDirPath = Directory.GetParent(dirPath).FullName;
            var childrenDirPath = Directory.GetDirectories(parentDirPath).ToList();
            var childrenFilePath = Directory.GetFiles(parentDirPath).ToList();
            var existChecks = new List<bool>();
            childrenDirPath.ForEach(path => existChecks.Add(this.EmptyDirPathList.Contains(path)));

            return existChecks.All(exist => exist == true);
        }

        public void Clean()
        {
            ListOptimization();

            if (this.EmptyDirPathList.Count == 0)
            {
                Console.WriteLine("No empty directories found.");
                return;
            }

            this.EmptyDirPathList.ForEach(path => Console.WriteLine($"You need to delete the upper directory \"{path}\"."));
            Console.WriteLine($"Found {this.EmptyDirCount} -> {this.EmptyDirPathList.Count} empty directories.");
            Console.Write("Do you want to delete them? (y/n)>");
            var input = Console.ReadLine();

            if (input != "y" && input != "Y" && input != "yes" && input != "Yes")
            {
                Console.WriteLine("Canceled.");
                return;
            }

            foreach (var dirPath in this.EmptyDirPathList)
            {
                try
                {
                    Directory.Delete(dirPath, true);
                    Console.WriteLine($"\"{dirPath}\" is deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}