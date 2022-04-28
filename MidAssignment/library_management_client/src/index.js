import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import "antd/dist/antd.css"
import "bootstrap-icons/font/bootstrap-icons.css";
import Authorization from './Share/Intercreptors/Authorization';
import { BrowserRouter ,HashRouter} from 'react-router-dom';
Authorization()
ReactDOM.render(
  <React.StrictMode>
     <HashRouter hashType="noslash">
      <App />
    </HashRouter>
  </React.StrictMode>,
  document.getElementById('root')
);

reportWebVitals();
