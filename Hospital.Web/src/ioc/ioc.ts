import { Container } from 'inversify';
import type { HttpService } from '../services/HttpService';
import DefaultHttpService from '../services/HttpService';
import type { SpecializationService } from '../services/SpecializationService';
import DefaultSpecializationService from '../services/SpecializationService';
import type { DoctorService } from '../services/DoctorService';
import DefaultDoctorService from '../services/DoctorService';
import HomePageStore from '../stores/pages/HomePageStore';
import SpecializationsPageStore from '../stores/pages/SpecializationsPageStore';
import { AppointmentStore, UpdateAppointmentModalStore }  from '../stores/components'
import ownTypes from './ownTypes';
import { OfficeService } from '../services/OfficeService';
import DefaultOfficeService from '../services/OfficeService';
import { AppointmentService } from '../services/AppointmentService';
import DefaultAppointmentService from '../services/AppointmentService';
import { InformationStore }  from '../stores/components'
import UserProfilePageStore from '../stores/pages/UserProfilePageStore';

export const container = new Container();
container.bind<HttpService>(ownTypes.httpService).to(DefaultHttpService).inSingletonScope();
container.bind<SpecializationService>(ownTypes.specializationService).to(DefaultSpecializationService).inSingletonScope();
container.bind<DoctorService>(ownTypes.doctorService).to(DefaultDoctorService).inSingletonScope();
container.bind<OfficeService>(ownTypes.officeService).to(DefaultOfficeService).inSingletonScope();
container.bind<AppointmentService>(ownTypes.appointmentService).to(DefaultAppointmentService).inSingletonScope();

container.bind<HomePageStore>(ownTypes.homePageStore).to(HomePageStore).inTransientScope();
container.bind<SpecializationsPageStore>(ownTypes.specializationsPageStore).to(SpecializationsPageStore).inTransientScope();
container.bind<UserProfilePageStore>(ownTypes.userProfilePageStore).to(UserProfilePageStore).inSingletonScope();
container.bind<UpdateAppointmentModalStore>(ownTypes.updateAppointmentModalStore).to(UpdateAppointmentModalStore).inTransientScope();

container.bind<AppointmentStore>(ownTypes.appointmentStore).to(AppointmentStore).inTransientScope();
container.bind<InformationStore>(ownTypes.informationStore).to(InformationStore).inTransientScope();