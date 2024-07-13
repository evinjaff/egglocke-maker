# cachedconstants.py - 
# This file generates important constant JSON files that are used in the pokepoll app. This file is run once to generate 
# the JSON files, and then the JSON files are used in the web app.
# This file generates the following JSON files:
# 1. pokemon_name_to_id.json - a dictionary that maps pokemon names to their respective pokemon ids
# 2. all_pokemon_names.json - a list of all pokemon names
# 3. ability_enum.json - a dictionary that maps ability ids in pkhex to ability names

import json
import re

# Constants

## Filepahs
POKEDEX_FILEPATH = '../static/pokepoll/pokedex.json'
ABILITY_ENUM_PATH = 'ability_enum.cs'
MOVE_ENUM_PATH = 'move_enum.cs'

FILE_1_EXPORT_PATH = '../static/pokepoll/pokemon_name_to_id.json'
FILE_2_EXPORT_PATH = '../static/pokepoll/all_pokemon_names.json'
FILE_3_EXPORT_PATH_FORMAT_STRING = '../static/pokepoll/pokemon_ability_to_id_gen_{}.json'
FILE_4_EXPORT_PATH_FORMAT_STRING = '../static/pokepoll/pokemon_move_to_id_gen_{}.json'

ALL_POKEMON_NAMES = [
]

MAX_POKEDEX_DICT = {
    1: 151,
    2: 251,
    3: 386,
    4: 493,
    5: 649,
    6: 721,
    7: 809,
    8: 898,
}

MAX_ABILITY_DICT = {
    3: 75,
    4: 124,
    5: 164,
    6: 191,
    7: 233,
    8: 267,
    9: 305
}

MAX_MOVE_DICT = {
    1: 166,
    2: 252,
    3: 355,
    4: 468,
    5: 560,
    6: 621
}


POKEMON_NAME_TO_ID = {
}

def import_json(filepath):
    with open(filepath, 'r') as f:
        data = json.load(f)
    return data

def get_all_pokemon_names(pokedex):
    all_pokemon_names = []
    for pokemon in pokedex:
        all_pokemon_names.append(pokemon['name']["english"])
    return all_pokemon_names

def get_pokemon_name_to_id(pokedex):
    pokemon_name_to_id = {}
    for pokemon in pokedex:
        pokemon_name_to_id[pokemon['name']["english"]] = pokemon['id']

    return pokemon_name_to_id


