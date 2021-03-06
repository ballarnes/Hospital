import { observer } from 'mobx-react';
import React, { useEffect } from 'react'
import { Alert, Button, Col, Form, Modal, Row, Spinner } from 'react-bootstrap';
import { useTranslation } from 'react-i18next';
import { useInjection } from '../../ioc/ioc.react';
import ownTypes from '../../ioc/ownTypes';
import { AppointmentStore, UpdateAppointmentModalStore } from '../../stores/components';
import DatePicker from "react-datepicker";
import { Appointment } from '../../models/Appointment';
import { useAuth } from 'react-oidc-context';
import UserProfilePageStore from '../../stores/pages/UserProfilePageStore';
import { setMinutes, setHours, addDays } from "date-fns";

interface Props {
  show: boolean,
  onHide: (() => void) | undefined,
  appointment: Appointment
}

const UpdateAppointmentModal = observer((props: Props) => {

  const store = useInjection<UpdateAppointmentModalStore>(ownTypes.updateAppointmentModalStore);
  const profilePageStore = useInjection<UserProfilePageStore>(ownTypes.userProfilePageStore);
  store.appointment = props.appointment;
  const auth = useAuth();
  const authorization = auth.user?.access_token ?? '';
  
  useEffect(() => {
    const getModal = async () => {
      await store.init(authorization);
    }
    getModal()
  }, [store, authorization])

  const onExit = () => {
    setTimeout(store.clear, 400);
  }

  const appointmentStore = useInjection<AppointmentStore>(ownTypes.appointmentStore);
  const { t, i18n } = useTranslation();

  const isWeekday = (date: Date) => {
    const day = date.getDay();
    return day !== 0 && day !== 6;
  };

  return (
    <>
      <Modal {...props} centered onExit={onExit}>
        <Modal.Header closeButton>
          <Modal.Title>{t('personalArea.updateModal.header')}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {store.isLoading ? (
            <Spinner animation="border"/>
          ) : (
          <>
          {store.saved && (
            <Alert variant="success">{t('saved')}!</Alert>
          )}
          {store.delete && !store.deleted ? (
            <p>{t("personalArea.updateModal.subDelete")}</p>
          ) : (
            <Form>
              <p>{t('appointment.doctorName')}: <strong>{store.appointment.doctor.name} {store.appointment.doctor.surname}</strong></p>
              <p>{t('appointment.specialization')}: <strong>{store.appointment.doctor.specialization.name}</strong></p>
            <Form.Group className="mb-3">
              {store.saved ? (
                  <p>{t('appointment.date')}: <strong>{new Date(store.appointment.startDate).toLocaleDateString()}</strong></p>
                ) : (
                <>
                <Form.Label>{t('personalArea.updateModal.date')}</Form.Label>
                <Row className="justify-content-start text-start">
                  <Col>
                  <DatePicker
                    locale={appointmentStore.getLocale(i18n.language)}
                    dateFormat={appointmentStore.getDateFormat(i18n.language)}
                    selected={new Date(store.newDate)}
                    showTimeSelect
                    onChange={(date: Date) => store.changeDate(date)}
                    minDate={new Date().getHours() > 18 ? (addDays(new Date(), 1)) : (new Date())}
                    maxDate={addDays(new Date(), 14)}
                    minTime={new Date(store.newDate).getDate() == new Date().getDate() ? (new Date()) : (setHours(setMinutes(store.newDate, 0), 9))}
                    maxTime={setHours(setMinutes(store.newDate, 30), 18)}
                    filterDate={isWeekday}
                    excludeTimes={store.excludedTimes}
                    className="mb-3 text-center form-control"
                    withPortal
                  />
                  </Col>
                </Row>
                </>
              )}
              {store.officeError && (
                  <p style={{ color: 'red', fontSize: 14 }}>{t("error.sorry")},{t("error.noOffices")} {t("error.tryToChangeTheInterval")}</p>
              )}
            </Form.Group>
            <Form.Group
              className="mb-3"
            >
              {store.saved && (
                <p>{t('appointment.time')}: <strong>{new Date(store.appointment.startDate).toLocaleTimeString()} - {new Date(store.appointment.endDate).toLocaleTimeString()}</strong></p>
                )}
              
            </Form.Group>
          </Form>
          )}
          </>
          )}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={props.onHide}>
            {t("personalArea.updateModal.close")}
          </Button>
          {store.delete ? (
            <Button variant="outline-danger" onClick={() => {store.deleteAppointment(); profilePageStore.update = true;}}>
            {t("personalArea.updateModal.iwanttodelete")}
            </Button>
          ) : (
            <Button variant="outline-danger" onClick={() => {store.delete = true}}>
            {t("personalArea.updateModal.delete")}
            </Button>
          )}
          {!store.isLoading && !store.saved && !store.delete && (
            <Button variant="success" onClick={store.getFreeOffices}>
              {t("personalArea.updateModal.save")}
            </Button>
          )}
        </Modal.Footer>
      </Modal>
    </>
  );
});

export default UpdateAppointmentModal