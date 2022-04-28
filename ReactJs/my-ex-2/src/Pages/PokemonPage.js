import React, { useState, useEffect } from "react";
import axios from "axios";
import PokemonComponent from "../Components/Pokemon/PokemonComponent"
const PokemonPage = ()=>{
    const [currentPokemonId, setCurrentPokemonId] = useState(1);
    const [isLoading, setIsLoading] = useState(true);
    const [pokemon, setPokemon] = useState({
        name: null,
        height: null,
        weight: null,
        frontImage: null,
        backImage: null
    });
  
    useEffect(()=>{
        let didCancel = false;
        axios.get(`https://pokeapi.co/api/v2/pokemon/${currentPokemonId}`
        // ,{cancelToken: new CancelToken((c)=>{
        //     cancel = c;
        // })}
        ).then(response =>{
            if(!didCancel)
            {
                setPokemon({
                    id: response.data.id,
                    name: response.data.name,
                    height: response.data.height,
                    weight: response.data.weight,
                    frontImage: response.data.sprites.front_default,
                    backImage: response.data.sprites.back_default
                })
                console.log(pokemon)
                setIsLoading(false);
            }
        });
        return ()=>{
            didCancel = true;
        }
    },[currentPokemonId]);
    const handleNext = ()=>{
        setCurrentPokemonId(currentPokemonId + 1);
    }
    const handlePrevious = ()=>{
        setCurrentPokemonId(currentPokemonId - 1);
    }
    useEffect(()=>{
        document.title = `${pokemon.name}`
    },[pokemon.name])

    // useEffect(()=>{
    //     const myTimeout = setTimeout(()=>{
    //         setCurrentPokemonId(currentPokemonId + 1);
    //     },10000);
    //     return()=>{
    //         clearTimeout(myTimeout);
    //     }
    // },[currentPokemonId])
    if(isLoading){
        return <div>Loading</div>
    }else{
        return(
            <PokemonComponent
                values = {pokemon}
                currentId={currentPokemonId}
                setCurrentPokemonId = {setCurrentPokemonId}
                handleNext = {handleNext}
                handlePrevious = {handlePrevious}
            />
        )
    }
}

export default PokemonPage;