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

        private PK5 gen5Conversions(EggCreator pokemon, int originGeneration) {

            switch (originGeneration)
            {
                case 4:
                    PK4 pk4 = pokemon.exportPK4(this.currentSave.TrainerTID7, this.currentSave.TrainerSID7);
                    return pk4.ConvertToPK5();

                default:
                    throw new Exception("Unsupported origin generation (" + originGeneration + ")");
            }

        }

        private PK6 gen6Conversions(EggCreator pokemon, int originGeneration) {

            switch (originGeneration)
            {

                //case 3:
                //    PK3 pk3 = pokemon.exportPK3(this.currentSave.TrainerTID7, this.currentSave.TrainerSID7);
                //    PK4 pk4 = pk3.ConvertToPK4();
                //    return pk4.ConvertToPK5().ConvertToPK6();

                case 4:
                    PK4 gen4_pk4 = pokemon.exportPK4(this.currentSave.TrainerTID7, this.currentSave.TrainerSID7);
                    PK5 gen4_pk5 = gen4_pk4.ConvertToPK5();
                    return gen4_pk5.ConvertToPK6();
                //case 5:
                //    PK5 gen5_pk5 = pokemon.exportPK5(this.currentSave.TrainerTID7, this.currentSave.TrainerSID7);
                //    return gen5_pk5.ConvertToPK6();
                default:
                    throw new Exception("Unsupported origin generation (" + originGeneration + ")");

            }

        }


        public void addEgg( EggCreator pokemon, int boxIndex) {

            var box = this.currentSave.BoxData;

            // check game version

            byte saveFileGeneration = this.currentSave.Generation;


            if (pokemon.generation != saveFileGeneration)
            {
                // run conversion scripts here for non-native pokemon
                Console.WriteLine("Converting from generation " + pokemon.generation + " to generation " + saveFileGeneration);
                PKM convertedPokemon;
                switch(saveFileGeneration) {
                    case 5:
                        convertedPokemon = gen5Conversions(pokemon, pokemon.generation);
                        break;

                    case 6:
                        convertedPokemon = gen6Conversions(pokemon, pokemon.generation);
                        break;
                    default:
                        throw new Exception("Unsupported save file generation (Generation " + saveFileGeneration + ")");
                }

                box[boxIndex] = convertedPokemon;
            }
            else {
                // Native pokemon generation

                switch (saveFileGeneration) {
                    case 4:
                        box[boxIndex] = pokemon.exportPK4(this.currentSave.TrainerTID7, this.currentSave.TrainerSID7);
                        break;
                    default:
                        throw new Exception("Unsupported save file generation (Generation " + saveFileGeneration + ")");


                }

            }


            this.currentSave.BoxData = box;



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
                throw new Exception("Save file is null- possibly corrupted?");
            }
            return this.currentSave.OT;
        }

        public IList<PKM> getBox() {
            // return the box data
            if (currentSave == null)
            {
                throw new Exception("Save File is null- possibly corrupted?");
            }
            return this.currentSave.BoxData;
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
