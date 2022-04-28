import logo from './logo.svg';
import './App.css';
import {useState} from 'react';
import WelcomePage from './Pages/WelcomePage';
import CounterPage from './Pages/CounterPage';
import CheckboxPage from './Pages/CheckboxPage';
import PokemonPage from './Pages/PokemonPage';
import PokemonsPage from './Pages/PokemonsPage';
function App() {
  const [currentPage,setCurrentPage] = useState('Welcome');
  const OPTION = {
    WELCOME: 'welcome',
    COUNTER: 'counter',
    CHECKBOX: 'checkbox',
    POKEMON:'pokemon',
    POKEMONS:'pokemon list'
  }
  const handleChange = (evt) => {
    setCurrentPage(evt.target.value);
  }
  const getPage = ()=>{
    switch(currentPage){
      case OPTION.WELCOME: return <WelcomePage/>
      case OPTION.COUNTER: return <CounterPage/>
      case OPTION.CHECKBOX: return <CheckboxPage/>
      case OPTION.POKEMON: return <PokemonPage/>
      case OPTION.POKEMONS: return <PokemonsPage/>
      default : return <p>Not Yet Implemented</p>
    }
  }
  return (
    <div className="App">
      <div>
        <p>Current page: {currentPage}</p>
        <select onChange={handleChange} value={currentPage}>
          <option value={OPTION.WELCOME}>Show Welcome</option>
          <option value={OPTION.COUNTER}>Show Counter</option>
          <option value={OPTION.CHECKBOX}>Show Checkboxs</option>
          <option value={OPTION.POKEMON}>Show Pokemon</option>
          <option value={OPTION.POKEMONS}>Show Pokemons</option>
        </select> 
        {getPage()}
      </div>
    </div>
  );
}

export default App;
