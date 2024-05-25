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
            // Import the save file
            var sav = SaveUtil.GetVariantSAV(savePath);

        }


        public void export() {
            Console.WriteLine("To be Implemented!");
        }


        // Getters and Setters

        public string getOTString() {
            return "Not Implemented yet";
        }



    }
}
