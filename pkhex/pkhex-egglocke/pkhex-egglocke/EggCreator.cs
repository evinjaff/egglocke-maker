/* PokemonCreator.cs
*  Bridge class that creates PkHEX pokemon files based on JSON objects exported from web app.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKHeX.Core;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace pkhexEgglocke
{
    /// <summary>
    /// PokemonCreator: Pokemon data class that decodes JSON pokemon descriptors and 
    /// </summary>
    /// 

    // Parameters
    // These parameters mostly allow you to set rules around what user submissions you'd like to include (ex. allow user-supplied IVs)
    

    internal class EggCreator
    {

        protected bool allowUserIVs => true;
        protected bool allowUserShiny => true;
        protected bool allowUser => true;

        // Egg Parameters that are always true
        public bool IsEgg = true;
        public ushort EggLocationDP = Locations.Daycare4;
        public byte MetLevel = 0;

        //Other constant fields
        public int language = (int)LanguageID.English;


        // Mutable fields
        public byte ball = (int)Ball.Dive;

        public ushort dexNumber = 1;
        public int Language = 1;
        public int Ability = 2;
        public Nature Nature = Nature.Adamant;



        //Misc creator stuff
        public string OT = "Default";
        public byte OTGender = (int)Gender.Female;
        public string nickname = "Hello";

        public int[] IV = [31, 31, 31, 31, 31, 31];
        public int[] EV = [0, 0, 0, 0, 0, 0];

        public ushort [] moves = {425, 262};
        public ushort[] movespp = { 30, 40 };


        public EggCreator(byte ball, ushort dexNumber, int language, int ability, Nature Nature, string OT, byte OTGender, string nickname, int[] IV, int[] EV, ushort[]  moves, ushort[] movespp) { 
            
            this.ball = ball;
            this.dexNumber = dexNumber;
            this.language = language;
            this.Ability = ability;
            this.Nature = Nature;
            this.OT = OT;
            this.OTGender = OTGender;
            this.nickname = nickname;
            this.IV = IV;
            this.EV = EV;
            this.moves = moves;
            this.movespp = movespp;


        }


        public static EggCreator decodeJSON(string filepath) {
            string json = File.ReadAllText(filepath);

            dynamic newObject = JsonConvert.DeserializeObject(json);

            Nature nature = lookupNatureInt((int) newObject.nature.Value);

            int[] IV = newObject.IV.ToObject<int[]>();
            int[] EV = newObject.EV.ToObject<int[]>();

            int[] moves = newObject.moves.ToObject<int[]>();
            int[] movespp = newObject.movespp.ToObject<int[]>();

            return new EggCreator(
                newObject.ball,
                newObject.dexNumber,
                newObject.language,
                newObject.ability,
                nature,
                newObject.OT,
                newObject.OTGender,
                newObject.nickname,
                IV,
                EV,
                convertIntArrayViaCast(moves),
                convertIntArrayViaCast(movespp)

                );
           
        }

        public static Nature lookupNatureInt(int nature)
        {
            if (nature == 1) {
                return Nature.Adamant;
            }

            return Nature.Bashful;
        }

        public static ushort[] convertIntArrayViaCast(int[] array)
        {

            ushort[] result = new ushort[array.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (ushort) array[i];
            }

            return result;
        }


    }
}
