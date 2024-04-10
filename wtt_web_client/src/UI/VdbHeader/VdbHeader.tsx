import cl from "./VdbHeader.module.css";
import { NavLink } from "react-router-dom";
//import GlobalContext from '../../helpers/GlobalContext';
import AuthorizedApiInteractionBase from "src/helpers/Api/AuthorizedApiInteractionBase";
import { useMemo, useState } from "react";
import { isatty } from "tty";

const VdbHeader: React.FC = () =>
{
    const [isAuthed, setIsAuthed] = useState(false);

    useMemo(async () =>
    {
        try
        {
            var user = await AuthorizedApiInteractionBase.Create();
            setIsAuthed(user?.Access ? true : false);
        }
        catch (e)
        {
            if (e instanceof Error) console.info(e.message);
            setIsAuthed(false);
        }
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
                        .lowskill.dev
                    </span>
                </NavLink >
                {/* <NavLink to="/download" className={cl.menuElement}>
                    Download
                </NavLink> */}
                <NavLink to={isAuthed ? "/personal" : "/auth"} className={cl.menuElement} >
                    Account
                </NavLink>
                <NavLink to={isAuthed ? "/panel" : "/auth"} className={cl.menuElement} >
                    Editor
                </NavLink>
                <NavLink to={isAuthed ? "/scheduler" : "/auth"} className={cl.menuElement} >
                    Scheduler
                </NavLink>
            </nav>
        </header>);
}

export default VdbHeader;