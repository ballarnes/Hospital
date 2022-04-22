import { observer } from 'mobx-react'
import React, { useEffect } from 'react'
import { Badge, Col, Container, ListGroup, OverlayTrigger, Popover, Row, Spinner, Table } from 'react-bootstrap'
import { useTranslation } from 'react-i18next'
import { useAuth } from 'react-oidc-context'
import Image from 'react-bootstrap/Image'
import UserProfilePageStore from '../../stores/pages/UserProfilePageStore'
import ownTypes from '../../ioc/ownTypes'
import { useInjection } from '../../ioc/ioc.react'
import UpcomingAppointment from '../../components/UpcomingAppointment'
import { InfoCircle } from 'react-bootstrap-icons'
import Pagination from '../../components/Pagination'

const UserProfilePage = observer(() => {
  const store = useInjection<UserProfilePageStore>(ownTypes.userProfilePageStore);
  const { t } = useTranslation();
  const auth = useAuth();
  store.name = auth.user?.profile.name ?? "";

  const infoPopover = (
    <Popover id="popover-basic">
      <Popover.Header as="h3" className='text-center'>{t('personalArea.infoPopover.header')}</Popover.Header>
      <Popover.Body>{t('personalArea.infoPopover.body')}</Popover.Body>
    </Popover>
  );

  useEffect(() => {
    const getAppointments = async () => {
      await store.init();
    }
    getAppointments()
  }, [store, store.pageIndex, store.update])

  return (
    <Container className="pt-4 pb-4 justify-content-center">
      <h1 className='mb-4 text-center'>{t('personalArea.personalArea')}</h1>
      <Row className="justify-content-center">
        {store.isLoading ? (
          <Spinner animation="border" />
        ) : (
          <>
          <Row className="justify-content-center">
            <Col className="text-center">
              <Image src={'https://ui-avatars.com/api/?size=256&background=309896&color=fff&name=' + auth.user?.profile.given_name + '+' + auth.user?.profile.family_name} roundedCircle></Image>
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
          
          <Row className="justify-content-center">
            <hr style={{ background: "#309896", height: 4, width: "30%", marginTop: 100, marginBottom: 100 }}></hr>
          </Row>

          <Row className="justify-content-center">
            {store.appointments.length > 0 && (
              <>
              <Col><h3>{t('personalArea.upcomingAppointments')} </h3></Col>
              <Col><h3 className='text-end'><OverlayTrigger placement="left" overlay={infoPopover}>
                <InfoCircle style={{ cursor: "help" }} />
              </OverlayTrigger></h3></Col>
              <Table striped bordered hover style={{ backgroundColor: "rgba(48,152,150,0.1)" }}>
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>{t('appointment.doctorName')}</th>
                    <th>{t('appointment.doctorSurname')}</th>
                    <th>{t('appointment.specialization')}</th>
                    <th>{t('appointment.time')}</th>
                    <th>{t('appointment.date')}</th>
                    <th>{t('appointment.office')}</th>
                  </tr>
                </thead>
                <tbody>
                  {store.appointments?.map((appointment, key) => (
                    <UpcomingAppointment key={key} appointment={appointment} />
                  ))}
                </tbody>
              </Table>
              </>
            )}
          </Row>
          </>
        )}
      </Row>

      <Pagination total={store.pagesCount} active={store.pageIndex+1} onChange={(val) => { 
          store.changePage(val); 
        }}/>
    </Container>
  )
});

export default UserProfilePage;