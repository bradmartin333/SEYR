﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SEYR.Session
{
    internal class DataStream
    {
        public string Path { get; set; } = null;
        internal static string Header = null;

        public DataStream(string path, string header = "", bool isDebug = false)
        {
            if (File.Exists(path)) File.Delete(path);
            Path = path;
            if (isDebug)
                Write("Stream Opened", addDT: true);
            else
            {
                Header = $"{header}TileRow\tTileCol\tFeature\tScore";
                Write(Header);
            } 
        }

        public async Task Write(string value, bool addNewLine = true, bool addDT = false, bool showInViewer = false)
        {
            using (StreamWriter file = new StreamWriter(Path, append: true))
                await file.WriteAsync($"{(addDT ? $"{System.DateTime.Now}\t" : "")}{value}{(addNewLine ? "\n" : "")}");
            if (showInViewer)
            {
                try { Channel.Viewer.Invoke((System.Windows.Forms.MethodInvoker)delegate { 
                    Channel.Viewer.InfoLabel.Text = $"{System.DateTime.Now}   {value.Replace("\t", "   ")}"; }); }
                catch (System.Exception) { }
            }
        }
    }
}
