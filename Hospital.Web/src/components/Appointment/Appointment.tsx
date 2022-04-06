import React, { useEffect } from 'react'
import { Alert, Button, Col, Container, FloatingLabel, Form, Row, Spinner } from 'react-bootstrap'
import ownTypes from '../../ioc/ownTypes'
import { observer } from 'mobx-react'
import { useInjection } from '../../ioc/ioc.react'
import { useTranslation } from 'react-i18next';
import { AppointmentStore } from '../../stores/components'
import DatePicker from "react-datepicker";

import "react-datepicker/dist/react-datepicker.css";
import { useAuth } from 'react-oidc-context'

const Appointment = observer(() => {
  const store = useInjection<AppointmentStore>(ownTypes.appointmentStore);
  const { t, i18n } = useTranslation();
  const auth = useAuth();
  const authorization = auth.user?.access_token ?? '';

  useEffect(() => {
    const getSpec = async () => {
      await store.init(authorization);
    }

    getSpec()
  }, [store, authorization])

  const isWeekday = (date: Date) => {
    const day = date.getDay();
    return day !== 0 && day !== 6;
  };

  return (
    <Container>
      <Row className="justify-content-center">
        {store.isLoading ? (
          <Spinner animation="border" />
        ) : (
          <>
          <Col lg={4} md={6} xs>
            <div className="d-grid gap-2">
              {!store.specializationSelected && (
                <>
                <h3 className='mb-4 text-center'>{t('selectSpecialization')}</h3>
                <Form.Select className="mb-3" aria-label={t("selectSpecialization")} onChange={(ev) => store.changeSpecialization(ev.target.value)}>
                <option disabled value='0'>{t("selectSpecialization")}</option>
                {store.specializations?.map((specialization, key) => (
                  <option key={key} value={specialization.id}>{specialization.name}</option>
                ))}
              </Form.Select>
              <Button variant="outline-success" onClick={store.selectSpecialization}>OK</Button>
                </>
              )}
              {(store.specializationSelected && !store.doctorSelected) && (
                <>
                <h3 className='mb-4 text-center'>{t('selectDoctor')}</h3>
                <Form.Select className="mb-3" aria-label={t("selectDoctor")} onChange={(ev) => store.changeDoctor(ev.target.value)}>
                <option disabled value='0'>{t("selectDoctor")}</option>
                {store.doctors?.map((doctor, key) => (
                  <option key={key} value={doctor.id}>{doctor.name} {doctor.surname}</option>
                ))}
                </Form.Select>
                <Button variant="outline-success" onClick={store.selectDoctor}>OK</Button>
                </>
              )}
              {(store.specializationSelected && store.doctorSelected && !store.dateSelected) && (
                <>
                <h3 className='mb-4 text-center'>{t('selectDateAndEnterName')}</h3>
                <Row className="justify-content-center text-center">
                  <Col>
                  <DatePicker
                    locale={store.getLocale(i18n.language)}
                    dateFormat={store.getDateFormat(i18n.language)}
                    selected={store.date}
                    onChange={(date: Date) => store.changeDate(date)}
                    minDate={new Date()}
                    maxDate={store.addDays(new Date(), 14)}
                    filterDate={isWeekday}
                    className="mb-3 text-center"
                    withPortal
                  />
                  </Col>
                </Row>
                <FloatingLabel
                controlId="floatingInput"
                label={t("patientName")}
                className="mb-3"
                >
                  <Form.Control
                  type="text"
                  disabled={true}
                  placeholder={t("patientName")}
                  value={auth.user?.profile.name}
                  onChange={(ev)=> {store.changeName(ev.target.value)}} />
                </FloatingLabel>
                <Button variant="outline-success" onClick={() => { store.changeName(auth.user?.profile.name ?? ''); }}>OK</Button>
                {!!store.nameError && (
                  <p style={{ color: 'red', fontSize: 14 }}>{t("error.patientNameError")}</p>
                )}
                </>
              )}
              {(store.dateSelected && store.patientNameSelected && store.intervals.length > 0 && !store.officeSelected) && (
                <>
                <h3 className='mb-4 text-center'>{t('selectInterval')}</h3>
                <Form.Select className="mb-3" aria-label={t("selectInterval")} onChange={(ev) => store.changeInterval(ev.target.value)}>
                <option disabled value='0'>{t("selectInterval")}</option>
                {store.intervals?.map((interval, key) => (
                  <option key={key} value={interval.id}>{interval.start} - {interval.end}</option>
                ))}
                </Form.Select>
                <Button variant="outline-success" onClick={store.selectInterval}>OK</Button>
                </>
              )}
              {(store.dateSelected && store.patientNameSelected && store.intervals.length == 0 && !store.officeSelected) && (
                  <Alert variant="danger">
                  <Alert.Heading>{t("error.sorry")},</Alert.Heading>
                  <p>
                    {t("error.noIntervals")}
                  </p>
                  <hr />
                  <p className="mb-0 d-flex justify-content-end">
                    {t("error.tryToChangeTheDoctorOrDate")}
                  </p>
                </Alert>
                )}
              {store.intervalSelected && (
                  <>
                  {store.officeSelected ? (
                  <>
                  {store.appointmentCreated ? (
                    <>
                    <h3 className='mb-4 text-center'>{t('appointmentCreated')}</h3>
                    <Alert variant="success">
                      <Alert.Heading>{t("success.success")},</Alert.Heading>
                      <p>
                        {t("success.useThisId")}: <b>{store.id}</b>
                      </p>
                    </Alert>
                    </>
                  ) : (
                    <>
                    <Alert variant="danger">
                      <Alert.Heading>{t("error.sorry")},</Alert.Heading>
                      <p>
                        {t("error.appointmentNotCreated")}
                      </p>
                      <hr />
                      <p className="mb-0 d-flex justify-content-end">
                        {t("error.tryLater")}
                      </p>
                    </Alert>
                    </>
                  )}
                  </>
                ) : (
                  <>
                    <Alert variant="danger">
                      <Alert.Heading>{t("error.sorry")},</Alert.Heading>
                      <p>
                        {t("error.noOffices")}
                      </p>
                      <hr />
                      <p className="mb-0 d-flex justify-content-end">
                        {t("error.tryToChangeTheInterval")}
                      </p>
                    </Alert>
                  </>
                )}
                  </>
                )}
            </div>
          </Col>
          </>
        )}
      </Row>
    </Container>
  )
});

export default Appointment