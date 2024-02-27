import cl from "./VdbHeader.module.css";
import { NavLink } from "react-router-dom";
import GlobalContext from '../../helpers/GlobalContext';
import AuthorizedApiInteractionBase from "src/helpers/Api/AuthorizedApiInteractionBase";
import { useMemo, useState } from "react";

const VdbHeader: React.FC = () =>
{
    let [accountPath, setAccountPath] = useState<string>("/auth")

    useMemo(async () =>
    {
        try { var user = await AuthorizedApiInteractionBase.Create(); }
        catch (e) { if (e instanceof Error) console.info(e.message); }
        setAccountPath(user?.Access ? "/personal" : "/auth");
        console.info(`Account path was set to '${accountPath}'.`);
    }, []);

    return (
        <header className={cl.header}>
            <nav className={cl.menuRow}>
                <NavLink to="/" className={cl.titleWrapper}>
                    <span className={cl.beta}>
                        v0.1-beta
                    </span>
                    <span className={cl.title}>
                        <strong>
                            Wtt
                        </strong>
                    </span>
                    <span className={cl.subtitle}>
                        .bruhcontent.ru
                    </span>
                </NavLink >
                {/* <NavLink to="/download" className={cl.menuElement}>
                    Download
                </NavLink> */}
                <NavLink to={accountPath} className={cl.menuElement} >
                    Account
                </NavLink>
            </nav>
        </header>);
}

export default VdbHeader;