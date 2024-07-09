

function getPokemonSpriteURL(pokemonID, isShiny) {
    base_url = `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/`

    if (isShiny) {
        return `${base_url}shiny/${pokemonID}.png`
    }

    return `${base_url}${pokemonID}.png`
}

async function getPokemonNameFromID(pokemonID) {
    return fetch(`https://pokeapi.co/api/v2/pokemon/${pokemonID}`)
        .then(response => response.json())
        .then(data => data.name)
}