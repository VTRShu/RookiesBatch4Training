import React,{useState, useEffect}  from "react";
import axios from "axios";
import PokemonsComponent from "../Components/Pokemons/PokemonsComponent";
const PokemonsPage = ()=>{
    const[pokemons, setPokemons] = useState([]);
    const [pokemonId, setPokemonId] = useState(1);
    const [isLoading,setIsLoading] = useState(false);
    const [searchText,setSearchText] = useState('');
    const [sortByName, setSortByName] = useState('NONE');
    const [error,setError] = useState('');
    useEffect(()=>{
        setIsLoading(true);
        let didCancel = false;
        axios.get(`https://pokeapi.co/api/v2/pokemon?offset=00&limit=50`).then((response)=>{
            if(!didCancel)
            {   
                console.log(response);
                setPokemons(response.data.results);
                setIsLoading(false)
            }
        }).catch((error)=>{
            if(!didCancel)
            { 
                setIsLoading(false);
                setError('wrong!');
            }
        })
        return ()=>{
            didCancel = true;
        }
    },[])
    // const onSearchPokemon = ()=>{

    // }
    const handleSortByName = () =>{
        if(sortByName === 'NONE') 
        {
            setSortByName('ASC');
            return
        }if(sortByName === 'ASC')
        {
            setSortByName('DESC');
            return
        }if(sortByName === 'DESC')
        {
            setSortByName('NONE');
        };
    }
    const pokemonsFilter = pokemons.filter(pokemon => pokemon.name.toLowerCase().includes(searchText.toLowerCase()));
    const getPokemonsSorted = ()=>{
        if(sortByName === 'NONE') return pokemonsFilter;
        if(sortByName === 'ASC') return pokemonsFilter.sort((a,b)=> 
            {
                if(a.name.toLowerCase() < b.name.toLowerCase()) return -1;
                if(a.name.toLowerCase() > b.name.toLowerCase()) return 1;
            }
        );
        if(sortByName === 'DESC') return pokemonsFilter.sort((a,b)=> 
            {
                if(a.name.toLowerCase() < b.name.toLowerCase()) return 1;
                if(a.name.toLowerCase() > b.name.toLowerCase()) return -1;
            }
        );
    }
    const pokemonSorted = getPokemonsSorted();
    if(error)
    return(
        <div style={{color:'red'}}>{error}</div>
    )
    if(isLoading)
    {
        return(
            <div style={{color:'green'}}>loading....</div>
        )
    }
    return(
        <PokemonsComponent
            searchText = {searchText}
            sortByName = {sortByName}
            handleSortByName = {handleSortByName}
            pokemonSorted = {pokemonSorted}
            setSearchText = {setSearchText}
        />
    )
}

export default PokemonsPage;