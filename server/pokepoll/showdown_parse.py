import re
import random
import os
import json
from django.conf import settings
from django.utils import timezone


# Format Strings for validation errors
MOVE_VALIDATION_ERROR = "Move {} is not a valid move"
HELD_ITEM_VALIDATION_ERROR = "Item {} is not a valid held item"
POKEMON_SPECIES_VALIDATION_ERROR = "Pokemon {} is not a valid pokemon"

def parseShowdownFormatGen4(block):
	
	patterns = {
	"name_nickname_item": re.compile(r"(?:([\w\s]+) \()?(\w+(?:-\w+)*)\)?\s*@\s*([\w\S ]+)", re.MULTILINE),
	"ability": re.compile(r"Ability:\s*([\w\S ]+)", re.MULTILINE),
	"nature": re.compile(r"(\w+)\sNature", re.MULTILINE),
	"moves": re.compile(r"- ([\w\S ]+)", re.MULTILINE),
	"is_shiny": re.compile(r"[Ss][Hh][Ii][Nn][Yy]:\s*[Yy][Ee][Ss]")
	}

	parsed_data = {}

	# Extract name and item
	match = patterns["name_nickname_item"].search(block)
	if match:
		nickname = match.group(1)  # May be None if there's no nickname
		name = match.group(2)      # Always the Pokémon's actual name
		item = match.group(3)      # Always the item

		parsed_data["name"] = name
		parsed_data["nickname"] = nickname if nickname else name  # Store None if no nickname
		parsed_data["item"] = item
	else:
		print("not found for name/item")

	# Extract ability
	match = patterns["ability"].search(block)
	if match:
		parsed_data["ability"] = match.group(1)
	else:
		print("not found for ability")

	# Extract Nature
	match = patterns["nature"].search(block)
	
	if match:
		parsed_data["nature"] = match.group(1)
	else:
		print("not found for nature")

	# Extract Moves
	parsed_data["moves"] = patterns["moves"].findall(block)

	# strip trailing space out of the tail end
	for i in parsed_data:
		if type(parsed_data[i]) == str:
			parsed_data[i] = parsed_data[i].rstrip()
		elif type(parsed_data[i]) == list:
			if len(parsed_data[i]) > 0 and type(parsed_data[i][0]) == str:
				string_arr = [""] * len(parsed_data[i])

				for list_idx in range(len(parsed_data[i])):
					string_arr[list_idx] = parsed_data[i][list_idx].rstrip()

				parsed_data[i] = string_arr


	# parse IVs and EVs
	parsed_data["evs"], parsed_data["ivs"] = parse_evs_ivs(block)

	# parse shiny status
	match = patterns["is_shiny"].search(block)
	if match:
		parsed_data["is_shiny"] = True
	else:
		parsed_data["is_shiny"] = False


	return parsed_data

def parse_evs_ivs(text):
	stats_order = ["HP", "Atk", "Def", "SpA", "SpD", "Spe"]
	
	evs = [0] * 6
	ivs = [ random.randint(0, 31) for i in range (1,7) ]  # Default IVs in Pokémon will be random integers from 0 - 31

	ev_match = re.search(r"EVs:\s*([\d\s/AtkDefSpASpDSpe]*)", text)
	iv_match = re.search(r"IVs:\s*([\d\s/AtkDefSpASpDSpe]*)", text)

	if ev_match:
		for entry in ev_match.group(1).split(" / "):
			parts = entry.split()
			if len(parts) == 2 and parts[1] in stats_order:
				evs[stats_order.index(parts[1])] = int(parts[0])

	if iv_match:
		for entry in iv_match.group(1).split(" / "):
			parts = entry.split()
			if len(parts) == 2 and parts[1] in stats_order:
				ivs[stats_order.index(parts[1])] = int(parts[0])

	return evs, ivs


def parseShowdownFormat(gen, raw):

	if gen == 4:
		return parseShowdownFormatGen4(raw)


def validate_good_showdown_format(gen, parse_result, dry_run=True, wet_run_creds=None):
	print(parse_result)

	# go through parse results and add objects

	# get name from species

	parsed_pokemon_species = None
	with open(os.path.join(settings.BASE_DIR, 'pokepoll/static/pokepoll/pokemon_name_to_id.json')) as f:
		pokedex = json.load(f)
		if parse_result["name"] in pokedex:
			parsed_pokemon_species = pokedex[parse_result["name"]]
		else:
			raise ValueError(POKEMON_SPECIES_VALIDATION_ERROR.format(parsed_pokemon_species))


	parsed_nickname = parse_result["nickname"]
	parsed_ivs = parse_result["ivs"]
	parsed_evs = parse_result["evs"]

	parsed_ability = parse_result["ability"]
	
	parsed_item = parse_result["item"]
	# Translate strings to ints
	with open(os.path.join(settings.BASE_DIR, 'pokepoll/static/pokepoll/held_items_to_id_gen_{}.json'.format(gen))) as f:
		item_dex = json.load(f)
		if parsed_item in item_dex:
			parsed_item = item_dex[parsed_item]
		else:
			raise ValueError(HELD_ITEM_VALIDATION_ERROR.format(parsed_item))
		
		

	parsed_moves = parse_result["moves"]
	parsed_nature = parse_result["nature"]

	# convert nature to ID
	with open(os.path.join(settings.BASE_DIR, 'pokepoll/static/pokepoll/nature_enum.json')) as f:
		nature_dex = json.load(f)
		if parsed_nature in nature_dex:
			parsed_nature = nature_dex[parsed_nature]
		else:
			parsed_nature = 0

	# convert ability to ID
	with open(os.path.join(settings.BASE_DIR, 'pokepoll/static/pokepoll/pokemon_ability_to_id_gen_{}.json'.format(gen))) as f:
		ability_dex = json.load(f)
		if parsed_ability in ability_dex:
			parsed_ability = ability_dex[parsed_ability]
		else:
			parsed_ability = 0

	# convert moves to ID
	with open(os.path.join(settings.BASE_DIR, 'pokepoll/static/pokepoll/pokemon_move_to_id_gen_{}.json'.format(gen))) as f:
		move_dex = json.load(f)

		for parsed_move_idx in range(len(parsed_moves)):
			if parsed_moves[parsed_move_idx] in move_dex:
				parsed_moves[parsed_move_idx] = move_dex[parsed_moves[parsed_move_idx]]
			else:
				raise ValueError( MOVE_VALIDATION_ERROR.format(parsed_moves[parsed_move_idx]) )
	
	kw_args = { 'pokemon_nickname': parsed_nickname,
		'pokemon_species': parsed_pokemon_species,
		'pub_date': timezone.now(),
		# 'submitter_id': foreign_key,
		'pokemon_is_shiny': parse_result["is_shiny"],
		'pokemon_IV': parsed_ivs,
		'pokemon_EV': parsed_evs,
		'pokemon_ability': parsed_ability,
		'pokemon_held_item': parsed_item,
		'pokemon_moves': parsed_moves,
		'pokemon_nature': parsed_nature,
		'pokemon_intended_generation': gen,
	}

	# synchronize non-pokemon related creds into object
	if not dry_run:
		for cred in wet_run_creds:
			kw_args[cred] = wet_run_creds[cred]

	# if any non-nullable fields are None, error
	non_nullable_fields = ["pokemon_species", "pokemon_ability"]
	bad_fields = []
	for non_nullable_field in non_nullable_fields:
		if kw_args[non_nullable_field] == None:
			bad_fields.append(non_nullable_field)

	if len(bad_fields) > 0:
		return {"code": 400, "bad_fields": bad_fields}



	return {"code": 200, "kw_args": kw_args}