import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import ownTypes from "../../ioc/ownTypes";
import { Doctor } from "../../models/Doctor";
import { Interval } from "../../models/Interval";
import { Office } from "../../models/Office";
import { Specialization } from "../../models/Specialization";
import AppointmentService from "../../services/AppointmentService";
import DoctorService from "../../services/DoctorService";
import IntervalService from "../../services/IntervalService";
import OfficeService from "../../services/OfficeService";
import SpecializationService from "../../services/SpecializationService";
import ru from 'date-fns/locale/ru';
import en from 'date-fns/locale/en-GB';

@injectable()
export default class AppointmentStore {

    specializations: Specialization[] = [];
    doctors: Doctor[] = [];
    intervals: Interval[] = [];
    offices: Office[] = [];
    authorization = '';
    isLoading = false;
    pageIndex = 0;
    pageSize = 10;
    specialization = 0;
    specializationSelected = false;
    doctor = 0;
    doctorSelected = false;
    date: Date = new Date();
    dateSelected = false;
    patientName = '';
    patientNameSelected = false;
    nameError = false;
    interval = 0;
    intervalSelected = false;
    office = 0;
    officeSelected = false;
    id = 0;
    appointmentCreated = false;

    constructor(   
        @inject(ownTypes.specializationService) private readonly specializationService: SpecializationService,
        @inject(ownTypes.doctorService) private readonly doctorService: DoctorService,
        @inject(ownTypes.intervalService) private readonly intervalService: IntervalService,
        @inject(ownTypes.officeService) private readonly officeService: OfficeService,
        @inject(ownTypes.appointmentService) private readonly appointmentService: AppointmentService
   ) {
       makeAutoObservable(this);
   }

    
    public init = async (authorization: string) => {
        try {
            this.isLoading = true;
            this.authorization = authorization;
            const result = await this.specializationService.getByPage(this.pageIndex, this.pageSize);
            this.specializations = result.data;
            this.specialization = result.data[0].id;
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }

    public changeSpecialization = async (id: string) => {
        this.specialization = parseInt(id);
    }

    public selectSpecialization = async () => {
        this.specializationSelected = true;
        this.getDoctorBySpecId();
    }

    public getDoctorBySpecId = async () => {
        try {
            this.isLoading = true;
            const result = await this.doctorService.getBySpecId(this.specialization);
            this.doctors = result.data;
            this.doctor = result.data[0].id;
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
    }

    public changeDate = async (date: Date) => {
        this.date = date;
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
            return "dd/MM/yyyy";
        }
        else {
            return "MM/dd/yyyy";
        }
    }

    public addDays = (date: Date, days: number) => {
        const result = new Date(date);
        result.setDate(result.getDate() + days);
        return result;
    }

    public changeName = async (name: string) => {
        this.patientName = name;
        this.patientNameSelected = true;
        this.dateSelected = true;
        this.getFreeIntervals();
    }

    public getFreeIntervals = async () => {
        try {
            this.isLoading = true;
            const result = await this.intervalService.getFreeIntervals(this.doctor, this.date);
            this.intervals = result.data;
            this.interval = result.data[0].id;
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
        this.intervalSelected = false;
    }

    public changeInterval = async (id: string) => {
        this.interval = parseInt(id);
    }

    public selectInterval = async () => {
        this.intervalSelected = true;
        this.getFreeOffices();
    }

    private getFreeOffices = async () => {
        try {
            this.isLoading = true;
            const result = await this.officeService.getFreeOffices(this.interval, this.date);
            this.offices = result.data;
            if (this.offices.length > 0) {
                this.office = result.data[0].id;
                this.officeSelected = true;
                this.makeAppointment();
            }
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }

    private makeAppointment = async () => {
        try {
            this.isLoading = true;
            const result = await this.appointmentService.makeAppointment(this.doctor, this.interval, this.office, this.date, this.patientName, this.authorization);
            this.id = result.id;
            if (this.id > 0) {
                this.appointmentCreated = true;
            }
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }
}