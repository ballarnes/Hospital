import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import ownTypes from "../../ioc/ownTypes";
import { Appointment } from "../../models/Appointment";
import IntervalService from "../../services/IntervalService";
import { Interval } from "../../models/Interval";
import AppointmentService from "../../services/AppointmentService";

@injectable()
export default class UpdateAppointmentModalStore  {

    intervals: Interval[] = [];
    appointment: Appointment | undefined;
    newDate = new Date();
    isLoading = false;
    pageSize = 2147483647;
    pageIndex = 0;
    interval = 0;
    authorization = '';
    saved = false;
    delete = false;
    deleted = false;

    constructor(   
        @inject(ownTypes.intervalService) private readonly intervalService: IntervalService,
        @inject(ownTypes.appointmentService) private readonly appointmentService: AppointmentService
   ) {
       makeAutoObservable(this);
   }

    public init = async (authorization: string) => {
        this.authorization = authorization;
        this.newDate = this.appointment?.date ?? new Date;
        this.getFreeIntervals();
        this.saved = false;
    }

    public changeDate = async (date: Date) => {
        this.newDate = date;
        this.getFreeIntervals();
    }

    public updateAppointment = async () => {
        try {
            this.isLoading = true;
            const date = new Date(this.newDate);
            date.setDate(date.getDate() + 1);
            const result = await this.appointmentService.updateAppointment(this.appointment?.id ?? 0, this.appointment?.doctorId ?? 0, this.interval, this.appointment?.officeId ?? 0, date, this.appointment?.patientName ?? '', this.authorization);
            if (this.appointment != undefined && result == 200) {
                this.appointment.date = this.newDate;
                this.appointment.interval = this.intervals.filter(i => i.id == this.interval)[0];
            }
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
        this.saved = true;
    }

    public getFreeIntervals = async () => {
        try {
            this.isLoading = true;
            const date = new Date(this.newDate);
            date.setDate(date.getDate() + 1);
            const result = await this.intervalService.getFreeIntervals(this.appointment?.doctorId ?? 0, date);
            this.intervals = result?.data ?? [];
            this.interval = result?.data[0].id ?? 0;
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }

    public changeInterval = async (id: string) => {
        this.interval = parseInt(id);
    }

    public deleteAppointment = async () => {
        try {
            this.isLoading = true;
            const result = await this.appointmentService.deleteAppointment(this.appointment?.id ?? 0, this.authorization);
            if (result == 200) {
                this.deleted = true;
                console.log("deleted");
                this.appointment = undefined;
            }
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }

    public clear = async () => {
        this.saved = false;
        this.delete = false;
        this.deleted = false;
    }
}