using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
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


        public void export() {
            Console.WriteLine("To be Implemented!");
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
