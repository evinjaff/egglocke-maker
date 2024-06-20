

async function populateIcons() {
    let icons = document.getElementsByClassName("pokemon-icon");
    for (let i = 0; i < icons.length; i++) {
        let dexNumber = icons[i].getAttribute("pokemon-dex");
        let isShiny = icons[i].getAttribute("pokemon_is_shiny");
        // convert to boolean
        isShiny = isShiny === "true" || isShiny === "True";

        console.log(isShiny);
        let spriteURL = await getPokemonSpriteURL(dexNumber, isShiny);

        icons[i].src = spriteURL;
    }
}