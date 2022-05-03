import { observer } from 'mobx-react';
import React, { useState } from 'react'
import { Appointment } from '../../models/Appointment';
import { PencilSquare } from 'react-bootstrap-icons';
import UpdateAppointmentModal from '../UpdateAppointmentModal';

interface Props {
  appointment: Appointment | null
}

const UpcomingAppointment = observer((props: Props) => {
  if (!props.appointment) {
    return null
  }

  const today = new Date().getDate();
  const currentHour = new Date().getHours();
  const date = Number(props.appointment.date.toString().substring(8, 10));
  const start = Number(props.appointment.interval.start.toString().split(':')[0]);

  const [modalShow, setModalShow] = useState(false);

  return (
    <tr>
      <td>{props.appointment.id}</td>
      <td>{props.appointment.doctor.name}</td>
      <td>{props.appointment.doctor.surname}</td>
      <td>{props.appointment.doctor.specialization.name}</td>
      <td>{props.appointment.interval.start.hours}:{props.appointment.interval.start.minutes}{props.appointment.interval.start.minutes == 0 && 0} - {props.appointment.interval.end.hours}:{props.appointment.interval.end.minutes}{props.appointment.interval.end.minutes == 0 && 0}</td>
      <td>{new Date(props.appointment.date).toLocaleDateString()}</td>
      <td>{props.appointment.office.number}</td>
      {(today == date && currentHour + 3 > start) ? (<><td></td></>) : (
        <>
          <td><PencilSquare style={{ cursor: "pointer" }} onClick={() => {
            setModalShow(true);
            }}/>
          </td>
        </>     
      )}
      <UpdateAppointmentModal show={modalShow} onHide={() => setModalShow(false)} appointment={props.appointment}/>
    </tr>
  )
});

export default UpcomingAppointment