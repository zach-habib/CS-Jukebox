﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CS_Jukebox
{
    static class Properties
    {
        public static readonly string ConfigPath = @"\csgo\cfg\gamestate_integration_jukebox.cfg";
        public static readonly string ConfigName = @"\gamestate_integration_jukebox.cfg";
        public static readonly string PropertiesFilePath = @"\properties.json";

        public static readonly string MusicKitsPath = @"\kits";

        public static string GameDir = null;
        public static List<MusicKit> MusicKits = null;
        public static MusicKit SelectedKit
        {
            get { return selectedKit; }
            set { SetKit(value); }
        }

        private static MusicKit selectedKit = null;
        private static string SelectedKitName = null;

        //Converts settings to json file then saves it
        public static void SaveProperties()
        {
            string dir = Directory.GetCurrentDirectory() + PropertiesFilePath;

            PropertiesFile propFile = new PropertiesFile();
            propFile.GameDir = GameDir;
            Console.WriteLine(propFile.GameDir);

            string jsonFile = JsonConvert.SerializeObject(propFile);

            Console.WriteLine("Saving json properties: ");
            Console.WriteLine(jsonFile);
            File.WriteAllText(dir, jsonFile);
        }

        //Calls all load methods
        public static void Load()
        {
            LoadProperties();
            LoadKits();
        }

        //Calls all save methods
        public static void Save()
        {
            SaveProperties();
            SaveKits();
        }

        //Reads properties file then deserializes it
        public static void LoadProperties()
        {
            string dir = Directory.GetCurrentDirectory() + PropertiesFilePath;
            PropertiesFile propFile;

            try
            {
                string jsonFile = File.ReadAllText(dir);
                propFile = JsonConvert.DeserializeObject<PropertiesFile>(jsonFile);
                GameDir = propFile.GameDir;
                SelectedKitName = propFile.SelectedKitName;
            }
            catch (FileNotFoundException e)
            {
                propFile = new PropertiesFile();
            }
        }

        //Copies the config from local folder to CS:GO cfg folder
        public static void CreateConfig()
        {
            string configPath = Properties.GameDir + Properties.ConfigPath;
            string root = Directory.GetCurrentDirectory();
            string configSrc = root + Properties.ConfigName;

            if (File.Exists(configPath))
            {
                File.Delete(configPath);
                File.Copy(configSrc, configPath);
            }
            else
            {
                File.Copy(configSrc, configPath);
            }
        }

        public static void SaveKits()
        {
            string dir = Directory.GetCurrentDirectory() + MusicKitsPath;

            Directory.CreateDirectory(dir);

            foreach (MusicKit musicKit in MusicKits)
            {
                string kitDir = dir + @"\" + musicKit.Name + ".json";
                string jsonFile = JsonConvert.SerializeObject(musicKit);
                File.WriteAllText(kitDir, jsonFile);
            }
        }

        public static void LoadKits()
        {
            string dir = Directory.GetCurrentDirectory() + MusicKitsPath;
            MusicKits = new List<MusicKit>();

            if (Directory.Exists(dir))
            {
                foreach (string filePath in Directory.GetFiles(dir))
                {
                    if (!filePath.EndsWith(".json")) continue;
                    string jsonFile = "";

                    try
                    {
                        Console.WriteLine("Attempting to read file: ");
                        Console.WriteLine(filePath);
                        jsonFile = File.ReadAllText(filePath);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception when trying to load music kits.");
                        Console.WriteLine(e.StackTrace);
                    }
                    finally
                    {
                        MusicKit musicKit = JsonConvert.DeserializeObject<MusicKit>(jsonFile);
                        MusicKits.Add(musicKit);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(dir);
            }

            //Find a value for SelectedKit if applicable
            if (MusicKits.Count > 0)
            {
                foreach (MusicKit musicKit in MusicKits)
                {
                    if (musicKit.Name.Equals(SelectedKitName))
                    {
                        SelectedKit = musicKit;
                    }
                }

                if (SelectedKit == null)
                {
                    SelectedKit = MusicKits[0];
                }
            }
        }

        private static void SetKit(MusicKit newKit)
        {
            selectedKit = newKit;
            SelectedKitName = selectedKit.Name;
        }

        //Inner class for properties parameters
        private class PropertiesFile
        {
            public string GameDir;
            public string SelectedKitName;
        }
    }
}
