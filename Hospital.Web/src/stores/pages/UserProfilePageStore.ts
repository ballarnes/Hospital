import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import ownTypes from "../../ioc/ownTypes";
import { Appointment } from "../../models/Appointment";
import AppointmentService from "../../services/AppointmentService";
import IntervalService from "../../services/IntervalService";
import { Interval } from "../../models/Interval";

@injectable()
export default class UserProfilePageStore  {

    appointments: Appointment[] = [];
    intervals: Interval[] = [];
    appointmentToEdit: Appointment | undefined;
    isLoading = false;
    pageSize = 5;
    pageIndex = 0;
    pagesCount = 0;
    totalCount = 0;
    name = '';
    interval = 0;
    update = false;

    constructor(   
        @inject(ownTypes.appointmentService) private readonly appointmentService: AppointmentService,
        @inject(ownTypes.intervalService) private readonly intervalService: IntervalService
   ) {
       makeAutoObservable(this);
   }

    public init = async () => {
        try {
            this.update = false;
            this.isLoading = true;
            const result = await this.appointmentService.getUpcomingAppointments(this.pageIndex, this.pageSize, this.name);
            this.appointments = result?.data ?? [];
            if (this.appointments.length == 0) {
                this.pageIndex -= 1;
            }
            this.pagesCount = result?.pagesCount ?? 0;
            this.totalCount = result?.totalCount ?? 0;
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }

    public changeDate = async (date: Date) => {
        if (this.appointmentToEdit != undefined) {
            this.appointmentToEdit.date = date;
            console.log(this.appointmentToEdit.date);
            this.getFreeIntervals();
        }
    }

    public getFreeIntervals = async () => {
        try {
            this.isLoading = true;
            const result = await this.intervalService.getFreeIntervals(this.appointmentToEdit?.doctorId ?? 0, this.appointmentToEdit?.date ?? new Date);
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

    public changePage = async (number: number) => {
        this.pageIndex = number - 1;
    }
}