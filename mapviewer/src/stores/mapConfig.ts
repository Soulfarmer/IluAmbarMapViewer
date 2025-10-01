import { defineStore } from 'pinia'
import { 
    getFirestore, 
    getDoc,
    doc
} from 'firebase/firestore'
import { fireApp } from './firebaseConfigStore';
import type { IMapConfig, LocationBase } from '@/models/locationInfo';

const db = getFirestore(fireApp)
const COLLECTION_NAME = "configuration"

export const useMapConfigStore = defineStore('mapConfigStore',{
    state:()=>({
       options:{} as IMapConfig
    }),
    getters:{
        spawnControl:(state):LocationBase=>{
            return state.options.spawnControl
        }
    },
    actions:{
        /// Fetch data from firestore
        async fetch(){
            await getDoc(doc(db,COLLECTION_NAME,"map"))
            .then(
                (snapshot)=>{
                    if (snapshot.exists()){
                        const coords = snapshot.get("spawn-control").split(":")
                        this.options.spawnControl = {
                            Latitude:parseFloat(coords[0]),
                            Longitude:parseFloat(coords[1])
                        } as LocationBase
                    }
                }
            )
        }
    }
});