def generate_nature_enum():
    post_gen_3_natures = ["Hardy", "Lonley", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest", "Mild", "Quiet", "Bashful", "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky"]
    gen_3_natures = ["Lonely", "Brave", "Adamant", "Naughty", "Bold", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Jolly", "Naive", "Modest", "Mild", "Quiet", "Rash", "Calm", "Gentle", "Sassy", "Careful"]

    # create a dictionary that maps the nature to the index, using post_gen_3_natures as the key 
    post_gen_3_nature_enum = {}
    gen3_nature_enum = {}

    counter = 0
    for nature in post_gen_3_natures:
        
        if nature in gen_3_natures:
            gen3_nature_enum[nature] = counter

        post_gen_3_nature_enum[nature] = counter


        counter += 1

    # Also add random nature
    post_gen_3_nature_enum["Random"] = counter
    gen3_nature_enum["Random"] = counter

    return post_gen_3_nature_enum, gen3_nature_enum



def decode_c_sharp_enum(ENUM_PATH):
    '''
    
    decode_c_sharp_enum(ENUM_PATH)

    This function, when given the path to a C# enum file, will parse the file and return a dictionary 
    that maps the enum values to their respective integer values. This is useful for using C# enums in
    non-C# applications.

    Now, could I have just manually translated the enum? Yeah. But the programmer's credo is to spend 10 hours
    automating a 5 minute task that you will only do twice.

    Parameters:
    ENUM_PATH (str): the path to the C# enum file

    '''
    
    # open the C# ability enum
    with open(ENUM_PATH, 'r') as f:
        lines = f.readlines()
        # joined_lines = "".join(lines)

        # PASS 1 - Remove Comments
        lines_phase_1 = []
        # we have become the compiler
        for line in lines:
            # strip any syntax after "//"
            line = line.split("//")[0]
            # strip any whitespace
            line = line.strip()
            # if the line is not empty, add it to the list
            if line:
                lines_phase_1.append(line)

        # print(lines_phase_1)

        lines_phase_2 = []
        # PASS 2 - look for an enum
        found_enum = False
        found_inside_enum = False
        for line in lines_phase_1:

            # cut up any whitespace
            broken_up_line = line.split()

            if not found_enum:
            
                if "enum" in broken_up_line:
                    found_enum = True

                    # look for a "{" that starts the enum

            # if we are inside the enum, we need to append track of the lines
            elif found_inside_enum:
                if "}" in broken_up_line:
                    found_inside_enum = False
                    found_enum = False
                    break
                else:
                    lines_phase_2.append(line)
            
            else:
                # now we need to look for the "{" that starts the enum
                if "{" in broken_up_line:
                    # print("Found enum")
                    # print(line)

                    # find the index of "{"
                    index = line.index("{")
                    remainder_of_line = line[index+1:]

                    lines_phase_2.append(remainder_of_line)

                    # mark that we are inside the enum
                    found_inside_enum = True
            
        lines_phase_3 = "".join(lines_phase_2)
        # separate each by commas and form a list
        lines_phase_4 = lines_phase_3.split(",")

        # print(lines_phase_4)

        counter = 0
        results = []
        for line in lines_phase_4:
            # remove leading and trailing whitespace but keep the middle
            line = line.strip()

            # we also exclude MAX_COUNT as this is a special case of using the last element in the enum to denote length
            if line == "MAX_COUNT":
                continue

            # add a space before a capital letter
            line = re.sub(r"([A-Z])", r" \1", line)

            # remove any leading or trailing whitespace
            line = line.strip()


            # ignore empty lines
            if not line:
                continue

            results.append(line)
            counter += 1


        return results


def main():

    # Derive constant JSON files from static/pokepoll/pokemdex.json
    pokedex = import_json(POKEDEX_FILEPATH)

    # file 1: all_pokemon_names.json
    all_pokemon_names = get_all_pokemon_names(pokedex)

    # file 2: pokemon_name_to_id.json

    # file 3: ability lookup of ability_id to ability_name
    ability_lookup = decode_c_sharp_enum(ABILITY_ENUM_PATH)

    # file 4: move lookup of move_id to move_name
    move_lookup = decode_c_sharp_enum(MOVE_ENUM_PATH)



    for terminating_index in MAX_POKEDEX_DICT:
        pokedex_slice = pokedex[:MAX_POKEDEX_DICT[terminating_index]]
        pokemon_name_to_id = get_pokemon_name_to_id(pokedex_slice)

        with open('../static/pokepoll/pokemon_name_to_id_gen_{}.json'.format(terminating_index), 'w') as f:
            json.dump(pokemon_name_to_id, f)

    # with open(FILE_1_EXPORT_PATH, 'w') as f:
    #     json.dump(pokemon_name_to_id, f)

    with open(FILE_2_EXPORT_PATH, 'w') as f:
        json.dump(all_pokemon_names, f)


    # For each result in ability lookup, dump a different JSON
    for terminating_index in MAX_ABILITY_DICT:
        ability_slice = ability_lookup[:MAX_ABILITY_DICT[terminating_index]]
        ability_lookup_export = {}

        counter = 0
        for ability in ability_slice:
            ability_lookup_export[ability] = counter
            counter += 1

        with open(FILE_3_EXPORT_PATH_FORMAT_STRING.format(terminating_index), 'w') as f:
            json.dump(ability_lookup_export, f)

    for terminating_index in MAX_MOVE_DICT:
        move_slice = move_lookup[:MAX_MOVE_DICT[terminating_index]]
        move_lookup_export = {}

        counter = 0
        for move in move_slice:
            move_lookup_export[move] = counter
            counter += 1

        with open(FILE_4_EXPORT_PATH_FORMAT_STRING.format(terminating_index), 'w') as f:
            json.dump(move_lookup_export, f)

    # Also dump the full ability lookup
    master_ability_lookup = {}
    counter = 0
    for ability in ability_lookup:
        master_ability_lookup[ability] = counter
        counter += 1

    with open(FILE_3_EXPORT_PATH_FORMAT_STRING.format("full"), 'w') as f:
        json.dump(master_ability_lookup, f)


    # get nature enums and dump them
    post_gen_3_nature_enum, gen3_nature_enum = generate_nature_enum()

    with open('../static/pokepoll/nature_enum.json', 'w') as f:
        json.dump(post_gen_3_nature_enum, f)

    with open('../static/pokepoll/gen3_nature_enum.json', 'w') as f:
        json.dump(gen3_nature_enum, f)




    
        




if __name__ == '__main__':
    main()