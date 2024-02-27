import cl from "./AuthForm.module.css";
import { CSSTransition } from 'react-transition-group';
import { useState, useEffect, useMemo } from 'react';
//import AuthHelper from '../../helpers/AuthHelper';
//import RegistrationRequest from '../../models/Auth/RegistrationRequest';
import ValidationHelper from '../../helpers/ValidationHelper';
import AuthorizedApiInteractionBase from "src/helpers/Api/AuthorizedApiInteractionBase";
import AuthHelper from "src/helpers/Api/AuthHelper";
import { useNavigate } from "react-router-dom";

const AuthForm: React.FC = () =>
{
    const navigate = useNavigate();
    const [transState, setTransState] = useState(false);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [submitEnabled, setSubmitEnabled] = useState(true);
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() =>
    {
        setTransState(true);
    }, []);

    useMemo(async () =>
    {
        try { var auth = await AuthorizedApiInteractionBase.Create(); } catch { }
        if (auth?.Access) navigate("/personal");
    }, [navigate]); // TODO: delete dep (use []) ?

    const onSubmit = async () =>
    {
        let emailError = ValidationHelper.ValidateEmailAndGetError(email);
        if (emailError)
        {
            console.info("Email failed client-side validation.");
            setErrorMessage(emailError);
            return;
        }

        let passError = ValidationHelper.ValidatePasswordAndGetError(password);
        if (passError)
        {
            console.info("Password failed client-side validation.");
            setErrorMessage(passError);
            return;
        }

        if (emailError || passError)
        {
            setSubmitEnabled(true);
            return;
        }

        setErrorMessage("");
        console.info(`Submitting: ${email}:${password}`);

        let result = -1;
        try
        {
            result = (await AuthHelper.Register(email, password));
        } catch { }

        if (result !== 200)
        {
            if (result > 0)
            {
                let selectedError: string;

                if (result === 400)
                    selectedError = "Problem on client side. Please reload window.";
                else if (result === 401)
                    selectedError = "Wrong password.";
                else if (result === 404)
                    selectedError = "Server was unable to find existing account and refused to create a new one.";
                else if (result === 409)
                    selectedError = "Server has found existing account but refused to authorize it.";
                else if (result >= 500)
                    selectedError = "Problem on server side. Please try later.";
                else
                {
                    selectedError = "Unable to send data to server.";
                }

                setErrorMessage(selectedError + ` Error code: HTTP_${result}.`);
            }
            else
            {
                setErrorMessage("Unable to send data to server.")
            }
        }
        else
        {
            console.info("Redirecting to personal...");
            window.location.replace("/personal");
            return;
        }
        setSubmitEnabled(true);
    }


    const transitionClasses = {
        enterActive: cl.welcomeTransitionEnter,
        enterDone: cl.welcomeTransitionEnterActive,
        exitActive: cl.welcomeTransitionExit,
        exitDone: cl.welcomeTransitionExitActive,
    }
    const commonTransProp = {
        timeout: 200,
        classNames: transitionClasses
    }

    return (
        <CSSTransition in={transState} {...commonTransProp}>
            <span className={cl.authWrapper}>
                <span className={cl.authHeader}>
                    <strong>Sign in</strong>&nbsp;or&nbsp;<strong>Sign up</strong>
                </span>
                <span className={cl.credentialsLabel}>Email</span>
                <input
                    onKeyUp={e => { if (e.key.toLowerCase() === "enter") onSubmit(); }}
                    onDragEnter={onSubmit}
                    type={"email"}
                    placeholder="Email"
                    value={email} onChange={(e) => setEmail(e.target.value)}
                    className={cl.credentialsInput} />
                <span className={cl.credentialsLabel}>Password</span>
                <input
                    onKeyUp={e => { if (e.key.toLowerCase() === "enter") onSubmit(); }}
                    type={"password"}
                    placeholder="Password"
                    value={password} onChange={(e) => setPassword(e.target.value)}
                    className={cl.credentialsInput} />
                { /* eslint-disable-next-line jsx-a11y/anchor-is-valid*/}
                <a className={cl.forgotPass} onClick={() => { navigate("/forgot-password") }} style={{ cursor: "pointer" }}>
                    <span>I forgot it!</span>
                </a>
                <button
                    type="submit"
                    onClick={onSubmit}
                    className={cl.authSubmit}
                    disabled={!submitEnabled}>SUBMIT</button>
                {errorMessage
                    ? <span className={cl.errorWrapper}>
                        <span>{errorMessage}</span>
                    </span>
                    : <span />
                }
            </span>
        </CSSTransition>
    );
}

export default AuthForm;