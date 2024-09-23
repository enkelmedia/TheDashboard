import { ManifestTdWidget } from '../../core/td-widget.js'

const widget : ManifestTdWidget = {
	type: "tdWidget",
	alias: "Td.Widget.MyActivities",
	name: "The Dashboard Widget My Activities",
  weight: 100,
  element : ()=>import('./widget-my-activities.element.ts'),
  meta : {
    width:4,
    height:7
  }
}

export const manifests = [
  widget
]
