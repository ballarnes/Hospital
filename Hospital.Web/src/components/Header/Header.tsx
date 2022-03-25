import { observer } from 'mobx-react';
import React from 'react'
import { Navbar, Nav, Container, Image, Button, NavDropdown } from 'react-bootstrap'
import { useTranslation } from 'react-i18next';
import { useNavigate } from "react-router-dom";

const Header = observer(() => {
  const navigate = useNavigate();
  const { t, i18n } = useTranslation();
  const changeLanguage = (lng: string) => {
    i18n.changeLanguage(lng);
  }

  return (
    <Container fluid style={{ paddingLeft: 0, paddingRight: 0 }}>
      <Navbar bg="dark" variant="dark">
          <Container>
            <Navbar.Brand onClick={() => navigate('/')}>
              <Image src="https://www.freeiconspng.com/thumbs/hospital-icon/ambulance-cross-hospital-icon-11.png"   width="30" height="30" rounded />
            </Navbar.Brand>
            <Nav className="me-auto" navbarScroll>
              <Button variant="dark" className='mx-2' onClick={() => navigate('/')}>{t('homePage')}</Button>
              <Button variant="dark" className='mx-2'  onClick={() => navigate('/specializations')}>{t('specializations')}</Button>
              <NavDropdown title={t('changeLang')} id="basic-nav-dropdown" className="justify-content-end">
                <NavDropdown.Item onClick={() => changeLanguage('ru')}>RU</NavDropdown.Item>
                <NavDropdown.Item onClick={() => changeLanguage('en')}>EN</NavDropdown.Item>
              </NavDropdown>
            </Nav>
          </Container>
        </Navbar>
    </Container>
  )
});

export default Header