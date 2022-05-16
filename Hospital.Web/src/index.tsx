import 'bootstrap/dist/css/bootstrap.min.css';
import "react-datepicker/dist/react-datepicker.css";
import '../static/colors.css';
import React from 'react'
import ReactDOM from 'react-dom'
import App from './App/App'
import { IoCProvider } from './ioc/ioc.react'
import { container } from './ioc/ioc';
import { configure } from "mobx"
import { AuthProvider } from 'react-oidc-context';

configure({
    enforceActions: "never",
})

const oidcConfig = {
    authority: "http://localhost:5000/",
    client_id: "hospitalui_pkce",
    client_secret: 'secret',
    redirect_uri: "http://localhost:5001/",
    response_type: 'code',
    scope: 'openid profile email hospital.appointment',
    loadUserInfo: true
};

ReactDOM.render(<AuthProvider {...oidcConfig}><React.StrictMode>
                    <IoCProvider container={container}>
                        <App/>
                    </IoCProvider>
                </React.StrictMode></AuthProvider>, document.getElementById('root'))