import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import ownTypes from "../../ioc/ownTypes";
import { Appointment } from "../../models/Appointment";
import AppointmentService from "../../services/AppointmentService";

@injectable()
export default class UserProfilePageStore  {

    appointments: Appointment[] = [];
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
        @inject(ownTypes.appointmentService) private readonly appointmentService: AppointmentService
   ) {
       makeAutoObservable(this);
   }

    public init = async () => {
        try {
            this.update = false;
            this.isLoading = true;
            if (this.pageIndex < 0) {
                this.pageIndex = 0;
            }
            const result = await this.appointmentService.getUpcomingAppointments(this.pageIndex, this.pageSize, this.name);
            this.appointments = result?.data ?? [];
            if (this.appointments.length == 0) {
                this.pageIndex = 0;
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
            this.appointmentToEdit.startDate = date;
        }
    }

    public changePage = async (number: number) => {
        this.pageIndex = number - 1;
    }
}