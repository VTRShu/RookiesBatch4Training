import React from 'react'

const PokemonsComponent = ({searchText, sortByName, handleSortByName,pokemonSorted,setSearchText}) => {
  
   return(
    <div>
    <input placeholder="search for pokemon" value={searchText} onChange={(evt)=> setSearchText(evt.target.value)}/>
    <table>
        <thead>
            <tr>
                <th onClick={handleSortByName}>Name ({sortByName})</th>
                <th>URL</th>
            </tr>
        </thead>
        <tbody>
            {
                pokemonSorted.map(pokemon =>
                    {
                        return(
                        <tr key={pokemon.name}>
                            <td>{pokemon.name}</td>
                            <td><a href={pokemon.url}>{pokemon.url}</a></td>
                        </tr>)
                    }
                )
            }
        </tbody>
    </table>
</div>
   )
 
};



export default PokemonsComponent;
