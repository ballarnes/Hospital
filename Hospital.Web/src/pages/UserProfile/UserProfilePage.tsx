import { observer } from 'mobx-react'
import React from 'react'
import { Badge, Col, Container, ListGroup, Row, Spinner } from 'react-bootstrap'
import { useTranslation } from 'react-i18next'
import { useAuth } from 'react-oidc-context'
import Image from 'react-bootstrap/Image'

const UserProfilePage = observer(() => {
  const { t } = useTranslation();
  const auth = useAuth();

  return (
    <Container className="pt-4 pb-4 justify-content-center">
      <h1 className='mb-4 text-center'>{t('personalArea.personalArea')}</h1>
      <Row className="justify-content-center">
        {auth.isLoading ? (
          <Spinner animation="border" />
        ) : (
          <>
          <Row className="justify-content-center">
            <Col className="text-center">
              <Image src={'https://ui-avatars.com/api/?size=256&background=random&name=' + auth.user?.profile.given_name + '+' + auth.user?.profile.family_name} roundedCircle></Image>
            </Col>
            <Col>
              <ListGroup variant="flush">
                <ListGroup.Item>{t('personalArea.name')}: {auth.user?.profile.given_name}</ListGroup.Item>
                <ListGroup.Item>{t('personalArea.surname')}: {auth.user?.profile.family_name}</ListGroup.Item>
                <ListGroup.Item>
                  {t('personalArea.email')}: {auth.user?.profile.email} 
                  {auth.user?.profile.email_verified && (
                      <Badge pill style={{ marginLeft: 10 }} bg="success">{t('personalArea.verified')}</Badge>
                  )}
                </ListGroup.Item>
                {auth.user?.profile.idp != 'local' && (
                  <ListGroup.Item style={{ textAlign: "right" }}>
                    {t('personalArea.loggedInWith')}: {auth.user?.profile.idp}
                  </ListGroup.Item>
                )}
              </ListGroup>
            </Col>
          </Row>
          </>
        )}
      </Row>
    </Container>
  )
});

export default UserProfilePage;