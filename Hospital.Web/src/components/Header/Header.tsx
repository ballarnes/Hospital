import { observer } from 'mobx-react';
import React from 'react'
import { Navbar, Nav, Container, Image, Button, Offcanvas, ButtonGroup, Col, Row } from 'react-bootstrap'
import { useTranslation } from 'react-i18next';
import { useAuth } from 'react-oidc-context';
import { useNavigate } from "react-router-dom";

const Header = observer(() => {
  const navigate = useNavigate();
  const { t, i18n } = useTranslation();
  const auth = useAuth();
  
  const changeLanguage = (lng: string) => {
    i18n.changeLanguage(lng);
  }

  return (
    <Navbar bg="dark" variant="dark" expand={false}>
      <Container fluid style={{ paddingLeft: "7%", paddingRight: "7%" }}>
      <Navbar.Brand>
           <Image src={process.env.LOGO_URL} width="30" height="30" rounded />
           <Button variant="dark" className='pl-5 mx-2' onClick={() => navigate('/')}>{t('homePage')}</Button>
           <Button variant="dark" className='mx-2' onClick={() => navigate('/specializations')}>{t('specializations')}</Button>
      </Navbar.Brand>

      {auth.isAuthenticated && (
          <Navbar.Text style={{ marginLeft: "50%" }}>
            {t('signedIn')}: <a>{auth.user?.profile.given_name}</a>
          </Navbar.Text>
      )} 

      <Navbar.Toggle aria-controls="offcanvasNavbar" />
      <Navbar.Offcanvas
        id="offcanvasNavbar"
        aria-labelledby="offcanvasNavbarLabel"
        placement="end"
      >
        <Offcanvas.Header closeButton>
          <Image src={process.env.LOGO_URL} width="50" height="50" rounded />
          <Offcanvas.Title id="offcanvasNavbarLabel">Hospital</Offcanvas.Title>
        </Offcanvas.Header>

        <hr style={{ background: "linear-gradient(90deg, #309896, transparent)", height: 8 }}></hr>

        <Offcanvas.Body>
          <Nav className="justify-content-end flex-grow-1 pe-3">
            <ButtonGroup vertical>
              {auth.isAuthenticated ? (
                <>
                  <Button variant="light" className='mx-2' onClick={() => navigate('/profile')}>{t('personalArea.personalArea')}</Button>
                  <Button variant="light" className='mx-2'  onClick={() => auth.signoutRedirect()}>{t('logout')}</Button>
                </>
                ) : (
                <Button variant="light" className='mx-2'  onClick={() => auth.signinRedirect()}>{t('login')}</Button>
              )}
            </ButtonGroup>
            <Row>
              <Col md={{ span: 4 }} style={{ marginTop: 10 }}>
                <Image src="https://s1.iconbird.com/ico/1012/Flag/w64h641351180252UnitedStates.png" width={35} style={{ marginLeft: 7, cursor: "pointer" }} onClick={() => changeLanguage('en')}></Image>
                <Image src="https://s1.iconbird.com/ico/1012/Flag/w64h641351180228Ukraine.png" width={35} style={{ marginLeft: 14, cursor: "pointer" }} onClick={() => changeLanguage('ru')}></Image>
              </Col>
            </Row>
          </Nav>
        </Offcanvas.Body>
      </Navbar.Offcanvas>
      </Container>
    </Navbar>
  )
});

export default Header