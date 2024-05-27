import pypokedex
import json

# Constants

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

def import_json(filepath):
    with open(filepath, 'r') as f:
        data = json.load(f)
    return data
        

def main():

    # Derive constant JSON files from static/pokepoll/pokemdex.json
    pokedex = import_json('static/pokepoll/pokedex.json')

    # file 1: all_pokemon_names.json
    all_pokemon_names = []
    for pokemon in pokedex:
        all_pokemon_names.append(pokemon['name']["english"])

    with open('static/pokepoll/all_pokemon_names.json', 'w') as f:
        json.dump(all_pokemon_names, f)




if __name__ == '__main__':
    main()