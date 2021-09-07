﻿/* TILE WORLD CREATOR
 * Copyright (c) 2015 doorfortyfour OG
 * 
 * TileWorldSaveLoad.cs
 * 
 * handels the save and load functionality of a map
 * 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

#if NET_STANDARD_4_6
using TileWorld.OdinSerializer;
#else
using TileWorld.FullSerializer;
#endif

namespace TileWorld
{
    public class TileWorldCreatorSaveLoad : MonoBehaviour
    {

        static string saveMapContent = "";

        public static bool isSaving = false;
        public static bool isLoading = false;

        [System.Serializable]
        public class SaveMap
        {
            public bool[] map = new bool[0];
            public bool[] maskMap = new bool[0];

            public int width = 0;
            public int height = 0;

            public bool useMask = false;
	        public int selectedMask = 0;
            
	        public int preset = 0;

            public SaveMap() { }

	        public SaveMap(bool[] _map, bool[] _maskMap, int _w, int _h, int _preset, bool _useMask, int _selectedMask)
	        {
		        map = new bool[_map.Length];
		        map = _map;

		        maskMap = new bool[_maskMap.Length];
		        maskMap = _maskMap;

		        width = _w;
		        height = _h;
		        
		        preset = _preset;

		        useMask = _useMask;
		        selectedMask = _selectedMask;
	        }
        }
        

        //Save and Load Methods
	    //--------

       
	    public static void Save(string _path, TileWorldCreator _creator)
        {
            saveMapContent = "";
          
            if (_path == "")
                return;


            for (int i = 0; i < _creator.configuration.worldMap.Count; i++)
            {
                bool[] _tmpMapSingle = new bool[_creator.configuration.worldMap[i].cellMapSingle.Length];
                bool[] _tmpMaskMapSingle = new bool[_creator.configuration.worldMap[i].maskMapSingle.Length];

                _tmpMapSingle = _creator.configuration.worldMap[i].cellMapSingle;
                _tmpMaskMapSingle = _creator.configuration.worldMap[i].maskMapSingle;

	            SaveMap _map = new SaveMap(_tmpMapSingle, _tmpMaskMapSingle, _creator.configuration.global.width, _creator.configuration.global.height, _creator.configuration.global.layerPresetIndex[i], _creator.configuration.worldMap[i].useMask, _creator.configuration.worldMap[i].selectedMask);
                

                AddData<SaveMap>(_map);
            }

        	#if NET_STANDARD_4_6
	        byte[] bytes = SerializationUtility.SerializeValue(saveMapContent, DataFormat.Binary);
	        File.WriteAllBytes(_path, bytes);
	        #else
	        fsData data;
	        fsSerializer _serializer = new fsSerializer(); 
	        _serializer.TrySerialize(typeof(string), saveMapContent, out data).AssertSuccessWithoutWarnings();
		    File.WriteAllText(_path, fsJsonPrinter.CompressedJson(data));
			#endif
        }

        static void AddData<T>(T _data)
        {
            StringWriter _sw = new StringWriter();

            XmlSerializer _serializer = new XmlSerializer(typeof(T));
            _serializer.Serialize(_sw, _data);

            //add simple tag between each xml map
            saveMapContent += _sw + "---"; // + System.Environment.NewLine;
            _sw.Close();
        }



        public static void Load(string _path, TileWorldCreator _creator)
        {
 
            if (_path == "")
                return;

            isLoading = true;

			#if NET_STANDARD_4_6
	        byte[] bytes = File.ReadAllBytes(_path);
	        saveMapContent = SerializationUtility.DeserializeValue<string>(bytes, DataFormat.Binary);
			#else
	        var _txt = File.ReadAllText(_path);
	        fsSerializer _serializer = new fsSerializer();
	        fsData data = fsJsonParser.Parse(_txt);
	        object deserialized = null;
	        _serializer.TryDeserialize(data, typeof(string), ref deserialized);
	        saveMapContent = deserialized as string;
			#endif

            //split file in several xml maps
            string[] _sp = new string[] { "---" };
            string[] lines = saveMapContent.Split(_sp, System.StringSplitOptions.None); //("---"[0]);

            _creator.configuration.worldMap = new List<TileWorldConfiguration.WorldMap>();

            _creator.configuration.global.layerCount = lines.Length - 1;

            //clear all settings except presets
            _creator.ClearSettings(false, lines.Length - 1);

            //load back map for each layer
            for (int l = 0; l < lines.Length - 1; l++)
            {

                SaveMap _map = GetData<SaveMap>(lines[l]);

                _creator.configuration.global.width = _map.width;
                _creator.configuration.global.height = _map.height;
	            _creator.configuration.global.layerPresetIndex[l] = _map.preset;


                _creator.configuration.worldMap.Add(new TileWorldConfiguration.WorldMap(_creator.configuration.global.width, _creator.configuration.global.height, false, _map.useMask, _map.selectedMask));
               

                //Load back multidimensional array from single dim array
                _creator.configuration.worldMap[l].cellMap = new bool[_creator.configuration.global.width, _creator.configuration.global.height];
                _creator.configuration.worldMap[l].tileObjects = new GameObject[_creator.configuration.global.width, _creator.configuration.global.height];
                _creator.configuration.worldMap[l].tileTypes = new TileWorldConfiguration.TileInformation.TileTypes[_creator.configuration.global.width, _creator.configuration.global.height];
                _creator.configuration.worldMap[l].maskMap = new bool[_creator.configuration.global.width, _creator.configuration.global.height];
                _creator.configuration.worldMap[l].oldCellMap = new bool[_creator.configuration.global.width, _creator.configuration.global.height];


                int _index = 0;

                

                for (int y = 0; y < _creator.configuration.global.height; y++)
                {
                    for (int x = 0; x < _creator.configuration.global.width; x++)
                    {
                        _creator.configuration.worldMap[l].cellMap[x, y] = _map.map[_index]; // creator.worldMap[l].mapSingle[_index];	

                        if (_map.maskMap.Length > 0)
                        {   
                            _creator.configuration.worldMap[l].maskMap[x, y] = _map.maskMap[_index]; 
                        }
                        
                        
                        _index++;
                    }
                }

                _creator.ResizeIntArray(false, 0);


            }

            //_sr.Close();

            isLoading = false;
        }


        static T GetData<T>(string _data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader _sr = new StringReader(_data);
          
            return (T)serializer.Deserialize(_sr);
        }

    }
}
