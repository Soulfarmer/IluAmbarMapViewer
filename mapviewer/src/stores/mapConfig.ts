import { defineStore } from 'pinia'
import { 
    getFirestore, 
    getDoc,
    doc
} from 'firebase/firestore'
import { fireApp } from './firebaseConfigStore';

const db = getFirestore(fireApp)
const COLLECTION_NAME = "configuration"

export const useMapConfigStore = defineStore('mapConfigStore',{
    state:()=>({
       options:""
    }),
    getters:{},
    actions:{
        /// Fetch data from firestore
        async fetch(){
            await getDoc(doc(db,COLLECTION_NAME,"map"))
            .then(
                (snapshot)=>{
                    console.log(snapshot.data())
                }
            )
        }
    }
});