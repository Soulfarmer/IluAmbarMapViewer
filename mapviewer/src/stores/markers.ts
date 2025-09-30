import { defineStore } from 'pinia'
import type { IMarker}  from './../models/locationInfo'
import { 
    getFirestore,
    collection, 
    getDocs
} from 'firebase/firestore'
import { fireApp } from './firebaseConfigStore';
import { MarkerConverter } from './converters/markerConverer';

const db = getFirestore(fireApp)
const COLLECTION_NAME = "markers"

export const useMarkerStore = defineStore('markerStore',{
    state:()=>({
        Markers:[] as IMarker[],
        
    }),
    getters:{},
    actions:{
        /// Fetch data from firestore
        async fetch(){
            await getDocs(collection(db,COLLECTION_NAME).withConverter(new MarkerConverter()))
            .then((snapshot)=>{
                  snapshot.docs.forEach(p=>{
                    if(p.exists()){
                        console.log(p.data())
                        this.Markers.push(p.data() as IMarker)
                    }
                  })
            })
        }
    }
});
