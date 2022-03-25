import "reflect-metadata";
import { injectable } from "inversify";
import { makeAutoObservable } from "mobx";

export enum TabsType {
  Appointment,
  Information
}

@injectable()
export default class HomePageStore {

    currentTab = TabsType[TabsType.Information];

    constructor(   
   ) {
       makeAutoObservable(this);
   }

    public changeTab = (tab: string | null) : void => {
      this.currentTab = !!tab ? tab : TabsType[TabsType.Information];
    }
}