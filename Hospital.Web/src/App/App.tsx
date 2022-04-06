import React from 'react'
import '../locales/config';
import { Route, Routes, Navigate, BrowserRouter } from 'react-router-dom';
import HomePage from '../pages/Home';
import Header from '../components/Header';
import { observer } from 'mobx-react';
import SpecializationsPage from '../pages/Specializations';
import { useAuth } from 'react-oidc-context';
import UserProfilePage from '../pages/UserProfile';

const App = observer(() => {
  const auth = useAuth();
  return (
          <BrowserRouter>
            <Header/>
                <Routes>
                  <Route path="/" element={<HomePage />} />
                  <Route path="*" element={<Navigate replace to="/" />} />
                  <Route path="specializations" element={<SpecializationsPage />} />
                  <Route path="profile" element={auth.isAuthenticated 
                      ? <UserProfilePage />
                      : <Navigate replace to="/" />} />
                </Routes>
            </BrowserRouter>
        )
});

export default App