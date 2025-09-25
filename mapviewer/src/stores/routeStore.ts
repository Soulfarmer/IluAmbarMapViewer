import { defineStore } from 'pinia'
import type {IRoute}  from './../models/locationInfo'

export type IvRoute={
    routes :IRoute[]
}

export const useRoutesStore = defineStore('routeStore',{
     state:():IvRoute=>({
        routes: [
            {Name:"",Waypoints:[{Latitude:0,Longitude:0,},{Latitude:0,Longitude:100}]},
            {Name:"",Waypoints:[{Latitude:250,Longitude:0,},{Latitude:250,Longitude:100}]}
        ]
    })
    });
