import { defineStore } from 'pinia'
import type { IMapConfig, IMarker}  from './../models/locationInfo'
import { 
    getFirestore,
    collection, 
    getDocs
} from 'firebase/firestore'
import { fireApp } from './firebaseConfigStore';
import { MarkerConverter } from './converters/markerConverer';

const db = getFirestore(fireApp)
const COLLECTION_NAME = "markers"

interface IMakerData {
    Markers: IMarker[]
    Config: IMapConfig
}

export const useMarkerStore = defineStore('markerStore',{
    state:()=>({
        markersData:{} as IMakerData
        
    }),
    getters:{},
    actions:{
        with(options: IMapConfig){
            this.markersData.Config = options
            return this
        },
        /// Fetch data from firestore
        async fetch(){
            await getDocs(collection(db,COLLECTION_NAME).withConverter(new MarkerConverter(this.markersData.Config)))
            .then((snapshot)=>{
                  snapshot.docs.forEach(p=>{
                    if(p.exists()){

                    }
                  })
            })
            
        }
    }
});
