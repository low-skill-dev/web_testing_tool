import cl from "./VdbPersonal.module.css";
import { NavLink, useNavigate } from 'react-router-dom';
import GlobalContext from '../../helpers/GlobalContext';
import hrefs from "../../config/hrefsList.json";
import { useState, useEffect, useMemo } from 'react';
import { CSSTransition } from 'react-transition-group';
import AuthorizedApiInteractionBase from "src/helpers/Api/AuthorizedApiInteractionBase";
import AuthHelper from "src/helpers/Api/AuthHelper";
import EnvHelper from "src/helpers/Common/EnvHelper";
// import DevicesList from "../DevicesList/DevicesList";
// import AuthHelper from '../../helpers/AuthHelper';
// import UserApiHelper from '../../helpers/UserApiHelper';
// import ISessionsResponse from '../../models/Auth/ISessionsResponse';
// import VdbSecurity from '../VdbSecurity/VdbSecurity';
// import EnvHelper from '../../helpers/Common/EnvHelper';
// import IUserInfoFromJwt from '../../models/Auth/IUserInfoFromJwt';
// import ApiHelper from '../../helpers/ApiHelper';

const VdbPersonal: React.FC = () =>
{
	const [transState, setTransState] = useState(false);
	const [userEmail, setUserEmail] = useState<string | null>(null);
	const navigate = useNavigate();

	useMemo(async () =>
	{
		try
		{
			var user = await AuthorizedApiInteractionBase.Create();
			if (user?.Access) setUserEmail(user.Access.Email);
			else throw new Error();
		}
		catch (e)
		{
			if (e instanceof Error && e.message) console.info(e.message);
			navigate("/auth");
		}

		setTransState(true);
	}, [navigate]);

	const logout = async () =>
	{
		await AuthHelper.TerminateSession();
		navigate("/");
		//window.location.reload();
	}


	const transitionClasses = {
		enterActive: cl.welcomeTransitionEnter,
		enterDone: cl.welcomeTransitionEnterActive,
		exitActive: cl.welcomeTransitionExit,
		exitDone: cl.welcomeTransitionExitActive,
	}
	const commonTransProp = {
		timeout: 300,
		classNames: transitionClasses
	}

	return (
		<CSSTransition in={transState} {...commonTransProp}>
			<span className={cl.personalWrapper}>
				<span className={cl.loggedAs}>
					You are logged in as: <span className={cl.loggedAsEmail}>
						{userEmail ?? "unknown"}
					</span>
					<button className={cl.logoutBtn} onClick={logout}>
						LOG OUT
					</button>
				</span>
				<span className={cl.personalText}>
					Security:
				</span>
				{/* <VdbSecurity /> */}
			</span>
		</CSSTransition>
	);
}

export default VdbPersonal;