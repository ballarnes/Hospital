import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import ownTypes from "../../ioc/ownTypes";
import { Doctor } from "../../models/Doctor";
import { Office } from "../../models/Office";
import { Specialization } from "../../models/Specialization";
import AppointmentService from "../../services/AppointmentService";
import DoctorService from "../../services/DoctorService";
import OfficeService from "../../services/OfficeService";
import SpecializationService from "../../services/SpecializationService";
import ru from 'date-fns/locale/ru';
import en from 'date-fns/locale/en-GB';
import { Appointment } from "../../models/Appointment";
import { setMinutes, setMilliseconds, addMinutes, addHours } from "date-fns";

@injectable()
export default class AppointmentStore {

    specializations: Specialization[] = [];
    appointmentsOnThisDay: Appointment[] = [];
    doctors: Doctor[] = [];
    intervals: Interval[] = [];
    offices: Office[] = [];
    excludedTimes: Date[] = [];
    dateError = false;
    progress = 0;
    authorization = '';
    isLoading = false;
    pageIndex = 0;
    pageSize = 10;
    specialization = 0;
    specializationSelected = false;
    doctor = 0;
    doctorSelected = false;
    date: Date;
    dateSelected = false;
    patientName = '';
    patientNameSelected = false;
    nameError = false;
    office = 0;
    officeSelected = false;
    id = 0;
    appointmentCreated = false;

    constructor(   
        @inject(ownTypes.specializationService) private readonly specializationService: SpecializationService,
        @inject(ownTypes.doctorService) private readonly doctorService: DoctorService,
        @inject(ownTypes.officeService) private readonly officeService: OfficeService,
        @inject(ownTypes.appointmentService) private readonly appointmentService: AppointmentService
   ) {
       makeAutoObservable(this);
       this.date = this.getCurrentDate();
   }

    public init = async (authorization: string) => {
        try {
            this.isLoading = true;
            this.authorization = authorization;
            const result = await this.specializationService.getByPage(this.pageIndex, this.pageSize);
            this.specializations = result?.data ?? [];
            this.specialization = result?.data[0].id ?? 0;
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }

    private getCurrentDate = () => {
        const newDate = new Date();
        if (newDate.getHours() >= 18) {
            newDate.setDate(newDate.getDate() + 1);
            newDate.setHours(0);
            newDate.setMinutes(0);
            return newDate;
        }
        else if (newDate.getHours() < 9) {
            newDate.setHours(9);
            newDate.setMinutes(0);
            return newDate;
        }
        else {
            if (newDate.getMinutes() > 30) {
                return addHours(setMinutes(newDate, 0), 1);
            }
            else {
                return setMinutes(newDate, 30);
            }
        }
    }

    public changeSpecialization = async (id: string) => {
        this.specialization = parseInt(id);
    }

    public selectSpecialization = async () => {
        this.specializationSelected = true;
        this.getDoctorBySpecId();
        this.progress = 33;
    }

    public getDoctorBySpecId = async () => {
        try {
            this.isLoading = true;
            const result = await this.doctorService.getBySpecId(this.specialization);
            this.doctors = result?.data ?? [];
            this.doctor = result?.data[0].id ?? 0;
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
        this.doctorSelected = false;
    }

    public changeDoctor = async (id: string) => {
        this.doctor = parseInt(id);
    }

    public selectDoctor = async () => {
        this.doctorSelected = true;
        this.getAppointmentsByDoctorDate();
        this.progress = 66;
    }

    public changeDate = async (date: Date) => {
        this.date = date;
        this.dateError = false;
        this.getAppointmentsByDoctorDate();
        this.progress = 78;
    }

    private fromDateToString = async (date: Date) => {
        date.setTime(date.getTime() - (date.getTimezoneOffset() * 60000));
        return date.toISOString();
    }

    public getLocale = (language: string) => {
        if (language == "ru") {
            return ru;
        }
        else {
            return en;
        }
    }

    public getDateFormat = (language: string) => {
        if (language == "ru") {
            return "dd/MM/yyyy HH:mm";
        }
        else {
            return "MM/dd/yyyy h:mm aa";
        }
    }

    public changeName = async (name: string) => {
        this.patientName = name;
        this.checkDate(this.date);
        if (!this.dateError) {
            this.patientNameSelected = true;
            this.dateSelected = true;
            this.getFreeOffices();
            this.progress = 100;
        }
    }

    private getFreeOffices = async () => {
        try {
            this.isLoading = true;
            const newDate = await this.fromDateToString(new Date(this.date));
            const result = await this.officeService.getFreeOfficesByDate(newDate);
            if (result?.data != undefined) {
                this.offices = result.data;
                if (this.offices.length > 0) {
                    this.office = result.data[0].id;
                    this.officeSelected = true;
                    this.makeAppointment();
                }
            }

          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.progress = 0;
        this.isLoading = false;
    }

    private getAppointmentsByDoctorDate = async() => {
        this.excludedTimes = [];
        try {
            this.isLoading = true;
            const result = await this.appointmentService.getAppointmentsByDoctorDate(this.doctor, this.date);

            if (result?.data != undefined) {
                this.appointmentsOnThisDay = result.data;
                if (this.appointmentsOnThisDay.length > 0) {
                    this.getExcludedTimes();
                }
            }
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }

    private getExcludedTimes = async () => {
        this.appointmentsOnThisDay.forEach(element => {
            this.excludedTimes.push(new Date(element.startDate));
        })
    }

    private makeAppointment = async () => {
        try {
            console.log(this.date);
            const result = await this.appointmentService.makeAppointment(this.doctor, this.office, await this.fromDateToString(setMilliseconds(this.date, 0)), await this.fromDateToString(setMilliseconds(addMinutes(this.date, 30), 0)), this.patientName, this.authorization);
            if (result?.id != undefined) {
                this.id = result.id
            }
            if (this.id > 0) {
                this.appointmentCreated = true;
            }
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
                this.appointmentCreated = false;
            }
          }
    }

    public checkDate = (date: Date) => {
        this.dateError = false;
        if (date < new Date()) {
            this.dateError = true;
        }
        this.excludedTimes.forEach(element => {
            if (element.getHours() == date.getHours() && element.getMinutes() == date.getMinutes() && element.getDate() == date.getDate()) {
                this.dateError = true;
            }
        })
    }
}