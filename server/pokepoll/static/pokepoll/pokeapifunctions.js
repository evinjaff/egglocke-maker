

function getPokemonSpriteURL(pokemonID, isShiny, isShowdown) {
    base_url = `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/`
    extension = `.png`

    if (isShowdown) {
        base_url = `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/showdown/`
        extension = `.gif`
    }



    if (isShiny) {
        return `${base_url}shiny/${pokemonID}${extension}`
    }

    return `${base_url}${pokemonID}${extension}`
}



async function getPokemonNameFromID(pokemonID) {
    return fetch(`https://pokeapi.co/api/v2/pokemon/${pokemonID}`)
        .then(response => response.json())
        .then(data => data.name)
}

async function getPokemonIDFromName(pokemonName) {
    return fetch(`https://pokeapi.co/api/v2/pokemon/${pokemonName}`)
        .then(response => response.json())
        .then(data => data.id)
}

async function pokemonIDandStatsMultiGet(pokemonName) { 
    return fetch(`https://pokeapi.co/api/v2/pokemon/${pokemonName}`)
        .then(response => response.json())
        .then(data => {
            return {
                id: data.id,
                stats: data.stats
            }
        })
 }