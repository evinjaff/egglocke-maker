

function getPokemonSpriteURL(pokemonID) {
    return `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${pokemonID}.png`
}

async function getPokemonNameFromID(pokemonID) {
    return fetch(`https://pokeapi.co/api/v2/pokemon/${pokemonID}`)
        .then(response => response.json())
        .then(data => data.name)
}