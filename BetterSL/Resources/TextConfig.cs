using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BetterSL.Resources
{
    public class TextConfig
    {
        public virtual string ConfigName { get; protected set; } = "config";
        public virtual string FileExtension { get; protected set; } = ".txt";
        public virtual List<string> DefaultText { get; protected set; } = new List<string>();
        public virtual List<string> CurrentText { get; protected set; } = new List<string>();
        public virtual string TargetDirectory { get; protected set; } = ".\\";

        public TextConfig(string path, string name)
        {
            TargetDirectory = path;
            ConfigName = name;
            Generate();
        }

        public TextConfig(string path, string name, string fileExtension)
        {
            TargetDirectory = path;
            ConfigName = name;
            FileExtension = fileExtension;
            Generate();
        }

        public TextConfig(string path, string name, List<string> defaultText, string fileExtension = ".txt")
        {
            TargetDirectory = path;
            ConfigName = name;
            FileExtension = fileExtension;
            DefaultText = defaultText;
            Generate();
        }

        public virtual string GetPath()
        {
            string path = Path.Combine(TargetDirectory, ConfigName) + FileExtension;
            return path;
        }

        public virtual void Read()
        {
            if (!Exists())
            {
                Generate();
            }
            CurrentText = File.ReadAllLines(GetPath()).ToList();
        }

        public virtual void Generate()
        {
            if (!Exists())
            {
                File.WriteAllLines(GetPath(), DefaultText.ToArray());
            }
            Read();
        }

        public virtual bool Exists()
        {
            return File.Exists(GetPath());
        }

        public virtual bool Delete()
        {
            if (!Exists())
            {
                return false;
            }
            else
            {
                File.Delete(GetPath());
                return true;
            }
        }
    }
}
