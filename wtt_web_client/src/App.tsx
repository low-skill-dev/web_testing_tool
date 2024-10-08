import axios from 'axios';
import VdbHeader from './UI/VdbHeader/VdbHeader';
import cl from "./App.module.css";
import { BrowserRouter } from 'react-router-dom';
import VdbMain from './UI/VdbMain/VdbMain';
import EnvHelper from './helpers/Common/EnvHelper';

function App()
{
  console.info(`Is debug mode: ${EnvHelper.isDebugMode}`);

  axios.defaults.withCredentials = true;
  axios.defaults.validateStatus = () => true;
  if (EnvHelper.isDebugMode)
  {
    axios.defaults.timeout = 360 * 1000;
  } else
  {
    axios.defaults.timeout = 10000;
  }

  //AuthHelper.EnsureUserInContext();

  return (
    <span className={cl.wrapper}>
      <span className={cl.app}>
        <BrowserRouter>
          <VdbHeader />
          <VdbMain />
        </BrowserRouter>
      </span>
    </span>
  );
}

export default App;
