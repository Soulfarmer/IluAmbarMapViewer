<template>
  <l-feature-group :ref="(el)=>fg=el" :name="props.Name">
    <l-marker ref="markers" v-for="(m,index) in props.Markers" 
        :key="index" 
        v-bind:lat-lng="getLatLon(m.Coords)">
    <l-tooltip>{{ m.Title }}<br/>{{ (m.Coords) }}<br/>@{{ m.PlayerName }}</l-tooltip></l-marker>
  </l-feature-group>
</template>
<script setup lang="ts">
import type { IMarker, LocationBase } from "@/models/locationInfo";
import { LFeatureGroup,LMarker,LTooltip } from "@vue-leaflet/vue-leaflet";
import {  onMounted, ref } from 'vue'

interface Props {
  Name: string
  Markers: IMarker[]
  Control:LocationBase
}

const props = defineProps<Props>()
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const fg = ref<typeof LFeatureGroup | any>(null)
const markers = ref<typeof LMarker[] | null>(null)
const emit = defineEmits(['initLayer'])

onMounted(()=>{
    emit('initLayer', fg.value,props.Name)
})

const getLatLon = (coords:LocationBase)=>{
      return [coords.Latitude+props.Control.Latitude,coords.Longitude+props.Control.Longitude] as L.LatLngExpression
  }
</script>

