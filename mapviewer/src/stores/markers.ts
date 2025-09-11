import { defineStore } from 'pinia'
import type { LocationInfo}  from './../models/locationInfo'


export type IMarker ={
    locations : LocationInfo[];
}

export const useMarkerStore = defineStore('markerStore',{
    state:():IMarker=>({
        locations:[{Latitude:10, Longitude:10, Title:'test'}]
    })
})