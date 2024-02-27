import cl from "./VdbWelcome.module.css";
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import { useState, useEffect } from 'react';



const VdbWelcome: React.FC = () =>
{
    const [fastTransSt, setFastTransSt] = useState(false);
    const [secureTransSt, setSecureTransSt] = useState(false);
    const [sourceTransSt, setSourceTransSt] = useState(false);
    const [descrTransSt, setDescrTransSt] = useState(false);
    const [othersTransSt, setOthersTransSt] = useState(false);

    useEffect(() =>
    {
        setTimeout(() => setFastTransSt(true), 0);
        setTimeout(() => setSecureTransSt(true), 200);
        setTimeout(() => setSourceTransSt(true), 400);
        setTimeout(() => setDescrTransSt(true), 600);
        setTimeout(() => setOthersTransSt(true), 800);
    }, []);

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
        <span className={cl.welcomeWrapper}>
            <span className={cl.welcomeRow}>
                <strong>
                    <span className={cl.welcomWords}>
                        <CSSTransition in={fastTransSt} {...commonTransProp}>
                            <span className={cl.welcomeWord}>
                                Flexible.
                            </span >
                        </CSSTransition>
                        <CSSTransition in={secureTransSt} {...commonTransProp}>
                            <span className={cl.welcomeWord}>
                                Reliable.
                            </span>
                        </CSSTransition>
                        <CSSTransition in={sourceTransSt} {...commonTransProp}>
                            <span className={cl.welcomeWord}>
                                Scalable.
                            </span>
                        </CSSTransition>
                    </span>
                </strong>
                <span className={cl.welcomeDescription}>
                    <span className={cl.welcomWords}>
                        <CSSTransition in={descrTransSt} {...commonTransProp}>
                            <span className={cl.welcomeDecriptionPhrase}>
                                Many types of the test actions are supported.
                            </span >
                        </CSSTransition>
                        <CSSTransition in={descrTransSt} {...commonTransProp}>
                            <span className={cl.welcomeDecriptionPhrase}>
                                Every action is being logged to the database.
                                Email fail reports available.
                            </span>
                        </CSSTransition>
                        <CSSTransition in={descrTransSt} {...commonTransProp}>
                            <span className={cl.welcomeDecriptionPhrase}>
                                Test scenarios can call each others.
                                Just like functions in programming!
                            </span>
                        </CSSTransition>
                    </span>
                </span>
            </span>
            {/* <CSSTransition in={othersTransSt} {...commonTransProp}>
                <span className={cl.currentStatusPhrase}>
                    Current status:
                </span>
            </CSSTransition>
            <CSSTransition in={othersTransSt} {...commonTransProp}>
                <span className={cl.nodesList}>
                    {<VdbActiveNodes />}
                </span>
            </CSSTransition> */}
            <CSSTransition in={othersTransSt} {...commonTransProp}>
                <span className={cl.currentStatusPhrase}>
                    About:
                </span>
            </CSSTransition>
            <CSSTransition in={othersTransSt} {...commonTransProp}>
                {/* https://medium.com/@ist.stevkovski/is-it-front-end-or-front-end-or-frontend-3ae717cae4aa */}
                <span className={cl.aboutText}>
                    <p style={{ marginTop: ".75rem" }}>
                        Wtt (Web testing tool) is a service originally developed by a single&nbsp;
                        <a href="https://t.me/luminodiode" className={cl.meLink}>person</a> as a bachelor's graduate work.
                        However, it is truly operational.
                        The service is using <b>ReactTS</b> as a front-end, <b>ASP WebAPI</b> as a back-end.
                        All the back-end services are containerized using <b>Docker</b> and launched with compose.&nbsp;
                        {/* </p>
                    <p style={{ marginTop: ".75rem" }}> */}
                        The <b>NGINX</b>es all across the app are configured to use only strong ciphers (according to the SSL Labs classification).&nbsp;
                        <b>Certbot</b> is configured to use ECDsa keys, however is being overriden by a <b>Cloudflare</b>'s RSA
                        (and then by my Kaspersky antivirus {String.fromCodePoint(129760)}). Only the CDN can access the website directly from their IP addresses,
                        while others will receive unexpected connection closure.
                    </p>
                </span>
            </CSSTransition>
            <CSSTransition in={othersTransSt} {...commonTransProp}>
                <span className={cl.currentStatusPhrase}>
                    Open-source:
                </span>
            </CSSTransition>
            <CSSTransition in={othersTransSt} {...commonTransProp}>
                <span className={cl.aboutText}>
                    <p style={{ marginTop: ".75rem" }}>
                        VPN Servers
                    </p>
                    <p style={{ marginLeft: "2.75rem" }}>

                        <a
                            href="https://github.com/LuminoDiode/rest2wireguard"
                            className={cl.myLink}>
                            github.com/LuminoDiode/rest2wireguard
                        </a>
                    </p>
                    <p style={{ marginTop: ".75rem" }}>
                        Main Server
                    </p>
                    <p style={{ marginLeft: "2.75rem" }}>

                        <a
                            href="https://github.com/LuminoDiode/vdb_main_server"
                            className={cl.myLink}>
                            github.com/LuminoDiode/vdb_main_server
                        </a>
                    </p>
                    <p style={{ marginTop: ".75rem" }}>
                        Web Client
                    </p>
                    <p style={{ marginLeft: "2.75rem" }}>

                        <a
                            href="https://github.com/LuminoDiode/vdb_web_client"
                            className={cl.myLink}>
                            github.com/LuminoDiode/vdb_web_client
                        </a>
                    </p>
                    <p style={{ marginTop: ".75rem" }}>
                        Dekstop App
                    </p>
                    <p style={{ marginLeft: "2.75rem" }}>

                        <a
                            href="https://github.com/LuminoDiode/vdb_desktop_client"
                            className={cl.myLink}>
                            github.com/LuminoDiode/vdb_desktop_client
                        </a>
                    </p>
                </span>
            </CSSTransition >
            <hr style={{ marginTop: "6rem", color: "lightgray" }}></hr>
            <span className={cl.outerText}>RRV @2023-2024</span>
        </span >
    );
}

export default VdbWelcome;