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
using System.Security.Cryptography;

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
        // TODO: refactor to get/set

        protected bool allowUserIVs { get; set; } = true;
        protected bool allowUserShiny { get; set; } = true;
        protected bool allowUser { get; set; } = true;

        public int generation { get; set; } = 4;


        // Egg Parameters that are always true
        public bool IsEgg { get; set; } = true;
        public ushort EggLocationDP { get; set; } = Locations.Daycare4;
        public byte MetLevel { get; set; } = 0;

        //Other constant fields
        public int language { get; set; } = (int)LanguageID.English;


        // Mutable fields
        public byte ball { get; set; } = (int)Ball.Dive;

        public ushort dexNumber { get; set; } = 1;
        public int Language { get; set; } = 1;
        public int Ability { get; set; } = 2;
        public Nature Nature { get; set; } = Nature.Adamant;
        public int heldItem { get; set; } = 0;


        //Misc creator stuff
        public string OT { get; set; } = "Default";
        public byte OTGender { get; set; } = (int)Gender.Female;
        public string nickname { get; set; } = "Hello";

        public int[] IV { get; set; } = new int[] { 31, 31, 31, 31, 31, 31 };
        public int[] EV { get; set; } = new int[] { 0, 0, 0, 0, 0, 0 };

        public ushort[] moves { get; set; } = new ushort[] { 425, 262 };
        public ushort[] movespp { get; set; } = new ushort[] { 30, 40 };

        public bool isShiny { get; set; } = true;


        public EggCreator()
        {


        }

        internal void MakeShiny(PK4 pokemon)
        {
            // Get IDs
            ushort trainer_id = pokemon.TID16;
            ushort secret_id = pokemon.SID16;
            uint pid = ShinyUtil.GetShinyPID(trainer_id, secret_id, pokemon.PID, 0);

            pokemon.PID = pid;


        }

        public PK4 exportPK4(uint trainerID, uint secretID) {

            // create default
            PK4 mew = new PK4
            {
                TrainerTID7 = trainerID,
                TrainerSID7 = secretID,
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

            // Set the trainer ID according to the save file
            mew.TrainerTID7 = trainerID;
            mew.TrainerSID7 = secretID;

            // Standard Egg attributes
            mew.IsEgg = this.IsEgg;
            mew.EggLocationDP = this.EggLocationDP;
            mew.MetLevel = this.MetLevel;
            mew.Ball = this.ball;


            // Pokemon Info
            mew.Species = this.dexNumber;
            mew.Nickname = this.nickname;
            mew.Language = this.language;
            mew.OriginalTrainerName = this.OT;
            mew.OriginalTrainerGender = this.OTGender;

            mew.HeldItem = this.heldItem;
            mew.Version = GameVersion.HG;



            mew.Ability = this.Ability;

            Console.WriteLine(this.moves);
            // Moveset

            // Based on number of moves, set the moves
            for (int i = 0; i < this.moves.Length; i++)
            {
                mew.SetMove(i, this.moves[i]);
            }

            mew.IVs = this.IV;



            // Logic to confirm that the PID matches the nature and shininess
            for (; !((Nature)(mew.PID % 25) == this.Nature) && (this.isShiny == this.isShiny);)
            {
                // Attempt to set the nature
                mew.SetPIDNature(this.Nature);

                if (this.isShiny)
                {
                    MakeShiny(mew);
                }

            }

            Console.WriteLine("Nature after setting");
            Console.WriteLine(mew.Nature);

#if DEBUG
            // Check legality of the pokemon
            LegalityAnalysis legalitychecker = new LegalityAnalysis(mew);

            if (legalitychecker.Valid)
            {
                Console.WriteLine("Legal Egg created!");
            }
            else
            {
                Console.WriteLine("Illegal Egg Created!");
            }
#endif

            return mew;

        }



        public static EggCreator decodeJSON(string filepath, bool is_filepath)
        {
            /// <summary>
            /// decodeJSON: Decodes a JSON object into a PokemonCreator object
            /// </summary>
            /// 

            Console.WriteLine("filepath: ");
            Console.WriteLine(filepath);

            string json;
            if (is_filepath)
            {
                json = File.ReadAllText(filepath);
            }
            else
            {
                json = filepath;
            }


            dynamic newObject = JsonConvert.DeserializeObject(json);

            Console.WriteLine("Successfully deserialized JSON!");


            Console.WriteLine("Nature: ");
            Console.WriteLine((byte)newObject.nature.Value);

            Nature nature = natureEnumGrab((byte)newObject.nature.Value);

            Console.WriteLine(nature);

            int generation = (int)newObject.generation;

            int[] IV = newObject.IV.ToObject<int[]>();
            int[] EV = newObject.EV.ToObject<int[]>();

            int[] moves = newObject.moves.ToObject<int[]>();
            int[] movespp = newObject.movespp.ToObject<int[]>();
            bool isShiny = newObject.isShiny;


            byte ball = (byte)newObject.ball;
            ushort dexNumber = (ushort)newObject.dexNumber;
            int language = (int)newObject.language;
            int ability = (int)newObject.ability;
            int heldItem = (int)newObject.heldItem;

            string OT = newObject.OT;
            byte OTGender = (byte)newObject.OTGender;
            string nickname = newObject.nickname;

            // Print EggCreator args


            return new EggCreator()
            {
                ball = ball,
                dexNumber = dexNumber,
                language = language,
                Ability = ability,
                Nature = nature,
                OT = OT,
                OTGender = OTGender,
                nickname = nickname,
                IV = IV,
                EV = EV,
                moves = convertIntArrayViaCast(moves),
                movespp = convertIntArrayViaCast(movespp),
                heldItem = heldItem,
                isShiny = isShiny,
                generation = generation
            };

        }

        public static Nature natureEnumGrab(byte value)
        {
            if (Enum.IsDefined(typeof(Nature), value))
            {
                return (Nature)value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value is out of range for the Nature enum.");
            }
        }

        public static ushort[] convertIntArrayViaCast(int[] array)
        {
            ushort[] result = new ushort[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (ushort)array[i];
            }

            return result;
        }


    }
}
