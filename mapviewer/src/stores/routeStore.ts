import { defineStore } from 'pinia'
import type {IRoute}  from './../models/locationInfo'
import { 
    collection,
    getFirestore,
    query,
    getDocs
} from 'firebase/firestore'
import { fireApp } from './firebaseConfigStore';
import { RouteConverter } from './converters/routeConverter';

const db = getFirestore(fireApp)
const COLLECTION_NAME="route";

export const useRoutesStore = defineStore('routeStore',{
     state: () =>({
        routes: [] as IRoute[]
    }),
    getters:{
    },
    actions:{
        async fetch(){
            await getDocs(query(collection(db,COLLECTION_NAME)).withConverter(new RouteConverter()))
            .then((snapshot) =>
                snapshot.docs.forEach((p) => {
                    if (p.exists()) 
                        (console.log(p.data()))
                        this.routes.push(p.data() as IRoute)
                    })
            )
        }
    }
    });
