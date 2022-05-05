import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import ownTypes from "../../ioc/ownTypes";
import { Appointment } from "../../models/Appointment";
import { Office } from "../../models/Office";
import AppointmentService from "../../services/AppointmentService";
import OfficeService from "../../services/OfficeService";
import { setMilliseconds, addMinutes } from "date-fns";

@injectable()
export default class UpdateAppointmentModalStore  {

    appointment: Appointment | undefined;
    appointmentsOnThisDay: Appointment[] = [];
    offices: Office[] = [];
    newDate = new Date();
    excludedTimes: Date[] = [];
    isLoading = false;
    dateError = false;
    pageSize = 2147483647;
    pageIndex = 0;
    authorization = '';
    saved = false;
    delete = false;
    deleted = false;
    office = 0;
    officeError = false;

    constructor(
        @inject(ownTypes.appointmentService) private readonly appointmentService: AppointmentService,
        @inject(ownTypes.officeService) private readonly officeService: OfficeService
   ) {
       makeAutoObservable(this);
   }

    public init = async (authorization: string) => {
        this.authorization = authorization;
        this.newDate = this.appointment?.startDate ?? new Date;
        this.saved = false;
    }

    public changeDate = async (date: Date) => {
        this.newDate = date;
        this.dateError = false;
        this.getAppointmentsByDoctorDate();
    }

    private fromDateToString = async (date: Date) => {
        date.setTime(date.getTime() - (date.getTimezoneOffset() * 60000));
        return date.toISOString();
    }

    public updateAppointment = async () => {
        try {
            this.isLoading = true;
            const result = await this.appointmentService.updateAppointment(this.appointment?.id ?? 0, this.appointment?.doctorId ?? 0, this.office ?? 0, await this.fromDateToString(setMilliseconds(this.newDate, 0)), await this.fromDateToString(setMilliseconds(addMinutes(this.newDate, 30), 0)), this.appointment?.patientName ?? '', this.authorization);
            if (this.appointment != undefined && result == 200) {
                this.appointment.startDate = this.newDate;
                this.appointment.endDate = new Date(this.newDate.getTime() + 30*60000);
            }
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
        this.saved = true;
    }

    private getAppointmentsByDoctorDate = async() => {
        this.excludedTimes = [];
        try {
            this.isLoading = true;
            const result = await this.appointmentService.getAppointmentsByDoctorDate(this.appointment?.doctorId ?? 0, this.newDate);

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

    public getFreeOffices = async () => {
        try {
            this.isLoading = true;
            const newDate = await this.fromDateToString(new Date(this.newDate));
            const result = await this.officeService.getFreeOfficesByDate(newDate);
            if (result?.data != undefined) {
                this.offices = result.data;
                if (this.offices.length > 0) {
                    this.office = result.data[0].id;
                    this.officeError = false;
                    this.updateAppointment();
                }
                else {
                    this.officeError = true;
                }
            }

          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
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