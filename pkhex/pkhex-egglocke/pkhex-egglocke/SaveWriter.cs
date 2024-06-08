using System;
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



        public void addEgg( EggCreator pokemon, int boxIndex) {

            // hacky start - hardcoded to box 1

            var mew = new PK4();

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

           mew.HeldItem = 
            


            mew.Ability = pokemon.Ability;
            mew.Nature = pokemon.Nature;

            // Moveset
            mew.Move1 = (int)Move.ShadowSneak;
            mew.Move1_PP = 30;
            mew.Move2 = (int)Move.Memento;
            mew.Move2_PP = 20;


            mew.IVs = pokemon.IV;

            var box = this.currentSave.BoxData;

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
