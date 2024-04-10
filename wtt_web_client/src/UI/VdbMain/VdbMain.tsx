import { Route, Routes } from "react-router-dom";
import VdbWelcome from '../VdbWelcome/VdbWelcome';
import AuthForm from "../AuthForm/AuthForm";
//import VdbDownload from '../VdbDownload/VdbDownload';
import VdbPersonal from '../VdbPersonal/VdbPersonal';
import NotFound from "../NotFound/NotFound";
import eps from '../../config/endpoints.json'
import RecoveryPassword from "../RecoveryPassword/RecoveryPassword";
import ForgotPassword from '../ForgotPassword/ForgotPassword';
import MainPanel from "../Editor/MainPanel";
import Scheduler from "../Editor/Scheduler";

const VdbMain: React.FC = () =>
{

    return (
        <Routes>
            <Route path="/" element={<VdbWelcome />} />
            {/* <Route path="/download" element={<VdbDownload />} /> */}
            <Route path="/auth" element={<AuthForm />} />
            <Route path="/personal" element={<VdbPersonal />} />
            {<Route path="/panel" element={<MainPanel />} />}
            {<Route path="/scheduler" element={<Scheduler />} />}
            <Route path="/forgot-password" element={<ForgotPassword />} />
            <Route path={eps.fontend.passwordRecovery + "/*"} element={<RecoveryPassword />} />
            <Route path="/*" element={<NotFound />} />
        </Routes>
    );
}

export default VdbMain;