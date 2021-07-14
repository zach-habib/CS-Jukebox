﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Jukebox
{
    //Contains references to SongProfiles
    public class MusicKit
    {
        public string Name;

        public SongProfile freezeSong { get; set; }
        public SongProfile startSong { get; set; }
        public SongProfile bombSong { get; set; }
        public SongProfile winSong { get; set; }
        public SongProfile loseSong { get; set; }
        public SongProfile MVPSong { get; set; }

        public MusicKit(string name)
        {
            Name = name;

            freezeSong = new SongProfile();
            startSong = new SongProfile();
            bombSong = new SongProfile();
            winSong = new SongProfile();
            loseSong = new SongProfile();
            MVPSong = new SongProfile();
        }
    }
}
