import { initializeApp } from 'firebase/app'
import { getAnalytics } from 'firebase/analytics'

const firebaseConfig = {

  apiKey: "AIzaSyBnhpQ7x1-mW6tp2Uf9lmEcXeBmiF-GQzA",

  authDomain: "ilu-ambar-ce3ed.firebaseapp.com",

  projectId: "ilu-ambar-ce3ed",

  storageBucket: "ilu-ambar-ce3ed.firebasestorage.app",

  messagingSenderId: "1007067266639",

  appId: "1:1007067266639:web:c660583358a991335db43a"

};

export const fireApp = initializeApp(firebaseConfig,"IluAmbar")
export const analytics = getAnalytics(fireApp)