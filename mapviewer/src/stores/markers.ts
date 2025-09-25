import { defineStore } from 'pinia'
import type { LocationInfo}  from './../models/locationInfo'

export type IMarker ={
    markers : LocationInfo[];
}

export const useMarkerStore = defineStore('markerStore',{
    state:():IMarker=>({
        markers:[{Latitude:0, Longitude:0, Title:'Spawn'}],
    }),
    getters:{},
    actions:{
        /// Fetch data from firestore
        fetch(){

        }
    }
});
