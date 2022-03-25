import 'bootstrap/dist/css/bootstrap.min.css';
import React from 'react'
import ReactDOM from 'react-dom'
import App from './App/App'
import { IoCProvider } from './ioc/ioc.react'
import { container } from './ioc/ioc';
import { configure } from "mobx"

configure({
    enforceActions: "never",
})


ReactDOM.render(<React.StrictMode>
                    <IoCProvider container={container}>
                        <App/>
                    </IoCProvider>
                </React.StrictMode>, document.getElementById('root'))