import React from 'react'
import '../locales/config';
import { Route, Routes, Navigate, BrowserRouter } from 'react-router-dom';
import HomePage from '../pages/Home';
import Header from '../components/Header';
import { observer } from 'mobx-react';
import SpecializationsPage from '../pages/specializations';

const App = observer(() => {

  return (
          <BrowserRouter>
            <Header/>
                <Routes>
                  <Route path="/" element={<HomePage />} />
                  <Route path="*" element={<Navigate replace to="/" />} />
                  <Route path="specializations" element={<SpecializationsPage />} />
                </Routes>
            </BrowserRouter>
        )
});

export default App