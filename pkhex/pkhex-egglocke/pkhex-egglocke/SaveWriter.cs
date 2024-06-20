﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PKHeX.Core;

[assembly: InternalsVisibleTo("pkhexEgglockeTests")]
namespace pkhexEgglocke
{
    /// <summary>
    /// SaveWriter: a StringBuilder-style class which imports a pkhex save from a file and makes modifications
    /// </summary>
   
    internal class SaveWriter
    {

        // Internal Variables
        protected SaveFile? currentSave;

        
        public SaveWriter() { 
            // as empty constructor, must load a save file or else this class doesn't work
        }


        /// <summary>
        /// String filepath based 
        /// <example>
        /// For example:
        /// <code>
        /// SaveWriter sw = new SaveWriter("C:\Users\ash\backup.sav");
        /// p.addPokemonFromFile("deoxys.pk1", 1, 1);
        /// </code>
        /// results: Adds a pokemon to index 1 in box 1 of the save file
        /// </example>
        /// </summary> 
        public SaveWriter(string savePath) {

            #if DEBUG
                Console.WriteLine("Reading from ");
                Console.WriteLine(savePath);
            #endif


            validateConstructor(savePath);

            this.currentSave = SaveUtil.GetVariantSAV(savePath);

        }

        public void massAddEggs(string JSONPath) { 
        
            string json = File.ReadAllText(JSONPath);

            dynamic newOject = JsonConvert.DeserializeObject(json);


            for (int i=0; i < newOject.Count; i++) {

                string jsonEgg = newOject[i].ToString();

                EggCreator egg = EggCreator.decodeJSON(jsonEgg, false);
                addEgg(egg, i);

            }

        
        }

        internal void MakeShiny(PK4 pokemon) { 
            // Get IDs
            ushort trainer_id = pokemon.TID16;
            ushort secret_id = pokemon.SID16;
            uint pid = ShinyUtil.GetShinyPID(trainer_id, secret_id, 0, 0);

            pokemon.PID = pid;  
            
           
        }

        internal static bool IsShinyPID(uint pid, uint trainer_id, uint secret_id) { 
            return ((trainer_id ^ secret_id ^ (pid & 0xFFFF) ^ (pid >> 16)) < 8);
        }


        internal uint GenerateShinyPID(uint trainer_id, uint secret_id) { 
            Random rand = new Random();
            uint pid;
            do
            {
                pid = (uint)rand.Next(0, 0x1000000);
            } while (!IsShinyPID(pid, trainer_id, secret_id));

            return pid;

        }

        private static PK4 createDefaultEgg(uint tid, uint sid) {
            
            return new PK4
            {
                TrainerTID7 = tid,
                TrainerSID7 = sid,
                IsEgg = true,
                MetLevel = 1,
                Ball = 4,
                Species = 1,
                Nickname = "Egg",
                Language = 1,
                OriginalTrainerName = "PKHeX",
                OriginalTrainerGender = 0,
                HeldItem = 0,
                Ability = 0,
                Nature = 0,
                Move1 = 0,
                Move1_PP = 0,
                Move2 = 0,
                Move2_PP = 0,
                Move3 = 0,
                Move3_PP = 0,
                Move4 = 0,
                Move4_PP = 0,
                IVs = new int[] { 31, 31, 31, 31, 31, 31 }
            };
        
        
        }

        public void addEgg( EggCreator pokemon, int boxIndex) {

            // hacky start - hardcoded to gen 4 pokemon
            var mew = createDefaultEgg(this.currentSave.TrainerTID7, this.currentSave.TrainerSID7);

            // Set the trainer ID according to the save file
            mew.TrainerTID7 = this.currentSave.TrainerTID7;
            mew.TrainerSID7 = this.currentSave.TrainerSID7;

            // Standard Egg attributes
            mew.IsEgg = pokemon.IsEgg;
            mew.EggLocationDP = pokemon.EggLocationDP;
            mew.MetLevel = pokemon.MetLevel;
            mew.Ball = pokemon.ball;


            // Pokemon Info
            mew.Species = pokemon.dexNumber;
            mew.Nickname = pokemon.nickname;
            mew.Language = pokemon.language;
            mew.OriginalTrainerName = pokemon.OT;
            mew.OriginalTrainerGender = pokemon.OTGender;

            mew.HeldItem = pokemon.heldItem;
            


            mew.Ability = pokemon.Ability;
            mew.Nature = pokemon.Nature;

            // Moveset
            mew.Move1 = (int)Move.ShadowSneak;
            mew.Move1_PP = 30;
            mew.Move2 = (int)Move.Memento;
            mew.Move2_PP = 20;


            mew.IVs = pokemon.IV;

            var box = this.currentSave.BoxData;

            // Set shiny
            // Shiny stuff
            if (pokemon.isShiny)
            {
                Console.WriteLine("Making Shiny");
                MakeShiny(mew);
            }


            // Check legality of the pokemon

            LegalityAnalysis legalitychecker = new LegalityAnalysis(mew);

            #if DEBUG
            if (legalitychecker.Valid)
            {
                Console.WriteLine("Legal Egg created!");
            }
            else {
                Console.WriteLine("Illegal Egg Created!");
            }
            #endif

            box[boxIndex] = mew;


            // update box
            this.currentSave.BoxData = box;

            // this.currentSave.AddBoxData( , 1, 1 );

            
        }


        public void export(string location) {
            
            byte[] modifiedSaveData = this.currentSave.Write();
        
            // dump to location
            File.WriteAllBytes(location, modifiedSaveData);

        }

        public byte[] exportRawBytes() { 
            return this.currentSave.Write();
        }


        // Getters and Setters

        /// <summary>
        /// Return the OT String of the save file
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public string getOTString() {
            if (currentSave == null)
            {
                throw new Exception("OT field in save file is null- possibly corrupted?");
            }
            return this.currentSave.OT;
        }

        // Other utility functions

        public void validateConstructor(string filePath) {

            if (!isFilePathValid(filePath))
            {
                throw new Exception("Invalid save file path");
            }


        }

        public bool isFilePathValid(string filePath)
        {
            return (File.Exists(filePath));
        }



    }
}
