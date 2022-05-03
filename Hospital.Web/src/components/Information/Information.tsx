import React, { useEffect } from 'react'
import { Alert, Badge, Button, CloseButton, Col, Container, FloatingLabel, Form, ListGroup, Row, Spinner } from 'react-bootstrap'
import ownTypes from '../../ioc/ownTypes'
import { observer } from 'mobx-react'
import { useInjection } from '../../ioc/ioc.react'
import { useTranslation } from 'react-i18next';
import { InformationStore } from '../../stores/components'

import "react-datepicker/dist/react-datepicker.css";

const Information = observer(() => {
  const store = useInjection<InformationStore>(ownTypes.informationStore);
  const { t } = useTranslation();

  useEffect(() => {
    const getSpec = async () => {
      await store.init();
    }

    getSpec()
  }, [store])

  return (
    <Container>
      <Row className="justify-content-center">
        {store.isLoading ? (
          <Spinner animation="border" />
        ) : (
          <>
          <Col lg={4} md={6} xs>
            <div className="d-grid gap-2">
              {!!!store.appointment ? (
                <>
                <FloatingLabel
                controlId="floatingInput"
                label={t("appointment.enterId")}
                className="mb-3"
                >
                  <Form.Control
                  type="number"
                  min={1}
                  placeholder={t("appointment.enterId")}
                  value={store.id}
                  onChange={(ev)=> {store.changeId(ev.target.value)}} />
                </FloatingLabel>
              <Button variant="outline-success" onClick={store.getAppointment}>OK</Button>
                </>
              ) : (
                <>
                <Row>
                  <Col style={{ textAlign: "right" }}>
                    <CloseButton onClick={store.changeAppointment}/>
                  </Col>
                </Row>
                <ListGroup variant="flush">
                  <ListGroup.Item><h4>{t('appointment.doctorName')}: <Badge bg="success" pill>{store.appointment.doctor.name} {store.appointment.doctor.surname}</Badge></h4></ListGroup.Item>
                  <ListGroup.Item><h4>{t('appointment.specialization')}: <Badge bg="success" pill>{store.appointment.doctor.specialization.name}</Badge></h4></ListGroup.Item>
                  <ListGroup.Item><h4>{t('appointment.time')}: <Badge bg="success" pill>{store.appointment.interval.start.hours}:{store.appointment.interval.start.minutes}{store.appointment.interval.start.minutes == 0 && 0} - {store.appointment.interval.end.hours}:{store.appointment.interval.end.minutes}{store.appointment.interval.end.minutes == 0 && 0}</Badge></h4></ListGroup.Item>
                  <ListGroup.Item><h4>{t('appointment.date')}: <Badge bg="success" pill>{new Date(store.appointment.date).toLocaleDateString()}</Badge></h4></ListGroup.Item>
                  <ListGroup.Item><h4>{t('appointment.office')}: <Badge bg="success" pill>{store.appointment.office.number}</Badge></h4></ListGroup.Item>
                  <ListGroup.Item><h4>{t('appointment.patientName')}: <Badge bg="success" pill>{store.appointment.patientName}</Badge></h4></ListGroup.Item>
                </ListGroup>
                </>
              )}
              {store.error && (
                <Alert variant="danger">
                  <Alert.Heading>{t("error.sorry")},</Alert.Heading>
                  <p>
                    {t("error.notFoundAppointment")}
                  </p>
                  <hr />
                  <p className="mb-0 d-flex justify-content-end">
                    {t("error.tryLater")}
                  </p>
                </Alert>
              )}
            </div>
          </Col>
          </>
        )}
      </Row>
    </Container>
  )
});

export default Information